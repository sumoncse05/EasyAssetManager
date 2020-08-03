CREATE OR REPLACE PACKAGE BODY ERMP.pkg_portfolio_manager_rmlia
AS
--------------------------------------------------------------------------------
PROCEDURE dpd_set_generatebalance(
    pnm_run_id        IN number,
    pvc_batch_id      IN varchar2,
    pvc_appuser       IN varchar2
)
AS  
    vnm_businessyear       number:= to_number(to_char(sysdate,'rrrr'));
    vdt_workdate           date:= trunc(sysdate);    
    vdt_currbusinessdate   date;
    vdt_prevbusinessdate   date;
    vdt_monbusinessdate    date;
    vdt_qtrbusinessdate    date;
    vdt_yendbusinessdate   date;
    
    vvc_downldstatus       varchar2(1):='N';
    vvc_processstatus      varchar2(1):='N';
    vmn_countdata          number:=0;
    vvc_status             varchar2(20);
    vvc_statusmsg          varchar2(500);
    vvc_err_loc            varchar2(100) := 'pkg_portfolio_manager.dpd_set_generatebalance';   
    
BEGIN
    
    BEGIN
        SELECT today,prev_working_day 
          INTO vdt_currbusinessdate,vdt_prevbusinessdate
          FROM sttm_dates@ubs_bo_lnk
         WHERE branch_code='999';
         
        SELECT lia_downld_status,lia_process_status 
          INTO vvc_downldstatus,vvc_processstatus
          FROM rmp_work_schedule
         WHERE business_date = vdt_currbusinessdate;
         
    EXCEPTION WHEN no_data_found THEN
        vvc_downldstatus:= 'N';
        vvc_processstatus:= 'N';
        
        INSERT INTO rmp_work_schedule
        (work_date,business_date,prev_business_date,lia_downld_status,
         lia_downld_date,lia_process_status,lia_process_date,ins_by,ins_date)
        VALUES
        (vdt_workdate,vdt_currbusinessdate,vdt_prevbusinessdate,'N',
         '','N','',pvc_appuser,sysdate);
        
    END;        
    
    SELECT dfn_get_business_date(vdt_workdate,'M'),dfn_get_business_date(vdt_workdate,'Q'),dfn_get_business_date(vdt_workdate,'Y')
      INTO vdt_monbusinessdate, vdt_qtrbusinessdate, vdt_yendbusinessdate
      FROM dual;
                
    /*SELECT to_char(to_date(process_month,'mm/rrrr'),'rrrrmm'),to_date(process_month,'mm/rrrr'),last_day(to_date(process_month,'mm/rrrr'))
      INTO vvc_cyclecode,vdt_startdate,vdt_enddate
      FROM utl_batch_control
     WHERE run_id=pnm_run_id;*/
    
    IF vvc_downldstatus = 'N' THEN
        -- clear temporary tables 
        DELETE FROM rmp_account_balance_tmp;
        DELETE FROM rmp_customer_priority_tmp;        
        DELETE FROM rmp_customer_tmp;
        DELETE FROM rmp_customer_balance_tmp;
        
        --Archive old account balances -----------------------------------------
        INSERT INTO rmp_account_balance_hist
        SELECT * FROM rmp_account_balance_dtl
         WHERE business_date>vdt_monbusinessdate
           AND business_date<vdt_prevbusinessdate;
        
        DELETE FROM rmp_account_balance_dtl
         WHERE business_date>vdt_monbusinessdate
           AND business_date<vdt_prevbusinessdate;
              
        -- clear main tables 
        DELETE FROM rmp_account_balance_dtl 
         WHERE business_date = vdt_currbusinessdate;
        
        DELETE FROM rmp_customer_balance_dtl 
         WHERE business_date = vdt_currbusinessdate;
                            
        --Collect all account balance for consumer AND commercial              
        INSERT INTO rmp_account_balance_tmp
        (business_date,branch_code,cust_no,cust_ac_no,ac_open_date,account_class,balance,ins_date,ins_by)
        SELECT vdt_currbusinessdate,branch_code,cust_no,cust_ac_no,ac_open_date,account_class,acy_avl_bal,sysdate,pvc_appuser
          FROM sttm_cust_account@ubs_bo_lnk
         WHERE record_stat='O'
           AND account_class IN (SELECT account_class FROM rmp_account_class);
        
        -- SET negative balance to 0 as positive balance are liability only  
        UPDATE rmp_account_balance_tmp
           SET balance = 0
         WHERE balance < 0 OR balance is null;
        
        INSERT INTO rmp_account_balance_dtl
        (business_date,branch_code,cust_no,cust_ac_no,ac_open_date,account_class,balance,ins_date,ins_by)
        SELECT vdt_currbusinessdate,branch_code,cust_no,cust_ac_no,ac_open_date,account_class,balance,ins_date,ins_by
          FROM rmp_account_balance_tmp;
          
        -- account growth calculation
        /*
        UPDATE rmp_account_balance_dtl x 
           SET x.growth = (SELECT growth FROM
                               (WITH today_data as(
                                       SELECT business_date, cust_ac_no, balance
                                         FROM rmp_account_balance_dtl
                                       WHERE business_date = vdt_currbusinessdate),
                                    lastday_data as(
                                        SELECT business_date, cust_ac_no, balance 
                                       FROM rmp_account_balance_hist
                                       WHERE business_date = vdt_prevbusinessdate)           
                                 SELECT today_data.cust_ac_no, today_data.balance - lastday_data.balance growth
                                    FROM today_data, lastday_data
                                   WHERE today_data.cust_ac_no = lastday_data.cust_ac_no
                               ) val
                               WHERE  x.cust_ac_no   = val.cust_ac_no
                                 AND x.business_date = vdt_currbusinessdate
                            );
        */
        
        FOR c_growth IN(SELECT cb.cust_ac_no,nvl(cb.balance,0)-nvl(pb.balance,0) growth
                          FROM rmp_account_balance_dtl cb,rmp_account_balance_hist pb
                         WHERE cb.cust_ac_no = pb.cust_ac_no(+)
                           AND cb.business_date = vdt_currbusinessdate
                           AND pb.business_date(+) = vdt_prevbusinessdate)
        LOOP
            UPDATE rmp_account_balance_dtl
               SET growth = c_growth.growth
             WHERE cust_ac_no = c_growth.cust_ac_no
               AND business_date = vdt_currbusinessdate;
               
        END LOOP;
        
        INSERT INTO rmp_customer_priority_tmp
        (cust_no,cat_id)
        SELECT substr(rec_key,1,7) cust_no,'02'
          FROM cstm_function_userdef_fields@ubs_bo_lnk
         WHERE function_id ='STDCIF'
           AND field_val_4 in ('1001','1002','1003');
        
        INSERT INTO rmp_customer_tmp
        (business_date,cust_no,customer_type,customer_name,cif_creation_date,ins_date,ins_by)
        SELECT vdt_currbusinessdate,customer_no,customer_type,customer_name1,cif_creation_date,sysdate,pvc_appuser
          FROM sttm_customer@ubs_bo_lnk;
        
        DELETE FROM rmp_customer_tmp
         WHERE cust_no IN (SELECT cust_no FROM rmp_customer);                  
        
        -- append new customer into customer main table         
        INSERT INTO rmp_customer
        (cust_no,customer_name,cat_id,customer_type,cif_creation_date,ins_date,ins_by)
        SELECT c.cust_no,customer_name,nvl2(p.cust_no,'02','01'),customer_type,cif_creation_date,sysdate,pvc_appuser
          FROM rmp_customer_tmp c,rmp_customer_priority_tmp p
         WHERE c.cust_no=p.cust_no(+)
           AND c.cust_no IN (SELECT cust_no FROM rmp_account_balance_tmp);                
        
        INSERT INTO rmp_customer_balance_tmp
        (business_date,branch_code,cust_no,onboard_date,customer_type,cd_balance,savings_balance,snd_balance,rd_balance,fd_balance,ins_date,ins_by)
        SELECT vdt_currbusinessdate, a.branch_code,a.cust_no,min(ac_open_date),customer_type,sum(decode(ac_type_code,'01',balance,0))cd_balance,sum(decode(ac_type_code,'02',balance,0))sb_balance,
               sum(decode(ac_type_code,'03',balance,0))snd_balance,sum(decode(ac_type_code,'04',balance,0))rd_balance,
               sum(decode(ac_type_code,'05',balance,0))fd_balance,sysdate,pvc_appuser
          FROM rmp_account_balance_tmp a, rmp_customer c, rmp_account_class l
         WHERE a.cust_no=c.cust_no
           AND a.account_class = l.account_class
         GROUP by a.branch_code,a.cust_no,customer_type;
        
        UPDATE rmp_customer_balance_tmp
           SET tot_balance = cd_balance+savings_balance+snd_balance+rd_balance+fd_balance;
        
        INSERT INTO rmp_customer_balance_dtl
        (business_date,branch_code,cust_no,cd_balance,savings_balance,snd_balance,rd_balance,fd_balance,tot_balance,ins_date,ins_by)
        SELECT vdt_currbusinessdate,branch_code,cust_no,cd_balance,savings_balance,snd_balance,rd_balance,fd_balance,tot_balance,ins_date,ins_by
          FROM rmp_customer_balance_tmp;
          
        --growth calculation for customer
          
        /*UPDATE rmp_customer_balance_dtl c_groth 
           SET (cd_growth, savings_growth, snd_growth, rd_growth, fd_growth, tot_growth) = (
                SELECT cdgrowth, savinggrowth, sndgrowth, rdgrowth, fdgrowth, totgrowth FROM
                   (WITH today_data as(
                           SELECT business_date, cust_no, branch_code, cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance
                             FROM rmp_customer_balance_dtl
                           WHERE business_date = vdt_currbusinessdate),
                        lastday_data as(
                            SELECT business_date, cust_no, branch_code, cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance
                           FROM rmp_customer_balance_dtl
                           WHERE business_date = vdt_prevbusinessdate)           
                     SELECT today_data.cust_no,today_data.branch_code, today_data.cd_balance - lastday_data.cd_balance cdgrowth,
                            today_data.savings_balance - lastday_data.savings_balance savinggrowth, today_data.snd_balance - lastday_data.snd_balance sndgrowth,
                            today_data.rd_balance - lastday_data.rd_balance rdgrowth, today_data.fd_balance - lastday_data.fd_balance fdgrowth,
                            today_data.tot_balance - lastday_data.tot_balance totgrowth
                       FROM today_data,lastday_data
                      WHERE today_data.cust_no = lastday_data.cust_no
                        AND today_data.branch_code = lastday_data.branch_code
                   ) val_c
                   WHERE c_groth.cust_no = val_c.cust_no
                     AND c_groth.branch_code = val_c.branch_code
                     AND c_groth.business_date = vdt_currbusinessdate
                );*/
              
              FOR cus_growth IN(SELECT cb.cust_no,cb.branch_code,nvl(cb.cd_balance,0)-nvl(pb.cd_balance,0) cd_growth,
                                       nvl(cb.savings_balance,0)-nvl(pb.savings_balance,0) savings_growth,nvl(cb.snd_balance,0)-nvl(pb.snd_balance,0) snd_growth,
                                       nvl(cb.rd_balance,0)-nvl(pb.rd_balance,0) rd_growth,nvl(cb.fd_balance,0)-nvl(pb.fd_balance,0) fd_growth,
                                       nvl(cb.tot_balance,0)-nvl(pb.tot_balance,0) tot_growth
                                  FROM rmp_customer_balance_dtl cb,rmp_customer_balance_dtl pb
                                 WHERE cb.cust_no = pb.cust_no(+)
                                   AND cb.branch_code=pb.branch_code
                                   AND cb.business_date = vdt_currbusinessdate
                                   AND pb.business_date(+) = vdt_prevbusinessdate)
                LOOP
                    UPDATE rmp_customer_balance_dtl
                       SET cd_growth = cus_growth.cd_growth,savings_growth = cus_growth.savings_growth,snd_growth = cus_growth.snd_growth,rd_growth = cus_growth.rd_growth,
                           fd_growth = cus_growth.fd_growth,tot_growth = cus_growth.tot_growth
                     WHERE cust_no = cus_growth.cust_no
                       AND branch_code=cus_growth.branch_code
                       AND business_date = vdt_currbusinessdate;
                       
                END LOOP;
        
        UPDATE rmp_work_schedule
           SET lia_downld_status = 'Y'
         WHERE business_date = vdt_currbusinessdate;
        
        commit;
    END IF;
    
    IF vvc_processstatus = 'N' THEN 
    
        --RM Movement Log
       FOR rm_movement IN(SELECT rm_slno,emp_id,old_rm_code,new_rm_code,old_branch_code,new_branch_code,status_code,eff_date 
                            FROM rmp_rm_movement_log 
                            WHERE eff_date<=vdt_currbusinessdate AND auth_date is null)
        LOOP
            CASE rm_movement.status_code
              WHEN '01' THEN --Transfer                  
                    -- Update new branch        
                  UPDATE rmp_rm_dtl
                     SET branch_code = rm_movement.new_branch_code, upd_date = sysdate, upd_by = pvc_appuser
                   WHERE  rm_code = rm_movement.old_rm_code;
                  
                  -- Insert new Record for assigning RM in RMP_RM_CUSTOMER
                  INSERT INTO rmp_rm_customer(branch_code, cust_no, rm_code, eff_date, ins_date, ins_by)
                                       SELECT branch_code, cust_no, rm_movement.new_rm_code, rm_movement.eff_date, sysdate, pvc_appuser 
                                         FROM rmp_rm_customer WHERE rm_code=rm_movement.old_rm_code;
                  
                  -- Update exp_date to RMP_RM_CUSTOMER            
                  UPDATE rmp_rm_customer 
                     SET exp_date = rm_movement.eff_date, upd_date = sysdate, upd_by = pvc_appuser 
                   WHERE rm_code=rm_movement.old_rm_code;
                  
                  --Update movement_log table auth_date AND auth_by
                  UPDATE rmp_rm_movement_log 
                     SET auth_by=pvc_appuser, auth_date = sysdate 
                   WHERE rm_slno = rm_movement.rm_slno;
                
              WHEN '02' THEN --Resigned    
                   --Update status to RMP_RM_DTL           
                   UPDATE rmp_rm_dtl
                     SET status = 'N', resign_date = rm_movement.eff_date, upd_date = sysdate, upd_by = pvc_appuser
                   WHERE  rm_code = rm_movement.old_rm_code;
                  
                  --Insert new Record for assigning RM in RMP_RM_CUSTOMER
                  INSERT INTO rmp_rm_customer(branch_code,cust_no,rm_code,eff_date,ins_date,ins_by)
                                       SELECT branch_code,cust_no, rm_movement.new_rm_code, rm_movement.eff_date, sysdate,pvc_appuser
                          FROM rmp_rm_customer 
                         WHERE rm_code=rm_movement.old_rm_code;
                         
                  --Update exp_date to RMP_RM_CUSTOMER
                  UPDATE rmp_rm_customer 
                     SET exp_date = rm_movement.eff_date, upd_date = sysdate, upd_by = pvc_appuser 
                   WHERE rm_code = rm_movement.old_rm_code;
                   
                   --Update movement_log table auth_date AND auth_by
                  UPDATE rmp_rm_movement_log 
                     SET auth_by = pvc_appuser, auth_date = sysdate 
                   WHERE rm_slno = rm_movement.rm_slno;
              
              WHEN '03' THEN --RM code update  
                  --Insert into RMP_RM_DTL with new RMCode AND inactive previous record            
                  INSERT INTO rmp_rm_dtl(emp_id, emp_name, emp_cat, desig_code, grade_code, dept_code, rm_code, branch_code, cat_id, email, 
                                         mobile, status, resign_date, ins_date, ins_by)
                                  SELECT emp_id, emp_name, emp_cat, desig_code, grade_code, dept_code, rm_movement. new_rm_code, branch_code, cat_id, email,  
                                         mobile, status, resign_date, sysdate, pvc_appuser 
                                    FROM rmp_rm_dtl 
                                   WHERE rm_code = rm_movement.old_rm_code;
              
                  UPDATE rmp_rm_dtl
                     SET status = 'N',upd_date=sysdate,upd_by=pvc_appuser
                   WHERE  rm_code = rm_movement.old_rm_code;
                   
                   --Update rmcode to rmp_rm_dtl , rmp_rm_balance_dtl, rmp_rm_balance_sum , rmp_rm_customer, rmp_rm_customer_log
                  UPDATE rmp_rm_customer 
                     SET rm_code = rm_movement.new_rm_code, upd_date = sysdate, upd_by = pvc_appuser 
                   WHERE rm_code = rm_movement.old_rm_code;
                   
                  UPDATE rmp_rm_balance_dtl 
                     SET rm_code = rm_movement.new_rm_code 
                   WHERE rm_code = rm_movement.old_rm_code;
                   
                  UPDATE rmp_rm_balance_sum  
                     SET rm_code = rm_movement.new_rm_code 
                   WHERE rm_code = rm_movement.old_rm_code;
                  UPDATE rmp_rm_customer 
                     SET rm_code = rm_movement.new_rm_code,upd_date = sysdate,upd_by = pvc_appuser 
                   WHERE rm_code = rm_movement.old_rm_code;
                   
                  UPDATE rmp_rm_customer_log 
                     SET new_rm_code = rm_movement.new_rm_code, upd_date = sysdate, upd_by = pvc_appuser 
                   WHERE old_rm_code = rm_movement.old_rm_code;
                  
                  --Update movement_log table auth_date AND auth_by
                  UPDATE rmp_rm_movement_log 
                     SET auth_by = pvc_appuser,auth_date = sysdate 
                   WHERE rm_slno = rm_movement.rm_slno;
                  
              WHEN '04' THEN -- (Leave) 
                   --  Update exp_date to rmp_rm_customer
                   INSERT INTO rmp_rm_customer(branch_code, cust_no, rm_code, eff_date, ins_date, ins_by)
                        SELECT branch_code, cust_no, rm_movement.new_rm_code, rm_movement.eff_date, sysdate, pvc_appuser 
                          FROM rmp_rm_customer WHERE rm_code = rm_movement.old_rm_code;
                     
                   -- Insert new Record for assigning RM in RMP_RM_CUSTOMER
                  UPDATE rmp_rm_customer 
                     SET exp_date = rm_movement.eff_date, upd_date = sysdate, upd_by = pvc_appuser
                   WHERE rm_code = rm_movement.old_rm_code;
                   
                   --Update movement_log table auth_date AND auth_b
                  UPDATE rmp_rm_movement_log 
                     SET auth_by = pvc_appuser, auth_date = sysdate 
                   WHERE rm_slno = rm_movement.rm_slno;                  
            END CASE;
            
        END LOOP;      
        
      --Add all the new account to sum table with curr balance for all field mtd,qtd,ytd ------
        INSERT INTO rmp_account_balance_sum
              (business_year,branch_code,cust_no,cust_ac_no,ac_open_date,account_class,business_date,balance,dtd_date,
               balance_dtd,mtd_date,balance_mtd,qtd_date,balance_qtd,ytd_date,balance_ytd,ins_date,ins_by)
        SELECT vnm_businessyear,branch_code,cust_no,cust_ac_no,ac_open_date,account_class,business_date,balance,business_date,
               balance,business_date,balance,business_date,balance,business_date,balance,ins_date,ins_by
          FROM rmp_account_balance_dtl
         WHERE business_date = vdt_currbusinessdate
           AND cust_ac_no IN (SELECT cust_ac_no FROM rmp_account_balance_dtl
                               WHERE business_date = vdt_currbusinessdate
                               MINUS
                              SELECT cust_ac_no FROM rmp_account_balance_sum
                               WHERE business_year = vnm_businessyear);       
                                
        -- transfer todays balance into yesterdays balance ---------------------------------------
        UPDATE rmp_account_balance_sum
           SET dtd_date = vdt_prevbusinessdate,
               balance_dtd = balance
         WHERE business_year = vnm_businessyear;
             
        /* existing customer has ytd balance AND it need not to update within this year.
           update curr bal AND update mtd AND qtd balance if month or quarter change */
        FOR c_acc IN (SELECT d.cust_ac_no,d.balance
                        FROM rmp_account_balance_tmp d, rmp_account_balance_sum s
                       WHERE d.cust_ac_no = s.cust_ac_no
                         AND s.business_year = vnm_businessyear)
        LOOP
            UPDATE rmp_account_balance_sum
               SET business_date = vdt_currbusinessdate,
                   balance = balance, 
                   mtd_date = CASE
                                WHEN vdt_currbusinessdate = vdt_monbusinessdate THEN vdt_monbusinessdate
                                ELSE mtd_date
                              END,
                   balance_mtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_monbusinessdate THEN balance
                                   ELSE balance_mtd
                                 END,
                   qtd_date = CASE
                                WHEN vdt_currbusinessdate = vdt_qtrbusinessdate THEN vdt_qtrbusinessdate
                                ELSE qtd_date
                              END,
                   balance_qtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_qtrbusinessdate THEN balance
                                   ELSE balance_qtd
                                 END,
                   ytd_date = CASE
                                WHEN vdt_currbusinessdate = vdt_yendbusinessdate THEN vdt_yendbusinessdate
                                ELSE ytd_date
                              END,
                   balance_ytd = CASE
                                   WHEN vdt_currbusinessdate = vdt_yendbusinessdate THEN balance
                                   ELSE balance_ytd
                                 END
            WHERE business_year = vnm_businessyear
              AND cust_ac_no = c_acc.cust_ac_no;
        
        END LOOP;
        
         
        
        -- For new business year Insert into new row
           INSERT INTO rmp_customer_balance_sum (
                       business_year, branch_code, cust_no, customer_type, cat_id, 
                       business_date, cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance,
                       dtd_date, cd_balance_dtd, savings_balance_dtd, snd_balance_dtd, rd_balance_dtd, fd_balance_dtd, tot_balance_dtd, 
                       mtd_date, cd_balance_mtd, savings_balance_mtd, snd_balance_mtd, rd_balance_mtd, fd_balance_mtd, tot_balance_mtd, 
                       qtd_date, cd_balance_qtd, savings_balance_qtd, snd_balance_qtd, rd_balance_qtd, fd_balance_qtd, tot_balance_qtd, 
                       ytd_date, cd_balance_ytd, savings_balance_ytd, snd_balance_ytd, rd_balance_ytd, fd_balance_ytd,tot_balance_ytd, ins_date, ins_by)                                                           
                SELECT vnm_businessyear, cd.branch_code, cd.cust_no, c.customer_type, c.cat_id,
                       business_date, cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance, 
                       business_date, cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance, 
                       business_date, cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance, 
                       business_date, cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance, 
                       business_date, cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance, sysdate , pvc_appuser 
             FROM rmp_customer_balance_dtl cd, rmp_customer c
             WHERE cd.cust_no = c.cust_no
               AND cd.business_date = vdt_currbusinessdate
               AND (cd.cust_no,cd.branch_code) IN (SELECT cust_no,branch_code FROM rmp_customer_balance_dtl
                                                    WHERE business_date = vdt_currbusinessdate
                                                    MINUS
                                                   SELECT cust_no,branch_code FROM rmp_customer_balance_sum
                                                    WHERE business_year = vnm_businessyear);
                                                    
            -- transfer todays balance into yesterdays balance ---------------------------------------                         
            UPDATE rmp_customer_balance_sum
               SET dtd_date = vdt_prevbusinessdate,
                   cd_balance_dtd = cd_balance,
                   savings_balance_dtd = savings_balance,
                   snd_balance_dtd = snd_balance,
                   rd_balance_dtd = rd_balance,
                   fd_balance_dtd = fd_balance,
                   tot_balance_dtd = tot_balance
             WHERE business_year = vnm_businessyear;


            FOR cust_balance in (SELECT d.cust_no,d.branch_code,d.customer_type,d.cd_balance,d.savings_balance,d.snd_balance,
                                 d.rd_balance,d.fd_balance,d.tot_balance
                            FROM rmp_customer_balance_tmp d, rmp_customer_balance_sum s
                           WHERE d.cust_no = s.cust_no
                             AND d.branch_code=s.branch_code 
                             AND d.customer_type=s.customer_type
                             AND s.business_year = vnm_businessyear
                             AND d.business_date = vdt_currbusinessdate)
            LOOP
                UPDATE rmp_customer_balance_sum
                   SET business_date = vdt_currbusinessdate,
                       cd_balance = cust_balance.cd_balance, 
                       savings_balance = cust_balance.savings_balance,
                       snd_balance = cust_balance.snd_balance,
                       rd_balance = cust_balance.rd_balance,
                       fd_balance = cust_balance.fd_balance,
                       tot_balance = cust_balance.tot_balance,                   
                        mtd_date = CASE
                                WHEN vdt_currbusinessdate = vdt_monbusinessdate THEN vdt_monbusinessdate
                                ELSE mtd_date
                              END,
                        cd_balance_mtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_monbusinessdate THEN cust_balance.cd_balance
                                   ELSE cd_balance_mtd
                                 END,
                        savings_balance_mtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_monbusinessdate THEN cust_balance.savings_balance
                                   ELSE savings_balance_mtd
                                 END,
                        snd_balance_mtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_monbusinessdate THEN cust_balance.snd_balance
                                   ELSE snd_balance_mtd
                                 END,
                        rd_balance_mtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_monbusinessdate THEN cust_balance.rd_balance
                                   ELSE rd_balance_mtd
                                 END,
                        fd_balance_mtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_monbusinessdate THEN cust_balance.fd_balance
                                   ELSE fd_balance_mtd
                                 END,
                        tot_balance_mtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_monbusinessdate THEN cust_balance.tot_balance
                                   ELSE tot_balance_mtd
                                 END,
                        qtd_date = CASE
                                WHEN vdt_currbusinessdate = vdt_qtrbusinessdate THEN vdt_qtrbusinessdate
                                ELSE qtd_date
                              END,
                        cd_balance_qtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_qtrbusinessdate THEN cust_balance.cd_balance
                                   ELSE cd_balance_qtd
                                 END,
                        savings_balance_qtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_qtrbusinessdate THEN cust_balance.savings_balance
                                   ELSE savings_balance_qtd
                                 END,
                        snd_balance_qtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_qtrbusinessdate THEN cust_balance.snd_balance
                                   ELSE snd_balance_qtd
                                 END,
                        rd_balance_qtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_qtrbusinessdate THEN cust_balance.rd_balance
                                   ELSE rd_balance_qtd
                                 END,
                        fd_balance_qtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_qtrbusinessdate THEN cust_balance.fd_balance
                                   ELSE fd_balance_qtd
                                 END,
                        tot_balance_qtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_qtrbusinessdate THEN cust_balance.tot_balance
                                   ELSE tot_balance_qtd
                                 END,
                        ytd_date = CASE
                                WHEN vdt_currbusinessdate = vdt_yendbusinessdate THEN vdt_yendbusinessdate
                                ELSE ytd_date
                              END,
                        cd_balance_ytd = CASE
                                   WHEN vdt_currbusinessdate = vdt_yendbusinessdate THEN cust_balance.cd_balance
                                   ELSE cd_balance_ytd
                                 END,
                        savings_balance_ytd = CASE
                                   WHEN vdt_currbusinessdate = vdt_yendbusinessdate THEN cust_balance.savings_balance
                                   ELSE savings_balance_ytd
                                 END,
                        snd_balance_ytd = CASE
                                   WHEN vdt_currbusinessdate = vdt_yendbusinessdate THEN cust_balance.snd_balance
                                   ELSE snd_balance_ytd
                                 END,
                        rd_balance_ytd = CASE
                                   WHEN vdt_currbusinessdate = vdt_yendbusinessdate THEN cust_balance.rd_balance
                                   ELSE rd_balance_ytd
                                 END,
                        fd_balance_ytd = CASE
                                   WHEN vdt_currbusinessdate = vdt_yendbusinessdate THEN cust_balance.fd_balance
                                   ELSE fd_balance_ytd
                                 END,
                        tot_balance_ytd = CASE
                                   WHEN vdt_currbusinessdate = vdt_yendbusinessdate THEN cust_balance.tot_balance
                                   ELSE tot_balance_ytd
                                 END
                WHERE business_year = vnm_businessyear
                  AND cust_no = cust_balance.cust_no 
                  AND branch_code = cust_balance.branch_code 
                  AND customer_type = cust_balance.customer_type;
                                
             END loop;
             
              
               
         --RM Balance Update
        INSERT INTO rmp_rm_balance_dtl ( 
                        business_date, branch_code, rm_code, customer_type, cust_count, cd_balance, savings_balance, 
                        snd_balance, rd_balance, fd_balance, tot_balance,ins_date, ins_by) 
                 SELECT max(vdt_currbusinessdate), b.branch_code,m.rm_code,b.customer_type,count(*),sum(cd_balance), sum(savings_balance), 
                        sum(snd_balance), sum(rd_balance), sum(fd_balance), sum(tot_balance), max(sysdate), max(pvc_appuser) 
                   FROM rmp_customer_balance_tmp b ,rmp_customer c, rmp_rm_customer m
                  WHERE b.cust_no = c.cust_no 
                    AND b.customer_type = c.customer_type
                    AND b.branch_code = m.branch_code 
                    AND m.cust_no = c.cust_no 
                    AND eff_date <= vdt_currbusinessdate
                    AND exp_date is null
               GROUP BY b.branch_code,m.rm_code,b.customer_type;
                
        --RM Growth calculation      
       /*UPDATE rmp_rm_balance_dtl rm_growth 
          SET (cd_growth, savings_growth, snd_growth, rd_growth, fd_growth, tot_growth) = (
        SELECT cdgrowth, savinggrowth, sndgrowth, rdgrowth, fdgrowth, totgrowth FROM
               (WITH today_data as(
                       SELECT business_date, rm_code, branch_code, customer_type, cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance
                         FROM rmp_rm_balance_dtl
                       WHERE business_date = vdt_currbusinessdate),
                    lastday_data as(
                        SELECT business_date, rm_code, branch_code, customer_type, cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance
                          FROM rmp_rm_balance_dtl
                         WHERE business_date = vdt_prevbusinessdate)           
                     SELECT today_data.rm_code, today_data.customer_type, today_data.branch_code, today_data.cd_balance - lastday_data.cd_balance cdgrowth,
                            today_data.savings_balance - lastday_data.savings_balance savinggrowth, today_data.snd_balance - lastday_data.snd_balance sndgrowth,
                            today_data.rd_balance - lastday_data.rd_balance rdgrowth, today_data.fd_balance - lastday_data.fd_balance fdgrowth,
                            today_data.tot_balance - lastday_data.tot_balance totgrowth
                       FROM today_data,lastday_data
                      WHERE today_data.rm_code = lastday_data.rm_code
                        AND today_data.branch_code = lastday_data.branch_code
                        AND today_data.customer_type = lastday_data.customer_type
               ) val_r
               WHERE rm_growth.rm_code = val_r.rm_code
                 AND rm_growth.branch_code = val_r.branch_code
                 AND rm_growth.customer_type = val_r.customer_type
                 AND rm_growth.business_date = vdt_currbusinessdate
            );*/
              
            FOR rm_growth IN(SELECT cb.rm_code,cb.branch_code,cb.customer_type,nvl(cb.cd_balance,0)-nvl(pb.cd_balance,0) cd_growth,
                                       nvl(cb.savings_balance,0)-nvl(pb.savings_balance,0) savings_growth,nvl(cb.snd_balance,0)-nvl(pb.snd_balance,0) snd_growth,
                                       nvl(cb.rd_balance,0)-nvl(pb.rd_balance,0) rd_growth,nvl(cb.fd_balance,0)-nvl(pb.fd_balance,0) fd_growth,
                                       nvl(cb.tot_balance,0)-nvl(pb.tot_balance,0) tot_growth
                                  FROM rmp_rm_balance_dtl cb,rmp_rm_balance_dtl pb
                                 WHERE cb.rm_code = pb.rm_code(+)
                                   AND cb.branch_code=pb.branch_code
                                   AND cb.customer_type=pb.customer_type
                                   AND cb.business_date = vdt_currbusinessdate
                                   AND pb.business_date(+) = vdt_prevbusinessdate)
                LOOP
                    UPDATE rmp_rm_balance_dtl
                       SET cd_growth = rm_growth.cd_growth,savings_growth = rm_growth.savings_growth,snd_growth = rm_growth.snd_growth,rd_growth = rm_growth.rd_growth,
                           fd_growth = rm_growth.fd_growth,tot_growth = rm_growth.tot_growth
                     WHERE rm_code = rm_growth.rm_code
                       AND branch_code=rm_growth.branch_code
                       AND customer_type=rm_growth.customer_type
                       AND business_date = vdt_currbusinessdate;
                       
                END LOOP;
       
        -- For new business year Insert into new row
           INSERT INTO rmp_rm_balance_sum (
                       business_year, branch_code, rm_code, customer_type,cust_count,
                       business_date, cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance, 
                       dtd_date, cd_balance_dtd, savings_balance_dtd, snd_balance_dtd, rd_balance_dtd, fd_balance_dtd, tot_balance_dtd,
                       mtd_date, cd_balance_mtd, savings_balance_mtd, snd_balance_mtd, rd_balance_mtd, fd_balance_mtd, tot_balance_mtd, 
                       qtd_date, cd_balance_qtd, savings_balance_qtd, snd_balance_qtd, rd_balance_qtd, fd_balance_qtd, tot_balance_qtd, 
                       ytd_date, cd_balance_ytd, savings_balance_ytd, snd_balance_ytd, rd_balance_ytd, fd_balance_ytd,tot_balance_ytd, ins_date, ins_by)
                SELECT vnm_businessyear, cd.branch_code, cd.rm_code, cd.customer_type, cd.cust_count,
                       business_date,cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance, 
                       business_date, cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance, 
                       business_date, cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance, 
                       business_date, cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance, 
                       business_date, cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance, sysdate , pvc_appuser 
                  FROM rmp_rm_balance_dtl cd
                 WHERE cd.business_date = vdt_currbusinessdate
                  AND (cd.rm_code, cd.branch_code, cd.customer_type) IN (SELECT rm_code,branch_code,customer_type FROM rmp_rm_balance_dtl
                                                                          WHERE business_date = vdt_currbusinessdate
                                                                          MINUS
                                                                         SELECT rm_code,branch_code,customer_type FROM rmp_rm_balance_sum
                                                                          WHERE business_year = vnm_businessyear);
                                                                          
            -- transfer todays balance into yesterdays balance ---------------------------------------                         
            UPDATE rmp_rm_balance_sum
               SET dtd_date = vdt_prevbusinessdate,
                   cd_balance_dtd = cd_balance,
                   savings_balance_dtd = savings_balance,
                   snd_balance_dtd = snd_balance,
                   rd_balance_dtd = rd_balance,
                   fd_balance_dtd = fd_balance,
                   tot_balance_dtd = tot_balance
             WHERE business_year = vnm_businessyear;

            FOR rm_balance IN(SELECT d.rm_code,d.branch_code,d.customer_type,d.cd_balance,d.savings_balance,d.snd_balance,
                                     d.rd_balance,d.fd_balance,d.tot_balance
                                FROM rmp_rm_balance_dtl d, rmp_rm_balance_sum s
                               WHERE d.rm_code = s.rm_code
                                 AND d.branch_code=s.branch_code 
                                 AND d.customer_type=s.customer_type
                                 AND s.business_year = vnm_businessyear
                                 AND d.business_date = vdt_currbusinessdate)
            LOOP
                UPDATE rmp_rm_balance_sum
                   SET business_date = vdt_currbusinessdate,
                       cd_balance = rm_balance.cd_balance, 
                       savings_balance = rm_balance.savings_balance,
                       snd_balance = rm_balance.snd_balance,
                       rd_balance = rm_balance.rd_balance,
                       fd_balance = rm_balance.fd_balance,
                       tot_balance = rm_balance.tot_balance,                   
                        mtd_date = CASE
                                WHEN vdt_currbusinessdate = vdt_monbusinessdate THEN vdt_monbusinessdate
                                ELSE mtd_date
                              END,
                        cd_balance_mtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_monbusinessdate THEN rm_balance.cd_balance
                                   ELSE cd_balance_mtd
                                 END,
                        savings_balance_mtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_monbusinessdate THEN rm_balance.savings_balance
                                   ELSE savings_balance_mtd
                                 END,
                        snd_balance_mtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_monbusinessdate THEN rm_balance.snd_balance
                                   ELSE snd_balance_mtd
                                 END,
                        rd_balance_mtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_monbusinessdate THEN rm_balance.rd_balance
                                   ELSE rd_balance_mtd
                                 END,
                        fd_balance_mtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_monbusinessdate THEN rm_balance.fd_balance
                                   ELSE fd_balance_mtd
                                 END,
                        tot_balance_mtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_monbusinessdate THEN rm_balance.tot_balance
                                   ELSE tot_balance_mtd
                                 END,
                        qtd_date = CASE
                                WHEN vdt_currbusinessdate = vdt_qtrbusinessdate THEN vdt_qtrbusinessdate
                                ELSE qtd_date
                              END,
                        cd_balance_qtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_qtrbusinessdate THEN rm_balance.cd_balance
                                   ELSE cd_balance_qtd
                                 END,
                        savings_balance_qtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_qtrbusinessdate THEN rm_balance.savings_balance
                                   ELSE savings_balance_qtd
                                 END,
                        snd_balance_qtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_qtrbusinessdate THEN rm_balance.snd_balance
                                   ELSE snd_balance_qtd
                                 END,
                        rd_balance_qtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_qtrbusinessdate THEN rm_balance.rd_balance
                                   ELSE rd_balance_qtd
                                 END,
                        fd_balance_qtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_qtrbusinessdate THEN rm_balance.fd_balance
                                   ELSE fd_balance_qtd
                                 END,
                        tot_balance_qtd = CASE
                                   WHEN vdt_currbusinessdate = vdt_qtrbusinessdate THEN rm_balance.tot_balance
                                   ELSE tot_balance_qtd
                                 END,
                        ytd_date = CASE
                                WHEN vdt_currbusinessdate = vdt_yendbusinessdate THEN vdt_yendbusinessdate
                                ELSE ytd_date
                              END,
                        cd_balance_ytd = CASE
                                   WHEN vdt_currbusinessdate = vdt_yendbusinessdate THEN rm_balance.cd_balance
                                   ELSE cd_balance_ytd
                                 END,
                        savings_balance_ytd = CASE
                                   WHEN vdt_currbusinessdate = vdt_yendbusinessdate THEN rm_balance.savings_balance
                                   ELSE savings_balance_ytd
                                 END,
                        snd_balance_ytd = CASE
                                   WHEN vdt_currbusinessdate = vdt_yendbusinessdate THEN rm_balance.snd_balance
                                   ELSE snd_balance_ytd
                                 END,
                        rd_balance_ytd = CASE
                                   WHEN vdt_currbusinessdate = vdt_yendbusinessdate THEN rm_balance.rd_balance
                                   ELSE rd_balance_ytd
                                 END,
                        fd_balance_ytd = CASE
                                   WHEN vdt_currbusinessdate = vdt_yendbusinessdate THEN rm_balance.fd_balance
                                   ELSE fd_balance_ytd
                                 END,
                        tot_balance_ytd = CASE
                                   WHEN vdt_currbusinessdate = vdt_yendbusinessdate THEN rm_balance.tot_balance
                                   ELSE tot_balance_ytd
                                 END
                WHERE business_year = vnm_businessyear
                  AND rm_code = rm_balance.rm_code 
                  AND branch_code = rm_balance.branch_code 
                  AND customer_type = rm_balance.customer_type;
                                
             END loop;
             
              

        UPDATE rmp_work_schedule
           SET lia_process_status = 'Y'
         WHERE business_date = vdt_currbusinessdate;
        
        commit;
        
        DELETE FROM utl_lock_transaction
           WHERE run_id = pnm_run_id;   
            
        commit;
    END IF;
