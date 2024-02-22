using EmployeeDAA.Core.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Services.PermissionService
{
    public interface IPermissionService
    {
        Task<List<Page>> GetAllModules(int RoleId);
        Task InsertAsync(IList<EmployeeDAA.Core.Permissions.Permission> data, int UserId, string UserName);
    }
}
