using HRVacationSystemDL;
using System.Collections.Generic;
using System;

namespace HRVacationSystemBL
{
    public interface IVacationManager
    {
        List<PersonelVacation> GetAllVacations(List<int> personelIdList, DateTime? date = null);
         bool ApproveorDenyVacation(int id, bool isApproved);
    }
}