EXCEPTION WHEN OTHERS THEN                                                 
    vvc_status    := sqlcode;
    vvc_statusmsg := substr(sqlerrm,1,500) ;
    ROLLBACK;  
           
    DELETE FROM utl_lock_transaction
       WHERE run_id = pnm_run_id;   
    commit;
    
    dpg_utl_error_log.dpd_utl_single_err(
        'ORA','PKG', vvc_err_loc, '','',to_char(vvc_status),vvc_statusmsg,pnm_run_id,pvc_batch_id,pvc_appuser,null);
                   
END dpd_set_generatebalance;


--------------------------------------------------------------------------------
PROCEDURE dpd_set_regenaratermbalance(
    pnm_run_id        IN number,
    pvc_batch_id      IN varchar2,
    pvc_appuser       IN varchar2
)
AS  
    vnm_businessyear       number;
    vdt_monbusinessdate    date;
    vdt_qtrbusinessdate    date;
    vdt_yendbusinessdate   date;
    
    vvc_status             varchar2(20);
    vvc_statusmsg          varchar2(500);
    vvc_err_loc            varchar2(100) := 'pkg_portfolio_manager.dpd_set_generatebalance';   
    
BEGIN
    
delete from rmp_rm_balance_dtl;
delete from rmp_rm_balance_sum;


