using EasyAssetManagerCore.BusinessLogic.Common;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Operation.Asset;
using System;
using System.Collections.Generic;
using System.Data;

namespace EasyAssetManagerCore.BusinessLogic.Operation.Asset
{
    public class RMAssetManager : BaseService, IRMAssetManager
    {
        private readonly IRMAssetRepository rmRepository;
        public RMAssetManager()
        {
            rmRepository = new RMAssetRepository(Connection);
        }

        public IEnumerable<Loan> GetUnAuthorizeRM(string loanType, string branchCode, string rmCode,AppSession session)
        {
            return rmRepository.GetUnAuthorizeLoanRM(loanType, branchCode, rmCode, session.User.user_id);
        }

        public IEnumerable<Branch> GetBranchList(string pvc_areacode, string user_id)
        {
            return rmRepository.GetBranchList(pvc_areacode,user_id);
        }

        public IEnumerable<Area> GetAreaList(string user_id)
        {
            return rmRepository.GetAreaList(user_id);
        }
        public IEnumerable<LoanProduct> GetLoanProductList(string loanType, string user_id)
        {
            return rmRepository.GetLoanProduct(loanType, user_id);
        }

        public IEnumerable<RM> GetRmList(string branch_code,string user_id)
        {
            return rmRepository.GetRMList(branch_code, user_id);
        }

        public Message SetAssignRM(List<string> loans, string newRmCode, string effectDate, AppSession session)
        {
            try
            {
                int SuccCnt = 0, FailCnt = 0;
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                foreach (var lonSl in loans)
                {
                    var msg = rmRepository.SetRMAssignWithLoan(lonSl, newRmCode, effectDate, session.User.user_id);

                    if (msg.pvc_status == "40999")
                    {
                        SuccCnt++;
                    }
                    else
                    {
                        FailCnt++;
                    }
                }

                MessageHelper.Success(Message, "Successful:" + SuccCnt.ToString() + ", Failed:" + FailCnt.ToString());
            }
            catch (Exception ex)
            {

                MessageHelper.Error(Message, ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }

        public Message SetAuthorizeRM(List<string> loans, AppSession session)
        {

            try
            {
                int SuccCnt = 0, FailCnt = 0;
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                foreach(var lonSl in loans)
                {
                    var msg = rmRepository.SetRMAuthorized(lonSl, session.User.user_id);

                    if (msg.pvc_status == "40999")
                    {
                        SuccCnt++;
                    }
                    else
                    {
                        FailCnt++;
                    }
                }
               
                MessageHelper.Success(Message, "Successful:" + SuccCnt.ToString() + ", Failed:" + FailCnt.ToString());
            }
            catch (Exception ex)
            {

                MessageHelper.Error(Message, ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }


        public IEnumerable<Loan> GetAvailableLoan(string loanType, string loanProduct, string rmCode, AppSession session)
        {
            return rmRepository.GetAvailableLoan(loanType, loanProduct, rmCode,session.User.user_id);
        }

        public IEnumerable<Designation> GetDesignationList(string user_id)
        {
            return rmRepository.GetDesignationList(user_id);
        }
        public IEnumerable<Grade> GetGradeList(string user_id)
        {
            return rmRepository.GetGradeList(user_id);
        }
        public IEnumerable<Department> GetDepartmentList(string user_id)
        {
            return rmRepository.GetDepartmentList(user_id);
        }
        public IEnumerable<Category> GetCategoryList(string user_id)
        {
            return rmRepository.GetCategoryList(user_id);
        }
        public IEnumerable<RMStatus> GetRMStatusList(string user_id)
        {
            return rmRepository.GetRMStatusList(user_id);
        }
        
        public Message SetRM(RM rm, AppSession session)
        {

            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();                
                    var msg = rmRepository.SetRM(rm, session.User.user_id);
                if (msg.pvc_status == "40999")
                {
                    MessageHelper.Success(Message, "RM Saved sucessfully:");
                }
                else
                {
                    MessageHelper.Error(Message, "RM Saved Failed:");
                }                
            }
            catch (Exception ex)
            {
                MessageHelper.Error(Message, ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }
        public Message UpdateRMStatus(RM rm, AppSession session)
        {

            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var msg = rmRepository.UpdateRMStatus(rm, session.User.user_id);
                if (msg.pvc_status == "40999")
                {
                    MessageHelper.Success(Message, "RM Update sucessfully:");
                }
                else
                {
                    MessageHelper.Error(Message, "RM Update Failed:");
                }
            }
            catch (Exception ex)
            {
                MessageHelper.Error(Message, ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }
        public RM GetRMDetails(string emp_id, string user_id)
        {
            var rmDetails = rmRepository.GetRMDetails(emp_id,user_id);
            if (rmDetails == null)
            {
                rmDetails = new RM();
                MessageHelper.Error(Message,"No Employee found");
            } 
            else
                MessageHelper.Success(Message, "Employee found");
            rmDetails.Message = Message;
            return rmDetails;
        }
        public IEnumerable<RM> GetRMDetailList(string emp_id,string rm_name, string user_id)
        {
            return rmRepository.GetRMDetailList(emp_id, rm_name, user_id);
        }
        public IEnumerable<Loan> GetLoanDetailList(string ac_area_code, string ac_branch_code, string ac_loan_no, string user_id)
        {
            return rmRepository.GetLoanDetailList(ac_area_code, ac_branch_code, ac_loan_no, user_id);
        }
        public Message UpdateMonitoringRM(string loan_ac_no, string moni_rm_code, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var msg = rmRepository.UpdateMonitoring(loan_ac_no, moni_rm_code, session.User.user_id);
                if (msg.pvc_status == "40999")
                {
                    MessageHelper.Success(Message, "Monitoring RM Update sucessfully:");
                }
                else
                {
                    MessageHelper.Error(Message, "Monitoring RM Update Failed:");
                }
            }
            catch (Exception ex)
            {
                MessageHelper.Error(Message, ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }
    }

    public interface IRMAssetManager
    {
        Message UpdateMonitoringRM(string loan_ac_no, string moni_rm_code, AppSession session);
        IEnumerable<Loan> GetLoanDetailList(string ac_area_code, string ac_branch_code, string ac_loan_no, string user_id);
        IEnumerable<Branch> GetBranchList(string areacode, string user_id);
        IEnumerable<Area> GetAreaList(string user_id);
        IEnumerable<RM> GetRmList(string branch_code, string user_id);
        IEnumerable<LoanProduct> GetLoanProductList(string loanType, string user_id);
        Message SetAssignRM(List<string> loans, string newRmCode, string effectDate, AppSession session);
        IEnumerable<Loan> GetUnAuthorizeRM(string loanType, string branchCode, string rmCode, AppSession session);
        Message SetAuthorizeRM(List<string> loans, AppSession session);
        IEnumerable<Loan> GetAvailableLoan(string loanType, string loanProduct, string rmCode, AppSession session);
        IEnumerable<Designation> GetDesignationList(string user_id); 
        IEnumerable<Grade> GetGradeList(string user_id);
        IEnumerable<Department> GetDepartmentList(string user_id);
        IEnumerable<Category> GetCategoryList(string user_id);
        IEnumerable<RMStatus> GetRMStatusList(string user_id);
        Message SetRM(RM rm, AppSession session); 
        RM GetRMDetails(string emp_id, string user_id);
        IEnumerable<RM> GetRMDetailList(string emp_id, string rm_name, string user_id); 
        Message UpdateRMStatus(RM rm, AppSession session);
    }
}
