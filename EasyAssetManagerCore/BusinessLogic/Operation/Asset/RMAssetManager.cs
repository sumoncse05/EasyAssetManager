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
        public RMAssetManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
            rmRepository = new RMAssetRepository(Connection);
        }

        public IEnumerable<Loan> GetUnAuthorizeRM(string loanType, string branchCode, string rmCode,AppSession session)
        {
            return rmRepository.GetUnAuthorizeLoanRM(loanType, branchCode, rmCode, session.User.user_id);
        }

        public IEnumerable<Branch> GetBranchList(string user_id)
        {
            return rmRepository.GetBranchList(user_id);
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
    }

    public interface IRMAssetManager
    {
        IEnumerable<Branch> GetBranchList(string user_id);
        IEnumerable<RM> GetRmList(string branch_code, string user_id);
        IEnumerable<LoanProduct> GetLoanProductList(string loanType, string user_id);
        Message SetAssignRM(List<string> loans, string newRmCode, string effectDate, AppSession session);
        IEnumerable<Loan> GetUnAuthorizeRM(string loanType, string branchCode, string rmCode, AppSession session);
        Message SetAuthorizeRM(List<string> loans, AppSession session);
        IEnumerable<Loan> GetAvailableLoan(string loanType, string loanProduct, string rmCode, AppSession session);
    }
}
