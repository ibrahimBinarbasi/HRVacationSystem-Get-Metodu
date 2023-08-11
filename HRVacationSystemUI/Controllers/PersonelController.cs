using HRVacationSystemBL;
using HRVacationSystemDL;
using HRVacationSystemUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Infrastructure;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace HRVacationSystemUI.Controllers
{
    public class PersonelController : Controller
    {
        private readonly IPersonelManager _personelManager;

        public PersonelController(IPersonelManager personelManager)
        {
            _personelManager = personelManager;
        }

        // GET: Personel
        [HttpGet]
        public ActionResult Profile(string email)
        {
            try
            {
                //Giriş yapmış olan personel bilgileri
                if (string.IsNullOrEmpty(email))
                {
                    TempData["ProfilePageError"] = $"Lütfen giriş yaptığınıza emin olun!";
                    return RedirectToAction("Signin", "Login");
                }
                //var personel = _personelManager.GetPersonelProile(email);

                //yukarıdaki personeli managerdan değil APiden alalım
                var personel = new Personel();
                using (WebClient client = new WebClient())
                {
                    #region GEt YAPILIYOR
                    string url = $"http://localhost:55778/hg/personel?email={email}";
                    var yanit = client.DownloadString(url);
                    #endregion

                    #region GET NETİCESİNDE ÇIKTI ALINIYOR
                    var response = JsonConvert.DeserializeObject<APIResponse<Personel>>(yanit);
                     personel = response.Data;
                    #endregion

                }
                //Bu yöntem çok uzun KISA YOLU var mı? EVET 
                PersonelViewModel model = new PersonelViewModel()
                {
                    Id = personel.Id,
                    Name = personel.Name,
                    Surname = personel.Surname,
                    Email = personel.Email,
                    Password = personel.Password,
                    BirthDate = personel.BirthDate,
                    WorkEndDate = personel.WorkEndDate,
                    WorkStartDate = personel.WorkStartDate,
                    IsActive = personel.IsActive,
                    ProfilePicture = personel.ProfilePicture,
                    Gender = personel.Gender,
                    CreatedDate = personel.CreatedDate,
                    GenderPage = personel.Gender == true ? 1 : 2
                }
                ;
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

                return View(model); // sayfaya model gönderildi!
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", $"Beklenmedik bir hata oluştu! {ex.Message}");
                //Ex loglanlanmalı
                return View(new PersonelViewModel());
            }
        }

        [HttpPost]
        public ActionResult Profile(PersonelViewModel model)
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

                //ViewModel DTO 
                if (!ModelState.IsValid) // Model geçerli mi? HAYIR geliyor ife girer
                {
                    return View(model);
                }

                //kişinin bilgilerini güncelle
                var guncellenecekPersonel = _personelManager.GetPersonelProile(model.Email);
                if (guncellenecekPersonel == null)
                {
                    ModelState.AddModelError("", $"Personel bulunmadı!");
                    return View(model);

                }
                guncellenecekPersonel.Name = model.Name;
                guncellenecekPersonel.Surname = model.Surname;
                guncellenecekPersonel.BirthDate = model.BirthDate;
                if (model.GenderPage == 1 || model.GenderPage == 2)
                {
                    guncellenecekPersonel.Gender = model.GenderPage == 1 ? true : false;

                }
                else
                {
                    ModelState.AddModelError("", $"Cinsiyer seçimi zorunludur!");
                    return View(model);

                }

                #region ProfilResmi
                if (model.ProfilePictureUpload!=null && model.ProfilePictureUpload.ContentType.Contains("" +
                    "image") && model.ProfilePictureUpload.ContentLength >0)
                {
                    var personelemail = model.Email.Substring(0, model.Email.IndexOf('@'));


                    string fileName = $"{personelemail}-{Guid.NewGuid().ToString().Replace("-","")}";

                    string uzanti = Path.GetExtension(model.ProfilePictureUpload.FileName);
                    string directoryPath =
                        Server.MapPath($"~/ProfilePictures");
                    string filePath = Server.MapPath($"~/ProfilePictures/{fileName}{uzanti}");

                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath); 
                    
                    model.ProfilePictureUpload.SaveAs(filePath);
                    guncellenecekPersonel.ProfilePicture = $"/ProfilePictures/{fileName}{uzanti}";

                }
                else
                {
                    ModelState.AddModelError("", $"Lütfen doğru formatta veya yeterli boyutta resim seçiniz!");
                    return View(model);
                }


                #endregion  


                if (_personelManager.PersonelUpdate())
                {
                    TempData["PersonelUpdate"] = "Personel bilgileri güncellendi!";
                    return RedirectToAction("Profile", "Personel", new { email = model.Email });
                }
                else
                {
                    ModelState.AddModelError("", $"Bilgilerinizi güncellerken hata oluştu!");
                    return View(model);
                }

                //Eğer yeni bir profil resmi seçtiyse profil resmini güncelle


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Beklenmedik bir hata oluştu! {ex.Message}");
                //Ex loglanlanmalı
                return View(model);
            }
        }
    }
}