using HRVacationSystemDL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRVacationSystemBL
{
    public class PersonelRoleManager : IPersonelRoleManager
    {
        //Burada context kullanılarak Personel tablosuyla ilgili işlemler yapılacak
        HRVacationEntities context = new HRVacationEntities();

        public List<int> GetPersonelsofSenior(int seniorId)
        {
            try
            {
                List<int> personels = new List<int>();
                personels = context.PersonelRole.Where(x=> x.RelatedSeniorId == seniorId).Select(x=>
               x.PersonelId).ToList();
                return personels;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool IsInRole(string email, string roleName)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(roleName))
                {
                    return false;
                }
                var personel = context.Personel.Where(x => x.Email.ToLower() == email.ToLower() && x.IsActive).FirstOrDefault();

                if (personel==null)
                {
                    throw new Exception("Personel bulunmadı ya da aktif değil!");
                }


                var role = context.Role.Where(x => x.Name.ToLower() == roleName.ToLower()).FirstOrDefault();
                var pr = context.PersonelRole.Where(x => x.RoleId == role.Id && x.PersonelId == personel.Id).ToList();

                return pr.Count > 0 ? true: false;

            }
            catch (Exception)
            {

                throw;
            }        }
    }
}
