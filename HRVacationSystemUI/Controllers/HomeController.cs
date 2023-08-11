using HRVacationSystemBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRVacationSystemUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPersonelRoleManager _personelRoleManager;
        private readonly IPersonelManager _personelManager;

        public HomeController(IPersonelRoleManager personelRoleManager, IPersonelManager personelManager)
        {
            _personelRoleManager = personelRoleManager;
            _personelManager = personelManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        //public PartialViewResult TopNavMenu()
        //{
        //    var loggedInUserEmail = HttpContext.User.Identity.Name;
        //    TempData["IsCEO"]= _personelRoleManager.IsInRole(loggedInUserEmail, "CEO");
        //    TempData["IsBusinessAnalyst"] = _personelRoleManager.IsInRole(loggedInUserEmail, "Business Analyst");
        //    TempData["IsTeamLeader"] = _personelRoleManager.IsInRole(loggedInUserEmail, "Team Leader");
        //    TempData["IsDeveloper"] = _personelRoleManager.IsInRole(loggedInUserEmail, "Software Developer");

        //    return PartialView();
        //}

        public PartialViewResult TopNavMenu()
        {
            var loggedInUserEmail = HttpContext.User.Identity.Name;
            var personel = _personelManager.GetPersonelProile(loggedInUserEmail);
           
            if (_personelRoleManager.IsInRole(loggedInUserEmail, "CEO"))
            {
                return PartialView("CeoTopNavMenu", personel);

            }
            TempData["IsBusinessAnalyst"] = _personelRoleManager.IsInRole(loggedInUserEmail, "Business Analyst");
            TempData["IsTeamLeader"] = _personelRoleManager.IsInRole(loggedInUserEmail, "Team Leader");
            TempData["IsDeveloper"] = _personelRoleManager.IsInRole(loggedInUserEmail, "Software Developer");

            return PartialView(personel);
        }


    }
}