begin

 FOR date_loop IN (SELECT work_date,business_date,Prev_business_date FROM rmp_work_schedule WHERE lia_process_status='Y' ORDER BY work_date ASC)
       
    LOOP 
      
       vnm_businessyear:=to_number(to_char(date_loop.work_date,'rrrr'));
       SELECT dfn_get_business_date(date_loop.work_date,'M'),dfn_get_business_date(date_loop.work_date,'Q'),dfn_get_business_date(date_loop.work_date,'Y')
        INTO vdt_monbusinessdate, vdt_qtrbusinessdate, vdt_yendbusinessdate
        FROM dual;
         
        FOR rm_movement IN(SELECT rm_slno,emp_id,old_rm_code,new_rm_code,old_branch_code,new_branch_code,status_code,eff_date 
                            FROM rmp_rm_movement_log 
                            WHERE eff_date<=date_loop.business_date AND auth_date is null)
              LOOP
                  CASE rm_movement.status_code
                    WHEN '01' THEN --Transfer                  
                        -- Update new branch        
                      UPDATE rmp_rm_dtl
                         SET branch_code = rm_movement.new_branch_code, upd_date = date_loop.work_date, upd_by = pvc_appuser
                       WHERE  rm_code = rm_movement.old_rm_code;
                      
                      -- Insert new Record for assigning RM in RMP_RM_CUSTOMER
                      INSERT INTO rmp_rm_customer(branch_code, cust_no, rm_code, eff_date, ins_date, ins_by)
                                           SELECT branch_code, cust_no, rm_movement.new_rm_code, rm_movement.eff_date, date_loop.work_date, pvc_appuser 
                                             FROM rmp_rm_customer WHERE rm_code=rm_movement.old_rm_code;
                      
                      -- Update exp_date to RMP_RM_CUSTOMER            
                      UPDATE rmp_rm_customer 
                         SET exp_date = rm_movement.eff_date, upd_date = date_loop.work_date, upd_by = pvc_appuser 
                       WHERE rm_code=rm_movement.old_rm_code;
                      
                      --Update movement_log table auth_date AND auth_by
                      UPDATE rmp_rm_movement_log 
                         SET auth_by=pvc_appuser, auth_date = date_loop.work_date 
                       WHERE rm_slno = rm_movement.rm_slno;
                    
                  WHEN '02' THEN --Resigned    
                       --Update status to RMP_RM_DTL           
                       UPDATE rmp_rm_dtl
                         SET status = 'N', resign_date = rm_movement.eff_date, upd_date = date_loop.work_date, upd_by = pvc_appuser
                       WHERE  rm_code = rm_movement.old_rm_code;
                      
                      --Insert new Record for assigning RM in RMP_RM_CUSTOMER
                      INSERT INTO rmp_rm_customer(branch_code,cust_no,rm_code,eff_date,ins_date,ins_by)
                                           SELECT branch_code,cust_no, rm_movement.new_rm_code, rm_movement.eff_date, date_loop.work_date,pvc_appuser
                              FROM rmp_rm_customer 
                             WHERE rm_code=rm_movement.old_rm_code;
                             
                      --Update exp_date to RMP_RM_CUSTOMER
                      UPDATE rmp_rm_customer 
                         SET exp_date = rm_movement.eff_date, upd_date = date_loop.work_date, upd_by = pvc_appuser 
                       WHERE rm_code = rm_movement.old_rm_code;
                       
                       --Update movement_log table auth_date AND auth_by
                      UPDATE rmp_rm_movement_log 
                         SET auth_by = pvc_appuser, auth_date = date_loop.work_date 
                       WHERE rm_slno = rm_movement.rm_slno;
                  
                  WHEN '03' THEN --RM code update  
                      --Insert into RMP_RM_DTL with new RMCode AND inactive previous record            
                      INSERT INTO rmp_rm_dtl(emp_id, emp_name, emp_cat, desig_code, grade_code, dept_code, rm_code, branch_code, cat_id, email, 
                                             mobile, status, resign_date, ins_date, ins_by)
                                      SELECT emp_id, emp_name, emp_cat, desig_code, grade_code, dept_code, rm_movement. new_rm_code, branch_code, cat_id, email,  
                                             mobile, status, resign_date, date_loop.work_date, pvc_appuser 
                                        FROM rmp_rm_dtl 
                                       WHERE rm_code = rm_movement.old_rm_code;
                  
                      UPDATE rmp_rm_dtl
                         SET status = 'N',upd_date=date_loop.work_date,upd_by=pvc_appuser
                       WHERE  rm_code = rm_movement.old_rm_code;
                       
                       --Update rmcode to rmp_rm_dtl , rmp_rm_balance_dtl, rmp_rm_balance_sum , rmp_rm_customer, rmp_rm_customer_log
                      UPDATE rmp_rm_customer 
                         SET rm_code = rm_movement.new_rm_code, upd_date = date_loop.work_date, upd_by = pvc_appuser 
                       WHERE rm_code = rm_movement.old_rm_code;
                       
                      UPDATE rmp_rm_balance_dtl 
                         SET rm_code = rm_movement.new_rm_code 
                       WHERE rm_code = rm_movement.old_rm_code;
                       
                      UPDATE rmp_rm_balance_sum  
                         SET rm_code = rm_movement.new_rm_code 
                       WHERE rm_code = rm_movement.old_rm_code;
                      UPDATE rmp_rm_customer 
                         SET rm_code = rm_movement.new_rm_code,upd_date = date_loop.work_date,upd_by = pvc_appuser 
                       WHERE rm_code = rm_movement.old_rm_code;
                       
                      UPDATE rmp_rm_customer_log 
                         SET new_rm_code = rm_movement.new_rm_code, upd_date = date_loop.work_date, upd_by = pvc_appuser 
                       WHERE old_rm_code = rm_movement.old_rm_code;
                      
                      --Update movement_log table auth_date AND auth_by
                      UPDATE rmp_rm_movement_log 
                         SET auth_by = pvc_appuser,auth_date = date_loop.work_date 
                       WHERE rm_slno = rm_movement.rm_slno;
                      
                  WHEN '04' THEN -- (Leave) 
                       --  Update exp_date to rmp_rm_customer
                       INSERT INTO rmp_rm_customer(branch_code, cust_no, rm_code, eff_date, ins_date, ins_by)
                            SELECT branch_code, cust_no, rm_movement.new_rm_code, rm_movement.eff_date, date_loop.work_date, pvc_appuser 
                              FROM rmp_rm_customer WHERE rm_code = rm_movement.old_rm_code;
                         
                       -- Insert new Record for assigning RM in RMP_RM_CUSTOMER
                      UPDATE rmp_rm_customer 
                         SET exp_date = rm_movement.eff_date, upd_date = date_loop.work_date, upd_by = pvc_appuser
                       WHERE rm_code = rm_movement.old_rm_code;
                       
                       --Update movement_log table auth_date AND auth_b
                      UPDATE rmp_rm_movement_log 
                         SET auth_by = pvc_appuser, auth_date = date_loop.work_date 
                       WHERE rm_slno = rm_movement.rm_slno;                  
                END CASE;
            
        END LOOP;   
           
         --RM Balance Update
            INSERT INTO rmp_rm_balance_dtl ( 
                            business_date, branch_code, rm_code, customer_type, cust_count, cd_balance, savings_balance, 
                            snd_balance, rd_balance, fd_balance, tot_balance,ins_date, ins_by) 
                     SELECT max(date_loop.work_date), b.branch_code,m.rm_code,c.customer_type,count(*),sum(cd_balance), sum(savings_balance), 
                            sum(snd_balance), sum(rd_balance), sum(fd_balance), sum(tot_balance), max(date_loop.work_date), max(pvc_appuser) 
                       FROM rmp_customer_balance_dtl b ,rmp_customer c, rmp_rm_customer m
                      WHERE b.cust_no = c.cust_no                         
                        AND b.branch_code = m.branch_code 
                        AND m.cust_no = c.cust_no 
                        AND eff_date <= date_loop.business_date
                        AND exp_date is null
                   GROUP BY b.branch_code,m.rm_code,c.customer_type;
                    
            --RM Growth calculation      
           UPDATE rmp_rm_balance_dtl rm_growth 
              SET (cd_growth, savings_growth, snd_growth, rd_growth, fd_growth, tot_growth) = (
            SELECT cdgrowth, savinggrowth, sndgrowth, rdgrowth, fdgrowth, totgrowth FROM
                   (WITH today_data as(
                           SELECT business_date, rm_code, branch_code, customer_type, cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance
                             FROM rmp_rm_balance_dtl
                           WHERE business_date = date_loop.business_date),
                        lastday_data as(
                            SELECT business_date, rm_code, branch_code, customer_type, cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance
                              FROM rmp_rm_balance_dtl
                             WHERE business_date = date_loop.Prev_business_date)           
                         SELECT today_data.rm_code, today_data.customer_type, today_data.branch_code, today_data.cd_balance - lastday_data.cd_balance cdgrowth,
                                today_data.savings_balance - lastday_data.savings_balance savinggrowth, today_data.snd_balance - lastday_data.snd_balance sndgrowth,
                                today_data.rd_balance - lastday_data.rd_balance rdgrowth, today_data.fd_balance - lastday_data.fd_balance fdgrowth,
                                today_data.tot_balance - lastday_data.tot_balance totgrowth
                           FROM today_data,lastday_data
                          WHERE today_data.rm_code = lastday_data.rm_code
                            AND today_data.branch_code = lastday_data.branch_code
                            AND today_data.customer_type = lastday_data.customer_type
                   ) val_r
                   WHERE rm_growth.rm_code = val_r.rm_code
                     AND rm_growth.branch_code = val_r.branch_code
                     AND rm_growth.customer_type = val_r.customer_type
                     AND rm_growth.business_date = date_loop.business_date
                );

           
            -- For new business year Insert into new row
               INSERT INTO rmp_rm_balance_sum (
                           business_year, branch_code, rm_code, customer_type,cust_count,
                           business_date, cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance, 
                           dtd_date, cd_balance_dtd, savings_balance_dtd, snd_balance_dtd, rd_balance_dtd, fd_balance_dtd, tot_balance_dtd,
                           mtd_date, cd_balance_mtd, savings_balance_mtd, snd_balance_mtd, rd_balance_mtd, fd_balance_mtd, tot_balance_mtd, 
                           qtd_date, cd_balance_qtd, savings_balance_qtd, snd_balance_qtd, rd_balance_qtd, fd_balance_qtd, tot_balance_qtd, 
                           ytd_date, cd_balance_ytd, savings_balance_ytd, snd_balance_ytd, rd_balance_ytd, fd_balance_ytd,tot_balance_ytd, ins_date, ins_by)
                    SELECT vnm_businessyear, cd.branch_code, cd.rm_code, cd.customer_type, cd.cust_count,
                           business_date,cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance, 
                           business_date, cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance, 
                           business_date, cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance, 
                           business_date, cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance, 
                           business_date, cd_balance, savings_balance, snd_balance, rd_balance, fd_balance, tot_balance, date_loop.work_date , pvc_appuser 
                      FROM rmp_rm_balance_dtl cd
                     WHERE cd.business_date = date_loop.business_date
                      AND (cd.rm_code, cd.branch_code, cd.customer_type) IN (SELECT rm_code,branch_code,customer_type FROM rmp_rm_balance_dtl
                                                                              WHERE business_date = date_loop.business_date
                                                                              MINUS
                                                                             SELECT rm_code,branch_code,customer_type FROM rmp_rm_balance_sum
                                                                              WHERE business_year = vnm_businessyear);
                                                                              
                -- transfer todays balance into yesterdays balance ---------------------------------------                         
                UPDATE rmp_rm_balance_sum
                   SET dtd_date = date_loop.Prev_business_date,
                       cd_balance_dtd = cd_balance,
                       savings_balance_dtd = savings_balance,
                       snd_balance_dtd = snd_balance,
                       rd_balance_dtd = rd_balance,
                       fd_balance_dtd = fd_balance,
                       tot_balance_dtd = tot_balance
                 WHERE business_year = vnm_businessyear;

            FOR rm_balance IN(SELECT d.rm_code,d.branch_code,d.customer_type,d.cd_balance,d.savings_balance,d.snd_balance,
                                     d.rd_balance,d.fd_balance,d.tot_balance
                                FROM rmp_rm_balance_dtl d, rmp_rm_balance_sum s
                               WHERE d.rm_code = s.rm_code
                                 AND d.branch_code=s.branch_code 
                                 AND d.customer_type=s.customer_type
                                 AND s.business_year = vnm_businessyear
                                 AND d.business_date = date_loop.business_date)
                LOOP
                    UPDATE rmp_rm_balance_sum
                       SET business_date = date_loop.business_date,
                           cd_balance = rm_balance.cd_balance, 
                           savings_balance = rm_balance.savings_balance,
                           snd_balance = rm_balance.snd_balance,
                           rd_balance = rm_balance.rd_balance,
                           fd_balance = rm_balance.fd_balance,
                           tot_balance = rm_balance.tot_balance,                   
                            mtd_date = CASE
                                    WHEN date_loop.business_date = vdt_monbusinessdate THEN vdt_monbusinessdate
                                    ELSE mtd_date
                                  END,
                            cd_balance_mtd = CASE
                                       WHEN date_loop.business_date = vdt_monbusinessdate THEN rm_balance.cd_balance
                                       ELSE cd_balance_mtd
                                     END,
                            savings_balance_mtd = CASE
                                       WHEN date_loop.business_date = vdt_monbusinessdate THEN rm_balance.savings_balance
                                       ELSE savings_balance_mtd
                                     END,
                            snd_balance_mtd = CASE
                                       WHEN date_loop.business_date = vdt_monbusinessdate THEN rm_balance.snd_balance
                                       ELSE snd_balance_mtd
                                     END,
                            rd_balance_mtd = CASE
                                       WHEN date_loop.business_date = vdt_monbusinessdate THEN rm_balance.rd_balance
                                       ELSE rd_balance_mtd
                                     END,
                            fd_balance_mtd = CASE
                                       WHEN date_loop.business_date = vdt_monbusinessdate THEN rm_balance.fd_balance
                                       ELSE fd_balance_mtd
                                     END,
                            tot_balance_mtd = CASE
                                       WHEN date_loop.business_date = vdt_monbusinessdate THEN rm_balance.tot_balance
                                       ELSE tot_balance_mtd
                                     END,
                            qtd_date = CASE
                                    WHEN date_loop.business_date = vdt_qtrbusinessdate THEN vdt_qtrbusinessdate
                                    ELSE qtd_date
                                  END,
                            cd_balance_qtd = CASE
                                       WHEN date_loop.business_date = vdt_qtrbusinessdate THEN rm_balance.cd_balance
                                       ELSE cd_balance_qtd
                                     END,
                            savings_balance_qtd = CASE
                                       WHEN date_loop.business_date = vdt_qtrbusinessdate THEN rm_balance.savings_balance
                                       ELSE savings_balance_qtd
                                     END,
                            snd_balance_qtd = CASE
                                       WHEN date_loop.business_date = vdt_qtrbusinessdate THEN rm_balance.snd_balance
                                       ELSE snd_balance_qtd
                                     END,
                            rd_balance_qtd = CASE
                                       WHEN date_loop.business_date = vdt_qtrbusinessdate THEN rm_balance.rd_balance
                                       ELSE rd_balance_qtd
                                     END,
                            fd_balance_qtd = CASE
                                       WHEN date_loop.business_date = vdt_qtrbusinessdate THEN rm_balance.fd_balance
                                       ELSE fd_balance_qtd
                                     END,
                            tot_balance_qtd = CASE
                                       WHEN date_loop.business_date = vdt_qtrbusinessdate THEN rm_balance.tot_balance
                                       ELSE tot_balance_qtd
                                     END,
                            ytd_date = CASE
                                    WHEN date_loop.business_date = vdt_yendbusinessdate THEN vdt_yendbusinessdate
                                    ELSE ytd_date
                                  END,
                            cd_balance_ytd = CASE
                                       WHEN date_loop.business_date = vdt_yendbusinessdate THEN rm_balance.cd_balance
                                       ELSE cd_balance_ytd
                                     END,
                            savings_balance_ytd = CASE
                                       WHEN date_loop.business_date = vdt_yendbusinessdate THEN rm_balance.savings_balance
                                       ELSE savings_balance_ytd
                                     END,
                            snd_balance_ytd = CASE
                                       WHEN date_loop.business_date = vdt_yendbusinessdate THEN rm_balance.snd_balance
                                       ELSE snd_balance_ytd
                                     END,
                            rd_balance_ytd = CASE
                                       WHEN date_loop.business_date = vdt_yendbusinessdate THEN rm_balance.rd_balance
                                       ELSE rd_balance_ytd
                                     END,
                            fd_balance_ytd = CASE
                                       WHEN date_loop.business_date = vdt_yendbusinessdate THEN rm_balance.fd_balance
                                       ELSE fd_balance_ytd
                                     END,
                            tot_balance_ytd = CASE
                                       WHEN date_loop.business_date = vdt_yendbusinessdate THEN rm_balance.tot_balance
                                       ELSE tot_balance_ytd
                                     END
                    WHERE business_year = vnm_businessyear
                      AND rm_code = rm_balance.rm_code 
                      AND branch_code = rm_balance.branch_code 
                      AND customer_type = rm_balance.customer_type;
                                    
                 END loop;
       END loop;
    END;
EXCEPTION WHEN OTHERS THEN                                                 
    vvc_status    := sqlcode;
    vvc_statusmsg := substr(sqlerrm,1,500) ;
    ROLLBACK;  
           
    DELETE FROM utl_lock_transaction
       WHERE run_id = pnm_run_id;   
    commit;
    
    dpg_utl_error_log.dpd_utl_single_err(
        'ORA','PKG', vvc_err_loc, '','',to_char(vvc_status),vvc_statusmsg,pnm_run_id,pvc_batch_id,pvc_appuser,null);
                   
END dpd_set_regenaratermbalance;
--------------------------------------------------------------------------------------------------------------------------
END pkg_portfolio_manager_rmlia;
/