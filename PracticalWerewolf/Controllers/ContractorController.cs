using PracticalWerewolf.Models.UserInfos;
using PracticalWerewolf.ViewModels.Contractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PracticalWerewolf.Controllers
{
    [Authorize]
    public class ContractorController : Controller
    {
        public enum ContractorMessageId
        {
            RegisterSuccess,
            Error
        }

        public ActionResult Index(ContractorMessageId? message)
        {
            ViewBag.StatusMessage = 
                message == ContractorMessageId.RegisterSuccess ? "Registered as a contractor succesfully"
                : message == ContractorMessageId.Error ? "Error occured"
                : "";


            var model = new ContractorIndexModel
            {
                ContractorInfo = null,
            };

            return View(model);
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(ContractorRegisterModel model)
        {

            return View();
        }
    }
}