using EasyAssetManagerCore.BusinessLogic.Operation;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EasyAssetManager.Controllers
{
    public class AgentController : BaseController
    {
        private readonly IAgentManager agentService;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        public AgentController(IAgentManager agentService, IHostingEnvironment environment, ICommonManager commonManager)
        {
            this.agentService = agentService;
            this.environment = environment;
            this.commonManager = commonManager;
        }
        [ScreenPermission("50111")]
        public IActionResult Index(string agentId = null)
        {
            var agent = new Agent();
            if (agentId != null)
                agent = agentService.GetAgentDetails(agentId, Session.User.user_id).FirstOrDefault();
            ViewBag.Agent = agent;
            return View();
        }

        [HttpPost]
        public IActionResult SaveAgent(Agent agent)
        {
            var message = agentService.SetAgentDetail(agent, Session);
            return Json(message);
        }
         [ScreenPermission("50113")]
        public IActionResult AuthorizeAgent()
        {
            return View();
        }

        public IActionResult AuthorizeAgentList()
        {
            var agents = agentService.GetUnAuthendicatedAgentDetails("", Session.User.user_id);
            return PartialView("_AuthorizeAgentList",agents);
        }

        public IActionResult AuthorizeAgentPreview(string agentId)
        {
            var agent = agentService.GetUnAuthendicatedAgentDetails(agentId, Session.User.user_id).FirstOrDefault() ;
            return View(agent);
        }
        [HttpPost]
        public IActionResult AuthorizeAgent(string agentId)
        {
            var message = agentService.SetAgentAuthorized(agentId, Session);
            return Json(message);
        }

        // [ScreenPermission("51011")]
        public IActionResult SearchAgent()
        {
            return View();
        }

        public IActionResult SearchAgentList(string agent_id,string agent_name)
        {
            var agents = agentService.GetAgentDetails(agent_id, agent_name, Session.User.user_id);
            return PartialView("_AgentList", agents);
        }
        // [ScreenPermission("50115")]
        public IActionResult AgentAccountSetup(string agentId = null)
        {
            var agent = new Agent();
            if (agentId != null)
                agent = agentService.GetAgentDetails(agentId, Session.User.user_id).FirstOrDefault();
            return View(agent);
        }

        public IActionResult SearchAgentAccount(string agentId, string cust_ac_no)
        {
            var agentAccounts = agentService.GetAgentAccounts(agentId,cust_ac_no, Session.User.user_id);
            return PartialView("_AgentAccount", agentAccounts);
        }

        [HttpPost]
        public IActionResult UpdateAccount(string agentId, string cust_ac_no, string status)
        {
            var message = agentService.SetAgentAccountStatus(agentId, cust_ac_no,status, Session);
            return Json(message);
        }
        [ScreenPermission("50117")]
        public IActionResult AgentOutletSetup(string agent_id=null, string outlet_id=null)
        {
            var agentOutlet = new AgentOutlet();
            if (agent_id != null && outlet_id != null)
                agentOutlet = agentService.GetAgentOutletDetails(agent_id, outlet_id, Session.User.user_id).FirstOrDefault();
            ViewBag.AgentOutlet = agentOutlet;
            return View();
        }
        [HttpGet]
        public IActionResult GetAgentOutletDetails(string agent_id, string outlet_id)
        {
            var outlets = agentService.GetAgentOutletDetails(agent_id, outlet_id, Session.User.user_id);
            return Json(outlets);
        }
        [HttpPost]
        public IActionResult SaveAgentOutletDetails(AgentOutlet agentOutlet)
        {
            var message = agentService.SetAgentOutletDetails(agentOutlet, Session);
            return Json(message);
        }
         [ScreenPermission("50119")]
        public IActionResult AgentOutletAuthorize()
        {
            return View();
        }

        public IActionResult AgentOutletAuthorizeList()
        {
            var agents = agentService.GetUnauthenticAgentOutlet("","", Session.User.user_id);
            return PartialView("_AgentOutletAuthorizeList", agents);
        }

        public IActionResult AgentOutletAuthorizePreview(string agent_id,string outlet_id)
        {
            var agentOutlet = agentService.GetUnauthenticAgentOutlet(agent_id, outlet_id, Session.User.user_id).FirstOrDefault();
            return View(agentOutlet);
        }
        [HttpPost]
        public IActionResult AuthorizeAgentOutlet(string agent_id, string outlet_id)
        {
            var message = agentService.SetAgentOutletAuthorized(agent_id, outlet_id, Session);
            return Json(message);
        }
        public IActionResult SearchAgentOutlet()
        {
            return View();
        }

        public IActionResult SearchAgentOutletList(string agent_id, string agent_name, string outlet_name)
        {
            var agentOutlet = agentService.GetAgentOutletDetails(agent_id, agent_name, outlet_name, Session.User.user_id);
            return PartialView("_AgentOutletList", agentOutlet);
        }
    }
}