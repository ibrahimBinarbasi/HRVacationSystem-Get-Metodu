using HRVacationSystemBL;
using HRVacationSystemDL;
using HRVacationSystemUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HRVacationSystemUI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IPersonelManager _personelManager;

        public LoginController(IPersonelManager personelManager)
        {
            _personelManager = personelManager;
        }



        //HTTPGET olacaktır
        [HttpGet] // yazmasanız da bu metot HTTPGET
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(string txtEmail = null, string txtPassword = null)
        {
            //Kişi eğer giriş yapabilirse başka göndereceğiz
            //Giriş yapamazsa aynı sayfa göndereceğiz
            try
            {
                if (string.IsNullOrEmpty(txtEmail) || string.IsNullOrEmpty(txtPassword))
                {
                    ViewBag.ErrorMessage = $"Lütfen email ve parola giriniz!";
                    return View();
                }
                // Burayı geçerse... Kullanıcı veritabanındaki personel tablosunda var mı diye bakacağız...
                var personel = _personelManager.GetPersonelProile(txtEmail);
                
                //Yoksa Hatalı Email ya da şifre
                if (!_personelManager.PersonelSignIn(txtEmail,txtPassword))
                {
                    ViewBag.ErrorMessage = $"Hatalı Email ya da Şifre!";
                    return View();
                }
                //Varsa login olacaktır
                //Yönlendirilmesi gereken sayfaya yönlendirilebilir.
                //Profil sayfasına gitsin
                FormsAuthentication.SetAuthCookie(txtEmail, false);

                return RedirectToAction("Profile", "Personel", new { email = txtEmail });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Beklenmedik sorun oluştu!{ex.Message}";
                //ToDo: ex loglanacaktır kullanıcya gösterilmeyecektir.
                return View();
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("SignIn", "Login");
        }    

        public ActionResult Register() 
        {
            List<SelectListItem> Genders = new List<SelectListItem>()
            {
                 new SelectListItem
            {
                Text = "Cinsiyet seçiniz",
                Value = "0"
            },
            new SelectListItem
            {
                Text = "Kadın",
                Value = "1"
            },
            new SelectListItem{
                Text = "Erkek",
                Value = "2"
            }};

            ViewBag.Genders = Genders;
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            try
            {
                List<SelectListItem> Genders = new List<SelectListItem>()
            {
                 new SelectListItem
            {
                Text = "Cinsiyet seçiniz",
                Value = "0"
            },
            new SelectListItem
            {
                Text = "Kadın",
                Value = "1"
            },
            new SelectListItem{
                Text = "Erkek",
                Value = "2"
            }};

                ViewBag.Genders = Genders;
                if (!ModelState.IsValid)
                {
                    return View();
                }

                Personel yeniPersonel = new Personel()
                {
                    Name=model.Name,
                    Surname=model.Surname,
                    BirthDate=model.BirthDate,
                    Email=model.Email,
                    IsActive=true,
                    CreatedDate= DateTime.Now

            };
                if (model.GenderPage == 1 || model.GenderPage == 2)
                {
                    yeniPersonel.Gender = model.GenderPage == 1 ? true : false;

                }
                //parola
                // yeniPersonel.Password= PersonelManager.MD5EncryptPassword(model.Password);
                //yeniPersonel.Password = PersonelManager.SHA384Hash(model.Password);
                //Personele rol verilmedi.

               string salt= MyPasswordHasher.CreateSalt512();
                yeniPersonel.Password = MyPasswordHasher.GenerateHMAC(model.Password, salt);
                yeniPersonel.Salt = salt;
                if (_personelManager.AddNewPersonel(yeniPersonel))
                {
                    return RedirectToAction("SignIn");
                }
                else
                {
                    ModelState.AddModelError("", "Yeni personel eklenemedi!");
                    return View();
                }



            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
