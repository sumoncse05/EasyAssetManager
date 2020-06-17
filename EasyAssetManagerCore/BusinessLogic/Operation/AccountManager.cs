using EasyAssetManagerCore.BusinessLogic.Common;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Operation;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace EasyAssetManagerCore.BusinessLogic.Operation
{
    public class AccountManager : BaseService, IAccountManager
    {
        private readonly IAccountRepository accountRepository;
       
        public AccountManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
            accountRepository = new AccountRepository(Connection);
        }
        public IEnumerable<Account> GeAccountDetails(string customerAccountNo, string userId)
        {
            return accountRepository.GeAccountDetails(customerAccountNo, userId);
        }
        public Account GeAccountDetailsWithSession(string customerAccountNo, string userId, AppSession session, IHttpContextAccessor contextAccessor)
        {
            var account = new Account();
            if (customerAccountNo == "")
            {
                MessageHelper.Error(Message, "Account Number is required.");
            }
            //else if (wf_type == "")
            //{
            //    MessageHelper.Error(Message, "Service Type is required.");
            //}
            else
            {
                account = accountRepository.GeAccountDetails(customerAccountNo, userId).FirstOrDefault();
                if (account == null)
                {
                    account = new Account();
                    MessageHelper.Error(Message, "Invalid Account Number.");
                }
                else
                {
                    session.TransactionSession.TransactionAccountDesc = account.ac_desc;
                    session.TransactionSession.TransactionAccountNo = customerAccountNo;
                    //session.TransactionSession.Wf_type = wf_type;
                    //session.TransactionSession.Remarks = remarks;
                    MessageHelper.Success(Message, "Customer is found.");
                    contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
                }
            }
            account.Message = Message;
            return account;
        }

      
        public IEnumerable<District> GetDistrictList(string div_code, string pvc_appuser)
        {
            return accountRepository.GetDistrictList(div_code, pvc_appuser);
        }

        public IEnumerable<Division> GetDivisionList(string pvc_appuser)
        {
            return accountRepository.GetDivisionList(pvc_appuser);
        }

        public IEnumerable<Thana> GetThanaList(string div_code, string dist_code, string pvc_appuser)
        {
            return accountRepository.GetThanaList(div_code, dist_code, pvc_appuser);
        }
    }

    public interface IAccountManager
    {
        IEnumerable<Account> GeAccountDetails(string customerAccountNo, string userId);
        Account GeAccountDetailsWithSession(string customerAccountNo, string userId, AppSession session, IHttpContextAccessor contextAccessor);
        IEnumerable<Division> GetDivisionList(string pvc_appuser);
        IEnumerable<District> GetDistrictList(string div_code, string pvc_appuser);
        IEnumerable<Thana> GetThanaList(string div_code, string dist_code, string pvc_appuser);
       
    }
}
