using HRVacationSystemDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRVacationSystemBL
{

    public interface IPersonelVacationManager
    {
        bool AddNewVacation(PersonelVacation model);

    }
    public class PersonelVacationManager : IPersonelVacationManager
    {
        HRVacationEntities context = new HRVacationEntities();

        public bool AddNewVacation(PersonelVacation model)
        {
            try
            {
                context.PersonelVacation.Add(model);
                return context.SaveChanges() > 0 ? true : false;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public PersonelVacation GetPersonelIzin(int id)
        {
            try
            {
                var personelVacation = context.PersonelVacation.FirstOrDefault(x => x.PersonelId == id);
                return personelVacation;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
