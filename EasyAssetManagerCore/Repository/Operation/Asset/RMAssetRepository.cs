using Dapper;
using Dapper.Oracle;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Common;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace EasyAssetManagerCore.Repository.Operation.Asset
{
    public class RMAssetRepository : BaseRepository, IRMAssetRepository
    {
        public RMAssetRepository(OracleConnection connection) : base(connection)
        {
            
        }
        public IEnumerable<RM> GetRMList(string pvc_branchcode, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_branchcode", pvc_branchcode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_userrmlist", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<RM>("pkg_liability_lov_manager.dpd_get_userrmlist", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<LoanProduct> GetLoanProduct(string pvc_sgid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_sgid", pvc_sgid, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_loandetails", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<LoanProduct>("pkg_asset_manager.dpd_get_loandetails", dyParam, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<Loan> GetAvailableLoan(string pvc_seg_id, string pvc_product_code,string pvc_origrm_code, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_seg_id", pvc_seg_id, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_product_code", pvc_product_code, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_oldrm_code", pvc_origrm_code, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_availableloan", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Loan>("pkg_asset_manager.dpd_get_availableloan", dyParam, commandType: CommandType.StoredProcedure);
        }

        public ResponseMessage SetRMAssignWithLoan(string pvc_loanslno, string pvc_rm_code, string pvc_effdate, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_loanslno", pvc_loanslno, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_rm_code", pvc_rm_code, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_effdate", pvc_effdate, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_asset_manager.dpd_set_assaignloantorm", dyParam);
            return res;

        }

        public IEnumerable<Branch> GetBranchList(string pvc_areacode, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_areacode", pvc_areacode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_allbranchlist", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Branch>("pkg_liability_lov_manager.dpd_get_areawisebranchlist", dyParam, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<Area> GetAreaList(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_areadtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Area>("pkg_liability_lov_manager.dpd_get_areadtl", dyParam, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<Loan> GetUnAuthorizeLoanRM(string pvc_seg_id, string pvc_branchCode, string pvc_rmcode, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_seg_id", pvc_seg_id, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_branch_code", pvc_branchCode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_oldrm_code", pvc_rmcode, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_authorizeloan", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Loan>("pkg_asset_manager.dpd_get_authorizeloan", dyParam, commandType: CommandType.StoredProcedure);
        }

        public ResponseMessage SetRMAuthorized(string pvc_custlogslno, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_loanslno", pvc_custlogslno, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_asset_manager.dpd_set_authorizeloantorm", dyParam);
            return res;
        }
        public IEnumerable<Department> GetDepartmentList(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            //dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_departmentdtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Department>("dpg_setup_manager.dpd_get_departmentdtl", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<Designation> GetDesignationList(string pvc_appuser,string pvc_designationId="")
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_designationId", pvc_designationId, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_designationdtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Designation>("dpg_setup_manager.dpd_get_designationdtl", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<Grade> GetGradeList(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            //dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_gradedtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Grade>("dpg_setup_manager.dpd_get_gradedtl", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<Category> GetCategoryList(string pvc_appuser,string pvc_catId="")
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_catId", pvc_catId, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_categorydtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Category>("dpg_setup_manager.dpd_get_categorydtl", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<RMStatus> GetRMStatusList(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_rmstatusdtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<RMStatus>("dpg_setup_manager.dpd_get_rmstatusdtl", dyParam, commandType: CommandType.StoredProcedure);
        }
        
        public ResponseMessage SetRM(RM rm, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_employeeid", rm.emp_id, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_rmcode", rm.rm_code, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_employeename", rm.emp_name, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_designationid", rm.desig_code, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_branchcode", rm.branch_code, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_categoryId", rm.cat_id, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_email", rm.email, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_mobile", rm.mobile, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_dep_code", rm.dept_code, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_gradecode", rm.grade_code, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "dpg_setup_manager.dpd_set_employeedtl", dyParam);
            return res;
        }
        public ResponseMessage UpdateRMStatus(RM rm, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_employeeid", rm.emp_id, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_rm_status", rm.status_code, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_old_rm_code", rm.rm_code, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_new_rm_code", rm.new_rm_code, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_old_branch_code", rm.branch_code, OracleMappingType.Varchar2, ParameterDirection.Input, 10); 
            dyParam.Add("pvc_new_branch_code", rm.new_branch_code, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            //dyParam.Add("pvc_eff_date", System.Convert.ToDateTime(rm.effect_date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_eff_date", rm.effect_date, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "dpg_setup_manager.dpd_set_rm_statusdtl", dyParam);
            return res;
        }
        public RM GetRMDetails(string pvc_employeeid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_employeeid", pvc_employeeid, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_employeedtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<RM>("dpg_setup_manager.dpd_get_employeedtl", dyParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
        }
        public IEnumerable<RM> GetRMDetailList(string pvc_rmcode, string pvc_rmname, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_rmcode", pvc_rmcode, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_rmname", pvc_rmname, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_rmdtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<RM>("dpg_search_manager.dpd_get_rmdtl", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<Loan> GetLoanDetailList(string pvc_area_code, string pvc_branch_code, string pvc_loan_acnumber, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_loan_acnumber", pvc_loan_acnumber, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_area_code", pvc_area_code, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_branch_code", pvc_branch_code, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_loandetails", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Loan>("pkg_asset_manager.dpd_get_loandetails", dyParam, commandType: CommandType.StoredProcedure);
        }
        public ResponseMessage UpdateMonitoring(string pvc_loan_acnumber, string pvc_monirm_code, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_loan_acnumber", pvc_loan_acnumber, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_monirm_code", pvc_monirm_code, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_asset_manager.dpd_update_monirm", dyParam);
            return res;
        }
    }

    public interface IRMAssetRepository
    {
        ResponseMessage UpdateMonitoring(string pvc_loan_acnumber, string pvc_monirm_code, string pvc_appuser);
        IEnumerable<Loan> GetLoanDetailList(string pvc_area_code, string pvc_branch_code, string pvc_loan_acnumber, string pvc_appuser);
        IEnumerable<RM> GetRMList(string pvc_branchcode, string pvc_appuser);
        IEnumerable<Loan> GetAvailableLoan(string pvc_seg_id, string pvc_product_code, string pvc_origrm_code, string pvc_appuser);
        ResponseMessage SetRMAssignWithLoan(string pvc_loanlogslno, string pvc_rm_code, string pvc_effdate, string pvc_appuser);
        IEnumerable<Branch> GetBranchList(string pvc_areacode, string pvc_appuser);
        IEnumerable<Area> GetAreaList(string pvc_appuser);
        IEnumerable<Loan> GetUnAuthorizeLoanRM(string pvc_seg_id, string pvc_branchCode, string pvc_rmcode, string pvc_appuser);
        IEnumerable<LoanProduct> GetLoanProduct(string pvc_sgid, string pvc_appuser);
        ResponseMessage SetRMAuthorized(string pvc_loanlogslno, string pvc_appuser);
        IEnumerable<Designation> GetDesignationList(string pvc_appuser, string pvc_designationId = "");
        IEnumerable<Grade> GetGradeList(string pvc_appuser);
        IEnumerable<Department> GetDepartmentList(string pvc_appuser);
        IEnumerable<Category> GetCategoryList(string pvc_appuser, string pvc_catId = ""); 
        IEnumerable<RMStatus> GetRMStatusList(string pvc_appuser);
        ResponseMessage SetRM(RM rm, string pvc_appuser); 
        ResponseMessage UpdateRMStatus(RM rm, string pvc_appuser);
        RM GetRMDetails(string pvc_employeeid, string pvc_appuser);
        IEnumerable<RM> GetRMDetailList(string pvc_employeeid, string pvc_rmname, string pvc_appuser);
    }
}
