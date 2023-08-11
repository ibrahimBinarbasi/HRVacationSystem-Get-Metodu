using HRVacationSystemDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRVacationSystemBL
{
    public interface IPersonelManager
    {
         bool PersonelSignIn(string email, string password);
         Personel GetPersonelProile(string email);
         bool PersonelUpdate();

        bool AddNewPersonel(Personel personel);

    }
}
