using HRVacationSystemBL;
using HRVacationSystemDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRVacationSystemUI.Controllers
{
    [Authorize]
    public class VacationController : Controller
    {

       private readonly IVacationManager _vacationManager ;
       private readonly IPersonelManager _personelManager ;
       private readonly IPersonelRoleManager _personelRoleManager ;

        public VacationController(IVacationManager vacationManager, IPersonelManager personelManager, IPersonelRoleManager personelRoleManager)
        {
            _vacationManager = vacationManager;
            _personelManager = personelManager;
            _personelRoleManager = personelRoleManager;
        }

        //[Authorize]
        [HttpGet]
        public ActionResult Index()
        
        {
            try
            {
                //Giiriş yapan kişi Kim? Sadece kendisine bağlı olan personellerin izinlerini listelsin
                var email = HttpContext.User.Identity.Name;
                var loggedinpersonel = _personelManager.GetPersonelProile(email);

                var list = _personelRoleManager.GetPersonelsofSenior(loggedinpersonel.Id);


                var model = _vacationManager.GetAllVacations(list,DateTime.Now.AddMonths(-2));
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Beklenmedik bir hata oluştu! {ex.Message}");
                return View(new List<PersonelVacation>());
            }
        }

        //[Authorize]
        public ActionResult Approve(int id, bool isApproved)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["ApproveErrorMsg"] = $"id değeri sıfırdan büyük olmalı!";
                    return RedirectToAction("Index");
                }
                var sonuc = _vacationManager.ApproveorDenyVacation(id, isApproved);
                if (sonuc && isApproved)
                {
                    TempData["ApproveSuccessMsg"] = $"İzin Yönetici tarafından  Onaylandı!";
                    return RedirectToAction("Index");
                }
                else if (sonuc && !isApproved)
                {
                    TempData["ApproveSuccessMsg"] = $"İzin Yönetici tarafından REddedildi!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ApproveErrorMsg"] = $"İşlem hatası oluştu!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ApproveErrorMsg"] = $"Beklenmedik bir hata oluştu! {ex.Message}";
                // ex loglamalıyız
                return RedirectToAction("Index");
            }
        }

        [AllowAnonymous]
        public ActionResult Deneme()
        {
            return View();
        }
    }
}