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
    public partial interface IUserService
    {
        #region Methods
        Task<IList<User>> GetAllAsync();
        Task<IPagedList<User>> GetAllAsync(GridRequestModel objGrid);

        Task<IPagedList<User>> GetUsersRoleTypewiseAllAsync(GridRequestModel objGrid);

        Task<IList<User>> GetUsersAllAsync();

        Task<IList<User>> GetRoleTypesUsersAllAsync(RoleTypes roleTypes);

        Task<User> GetByIdAsync(int Id);
        Task<IList<User>> GetByIdsAsync(IList<int> ids);
        Task<IList<User>> GetUsersByRoleId(int roleId);
        Task<IList<User>> GetUsersByRoleIds(IList<int> ids);
        Task InsertAsync(User user, int UserId, string Username);
        Task UpdateAsync(User user, int UserId, string Username);
        Task<int> GetUsersByUserToken(string UserToken, string UserId);

        Task DeleteAsync(IList<User> user, int UserId, string Username);
        Task<int> CheckUserNameDuplication(string UserName, string Email, int Id);
        Task<User> CheckLogin(string UserName, string Password);
        Task<User> GetUserByUserName(string UserName);
        #endregion
    }
}
