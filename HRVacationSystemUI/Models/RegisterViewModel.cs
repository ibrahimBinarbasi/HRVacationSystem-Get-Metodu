using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRVacationSystemUI.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]

        public string Surname { get; set; }
        [Required]

        //TODO: Regular Expression
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password",ErrorMessage ="Şifreler uyuşmuyor!")]
        public string ConfirmPassword { get; set; }

        public DateTime? BirthDate { get; set; }
        public bool? Gender { get; set; }
        public int GenderPage { get; set; }

    }
}