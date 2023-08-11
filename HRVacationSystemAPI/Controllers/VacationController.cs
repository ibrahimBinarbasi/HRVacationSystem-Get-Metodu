using HRVacationSystemAPI.Models;
using HRVacationSystemBL;
using HRVacationSystemDL;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace HRVacationSystemAPI.Controllers
{
    [System.Web.Http.RoutePrefix("hg")]
    public class VacationController : ApiController
    {
        PersonelManager personelManager = new PersonelManager();
        PersonelVacationManager personelVacationManager = new PersonelVacationManager();
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("deneme")]

        public APIResponse<PersonelVacation> GetPersonelIzinler(string email)
        {
            try
            {

                var personel = personelManager.GetPersonelProile(email);
                if (personel == null)
                    return new APIResponse<PersonelVacation>()
                    {
                        IsSuccess = false,
                        Message = $"Bu kişi sistemde kayıtlı değildir!",
                        Data = new PersonelVacation()
                    };
                else
                {
                    var vacation = personelVacationManager.GetPersonelIzin(personel.Id);
                    if (vacation == null)
                    {
                        return new APIResponse<PersonelVacation>()
                        {
                            IsSuccess = false,
                            Message = $"{personel.Name} adlı çalışanın izin talebi yoktur.!",
                            Data = new PersonelVacation()
                        };
                    }

                    else
                        return new APIResponse<PersonelVacation>()
                        {
                            IsSuccess = true,
                            Message = $"{personel.Name} adlı çalışanın izin talebi vardır!",
                        };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse<PersonelVacation>()
                {
                    IsSuccess = false,
                    Message = $"Beklenmedik bir hata oldu! {ex.Message}",
                    Data = new PersonelVacation()
                };
            }
        }




        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("personel")]
        public APIResponse<PersonelDTO> GetPersonelInfo(string email)
        {
            try
            {
                var personel = personelManager.GetPersonelProile(email);
                if (personel == null)
                    return new APIResponse<PersonelDTO>()
                    {
                        IsSuccess = false,
                        Message = $"Bu kişi sistemde kayıtlı değildir!",
                        Data = new PersonelDTO()
                    };
                else
                    return new APIResponse<PersonelDTO>()
                    {
                        IsSuccess = true,
                        Message = $"Personel bilgileri bulundu!",
                        Data = new PersonelDTO()
                        {
                            Name = personel.Name,
                            Surname = personel.Surname,
                            Email = personel.Email,
                            BirthDate = personel.BirthDate,
                            Gender = personel.Gender,
                            WorkEndDate = personel.WorkEndDate,
                            WorkStartDate = personel.WorkStartDate,
                            IsActive = personel.IsActive,
                            CreatedDate = personel.CreatedDate,
                            Id = personel.Id,
                            ProfilePicture = personel.ProfilePicture
                        }
                        // Data= personel.Adapt<PersonelDTO>()
                    };

            }
            catch (Exception ex)
            {
                return new APIResponse<PersonelDTO>()
                {
                    IsSuccess = false,
                    Message = $"Beklenmedik bir hata oldu! {ex.Message}",
                    Data = new PersonelDTO()
                };
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("izin")]
        public APIResponse<string> AddNewVacation([FromBody] PersonelVacation model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new APIResponse<string>()
                    {
                        IsSuccess = false,
                        Message = $"Bilgileri eksiksiz giriniz!",
                        Data = string.Empty
                    };
                }
                model.CreatedDate = DateTime.Now;
                if (personelVacationManager.AddNewVacation(model))
                {
                    TimeSpan span = new TimeSpan();
                    span = model.VacationEndDate.Value - model.VacationStartDate.Value;
                    return new APIResponse<string>()
                    {
                        IsSuccess = false,
                        Message = $"İzin talebi oluşturuldu.",
                        Data = $"{span.TotalDays} gün izin talep ettiniz"
                    };

                }
                else
                {
                    return new APIResponse<string>()
                    {
                        IsSuccess = false,
                        Message = $"Başarısız!",
                        Data = string.Empty
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse<string>()
                {
                    IsSuccess = false,
                    Message = $"Beklenmedik bir hata oldu! {ex.Message}",
                    Data = string.Empty
                };
            }
        }


    }
}