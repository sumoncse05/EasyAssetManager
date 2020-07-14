using EasyAssetManagerCore.BusinessLogic.Common;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Operation.Asset;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
            try
            {
                return rmRepository.GetUnAuthorizeLoanRM(loanType, branchCode, rmCode, session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RMAssetManager-GetUnAuthorizeRM", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
        }

        public IEnumerable<Branch> GetBranchList(string pvc_areacode, AppSession session)
        {
            try
            {
                return rmRepository.GetBranchList(pvc_areacode, session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RMAssetManager-GetBranchList", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
        }

        public IEnumerable<Area> GetAreaList(AppSession session)
        {
            try
            {
                return rmRepository.GetAreaList(session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RMAssetManager-GetAreaList", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
        }
        public IEnumerable<LoanProduct> GetLoanProductList(string loanType, AppSession session)
        {
            try
            {
                return rmRepository.GetLoanProduct(loanType, session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RMAssetManager-GetLoanProductList", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
        }

        public IEnumerable<RM> GetRmList(string branch_code,AppSession session)
        {
            try
            {
                return rmRepository.GetRMList(branch_code, session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RMAssetManager-GetRmList", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
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
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RMAssetManager-SetAssignRM", ex.Message + "|" + ex.StackTrace.TrimStart());

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
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RMAssetManager-SetAuthorizeRM", ex.Message + "|" + ex.StackTrace.TrimStart());
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
            try
            {
                return rmRepository.GetAvailableLoan(loanType, loanProduct, rmCode, session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RMAssetManager-GetAvailableLoan", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
        }

        public IEnumerable<Designation> GetDesignationList(AppSession session)
        {
            try
            {
                return rmRepository.GetDesignationList(session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RMAssetManager-GetDesignationList", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
        }
        public IEnumerable<Grade> GetGradeList(AppSession session)
        {
            try
            {
                return rmRepository.GetGradeList(session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RMAssetManager-GetGradeList", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
        }
        public IEnumerable<Department> GetDepartmentList(AppSession session)
        {
            try
            {
                return rmRepository.GetDepartmentList(session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RMAssetManager-GetDepartmentList", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
        }
        public IEnumerable<Category> GetCategoryList(AppSession session)
        {
            try
            {
                return rmRepository.GetCategoryList(session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RMAssetManager-GetCategoryList", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
        }
        public IEnumerable<RMStatus> GetRMStatusList(AppSession session)
        {
            try
            {
                return rmRepository.GetRMStatusList(session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RMAssetManager-GetRMStatusList", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
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
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RMAssetManager-SetRM", ex.Message + "|" + ex.StackTrace.TrimStart());
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
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RMAssetManager-UpdateRMStatus", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Error(Message, ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }
        public RM GetRMDetails(string emp_id, AppSession session)
        {
            try
            {
                var rmDetails = rmRepository.GetRMDetails(emp_id, session.User.user_id);
                if (rmDetails == null)
                {
                    rmDetails = new RM();
                    MessageHelper.Error(Message, "No Employee found");
                }
                else
                    MessageHelper.Success(Message, "Employee found");
                rmDetails.Message = Message;
                return rmDetails;
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RMAssetManager-GetRMDetails", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
        }
        public IEnumerable<RM> GetRMDetailList(string emp_id,string rm_name, AppSession session)
        {
            try
            {
                return rmRepository.GetRMDetailList(emp_id, rm_name, session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RMAssetManager-GetRMDetailList", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
        }
        public IEnumerable<Loan> GetLoanDetailList(string ac_area_code, string ac_branch_code, string ac_loan_no, AppSession session)
        {
            try
            {
                return rmRepository.GetLoanDetailList(ac_area_code, ac_branch_code, ac_loan_no, session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RMAssetManager-GetLoanDetailList", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
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
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RMAssetManager-UpdateMonitoringRM", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Error(Message, ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }
        public DashBord GetDashBoardDetails(AppSession session)
        {
            try
            {
                return rmRepository.GetDashBoardDetails(session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RMAssetManager-GetDashBoardDetails", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
        }
        public IEnumerable<DashBord> GetHalfYearlyDashBoard(AppSession session)
        {
            try
            {
                return rmRepository.GetHalfYearlyDashBoard(session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RMAssetManager-GetHalfYearlyDashBoard", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
        }
        public IEnumerable<AstDailyStatus> GetYearlyReportData(string area_code, string branch_code, string rm_code, string loantype, AppSession session)
        {
            try
            {
                return rmRepository.GetYearlyReportData(area_code, branch_code, rm_code, loantype, session.User.user_id);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RMAssetManager-GetYearlyReportData", ex.Message + "|" + ex.StackTrace.TrimStart());
                return null;
            }
        }
    }

    public interface IRMAssetManager
    {
        IEnumerable<Branch> GetBranchList(string areacode, AppSession session);
        IEnumerable<Area> GetAreaList(AppSession session);
        IEnumerable<RM> GetRmList(string branch_code, AppSession session);
        IEnumerable<LoanProduct> GetLoanProductList(string loanType, AppSession session);
        Message SetAssignRM(List<string> loans, string newRmCode, string effectDate, AppSession session);
        IEnumerable<Loan> GetUnAuthorizeRM(string loanType, string branchCode, string rmCode, AppSession session);
        Message SetAuthorizeRM(List<string> loans, AppSession session);
        IEnumerable<Loan> GetAvailableLoan(string loanType, string loanProduct, string rmCode, AppSession session);
        IEnumerable<Designation> GetDesignationList(AppSession session); 
        IEnumerable<Grade> GetGradeList(AppSession session);
        IEnumerable<Department> GetDepartmentList(AppSession session);
        IEnumerable<Category> GetCategoryList(AppSession session);
        IEnumerable<RMStatus> GetRMStatusList(AppSession session);
        Message SetRM(RM rm, AppSession session); 
        RM GetRMDetails(string emp_id, AppSession session);
        IEnumerable<RM> GetRMDetailList(string emp_id, string rm_name, AppSession session); 
        Message UpdateRMStatus(RM rm, AppSession session);
        IEnumerable<DashBord> GetHalfYearlyDashBoard(AppSession session);
        DashBord GetDashBoardDetails(AppSession session);
        Message UpdateMonitoringRM(string loan_ac_no, string moni_rm_code, AppSession session);
        IEnumerable<Loan> GetLoanDetailList(string ac_area_code, string ac_branch_code, string ac_loan_no, AppSession session);
        IEnumerable<AstDailyStatus> GetYearlyReportData(string area_code, string branch_code, string rm_code, string loantype, AppSession session);

    }
}
