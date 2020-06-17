using EasyAssetManagerCore.BusinessLogic.Common;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Operation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EasyAssetManagerCore.BusinessLogic.Operation
{
    public class SearchAgentTransactionManager : BaseService, ISearchAgentTransactionManager
    {
        private readonly IAccountOpeningRepository accountOpeningRepository;
        private readonly ICommonManager commonManager;
        private readonly ICbsDataConnectionManager cbsDataConnectionManager;
        public SearchAgentTransactionManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
            accountOpeningRepository = new AccountOpeningRepository(Connection);
            commonManager = new CommonManager();
            cbsDataConnectionManager = new CbsDataConnectionManager();
        }
        public IEnumerable<TransactionDetails> GetAccountOpenRequest(string pvc_transid, string pvc_custno, string pvc_custacno, AppSession session)
        {
            return accountOpeningRepository.GetTransactionDetails(pvc_transid, pvc_custno, pvc_custacno, session.User.user_id);
        }
        public TransactionDetails GetTransactionDetails(string trans_id, AppSession session, IHttpContextAccessor contextAccessor)
        {
            var transDetails = new TransactionDetails();
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                transDetails = accountOpeningRepository.GetTransactionDetails(trans_id, "", "", session.User.user_id).FirstOrDefault();

                if (transDetails != null)
                {                    
                    MessageHelper.Success(Message, "Transaction found.");
                }
                else
                {
                    MessageHelper.Error(Message, "Transaction not found. Please try again.");
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "SearchAgentTransactionManager-SetTransactionDetails", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Error(Message, "System Error.");
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
            }
            transDetails.Message = Message;
            return transDetails;
        }
    }
    public interface ISearchAgentTransactionManager
    {
        IEnumerable<TransactionDetails> GetAccountOpenRequest(string pvc_transid, string pvc_custno, string pvc_custacno, AppSession session);
        TransactionDetails GetTransactionDetails(string trans_id, AppSession session, IHttpContextAccessor contextAccessor);
    }
}
