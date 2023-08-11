using HRVacationSystemDL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRVacationSystemBL
{
   
    public class VacationManager : IVacationManager
    {
        HRVacationEntities context = new HRVacationEntities();

        
        public List<PersonelVacation> GetAllVacations(List<int> personelIdList,DateTime? date = null)
        {
            try
            {
                var vacations = context.PersonelVacation.ToList();
                if (personelIdList != null )
                {
                    vacations= vacations.Where(x=> personelIdList.Contains(x.PersonelId)).ToList();
                }
                if (date.HasValue)
                {
                    vacations = vacations.Where(x => x.CreatedDate >= date.Value).ToList();
                }
                return vacations;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ApproveorDenyVacation(int id, bool isApproved)
        {
            try
            {
                var vacation = context.PersonelVacation.FirstOrDefault(x => x.Id == id);
                vacation.IsApproved = isApproved;
                return context.SaveChanges() > 0 ? true : false;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}