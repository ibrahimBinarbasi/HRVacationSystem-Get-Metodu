using HRVacationSystemDL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRVacationSystemUI.Models
{
    public class PersonelViewModel //PersonelDTO Data Transfer Object
    {
        // personelin aynısının tıpkısından ELİMDE ARTIK VAR! 
        //Fakat buraada VERİTABANIYLA alakamız YOK!
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required(ErrorMessage = "İsim boş geçilemez!")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "İsim alanı min 2 mak 50 karakter olmalıdır!")]
        [Display(Name = "İsim")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Soyisim boş geçilemez!")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Soyisim alanı min 2 mak 50 karakter olmalıdır!")]
        [Display(Name = "Soyisim")]
        public string Surname { get; set; }
        public string Email { get; set; }
        [Display(Name = "Şifre")]
        public string Password { get; set; }
        public DateTime? BirthDate { get; set; }
        public int GenderPage { get; set; } // Ama ben sayfada GEnder alanını 0 1 2 olarak görmek istiyorum. ViewModel/DTO İşte böyle durumlarda KULLANILMAK için var.
        public bool? Gender { get; set; } //Veritabanında bool olarak Gender var

        public Nullable<System.DateTime> WorkStartDate { get; set; }
        public Nullable<System.DateTime> WorkEndDate { get; set; }
        public bool IsActive { get; set; }
        public string ProfilePicture { get; set; }

        public HttpPostedFileBase ProfilePictureUpload { get; set; }
        public List<PersonelRole> PersonelRole { get; set; } = new List<PersonelRole>();
        public List<PersonelVacation> PersonelVacation { get; set; } = new List<PersonelVacation>();
    }
}