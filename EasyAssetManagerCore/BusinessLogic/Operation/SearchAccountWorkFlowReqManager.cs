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
    public class SearchAccountWorkFlowReqManager:BaseService, ISearchAccountWorkFlowReqManager
    {
       // private readonly IAccountOpeningRepository accountOpeningRepository;
        private readonly ICommonManager commonManager;
        private readonly ICbsDataConnectionManager cbsDataConnectionManager;
        public SearchAccountWorkFlowReqManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
           // accountOpeningRepository = new AccountOpeningRepository(Connection);
            commonManager = new CommonManager();
            cbsDataConnectionManager = new CbsDataConnectionManager();
        }
        public IEnumerable<WorkflowDetail> GetAccountWorkFowRequest(string pvc_transid, string pvc_custno, AppSession session)
        {
            // return accountOpeningRepository.GetWorkflowRequest(pvc_transid, pvc_custno, session.User.user_id);
            return null;
        }
    }
    public interface ISearchAccountWorkFlowReqManager
    {
        IEnumerable<WorkflowDetail> GetAccountWorkFowRequest(string pvc_transid, string pvc_custno, AppSession session);
    }
}
