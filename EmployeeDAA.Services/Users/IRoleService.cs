using EmployeeDAA.Core.Domain.Grid;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Services.Users
{
    public partial interface IRoleService
    {
        #region Methods

        Task<Role> GetByIdAsync(int Id);
        Task<IList<Role>> GetByIdsAsync(IList<int> ids);

        Task<IPagedList<Role>> GetAllAsync(GridRequestModel objGrid);
        Task<IList<Role>> GetAllAsync(bool? isActive = null);
        Task InsertAsync(Role role, int UserId, string Username);
        Task UpdateAsync(Role role, int UserId, string Username);
        Task UpdateAsync(IList<Role> roleList, int UserId, string Username);
        Task<bool> IsNameExist(string Name, int Id);
        Task<Role> GetRoleByRoleType(RoleTypes RoleType);
        #endregion
    }
}
