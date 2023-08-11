using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRVacationSystemBL
{
    public interface IPersonelRoleManager
    {
        List<int> GetPersonelsofSenior(int seniorId);
        bool IsInRole(string email, string roleName);
    }
}
