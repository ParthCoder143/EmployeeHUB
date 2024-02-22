using DocumentFormat.OpenXml.Spreadsheet;
using EmployeeDAA.Core;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core.Domain.Grid;
using EmployeeDAA.Core.Infrastructure;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Services.Users
{
    public partial class UserService:IUserService
    {
        private readonly IRepository<User> _UserRepository;
        private readonly IRepository<Role> _RoleRepository;

        public UserService(IRepository<User> UserRepository, IRepository<Role> RoleRepository)
        {
            _UserRepository = UserRepository;
            _RoleRepository = RoleRepository;
        }
        public virtual async Task<IList<User>> GetAllAsync()
        {
            return await _UserRepository.GetAllAsync(query =>
            {
                return from u in _UserRepository.Table
                       select new User()
                       {
                           Id = u.Id,
                       };
            });
        }

        public virtual async Task<IList<User>> GetUsersAllAsync()
        {
            return await _UserRepository.GetAllAsync(query =>
            {
                return from u in _UserRepository.Table
                       join r in _RoleRepository.Table on u.RoleId equals r.Id
                       where r.RoleType == RoleTypes.SecurityManager || r.RoleType == RoleTypes.ReportsManager ||
                        r.RoleType == RoleTypes.ContentManager || r.RoleType == RoleTypes.UserManager || r.RoleType == RoleTypes.InventoryManager
                        || r.RoleType == RoleTypes.OrderManager
                       select new User()
                       {
                           Id = u.Id,
                           IsActive = u.IsActive,
                           IsDeleted = u.IsDeleted,
                           FirstName = u.FirstName,
                           LastName = u.LastName,
                           RoleId = u.RoleId,
                           Role = r.Name,
                           UserName = u.UserName,
                           Email = u.Email,
                           Mobile = u.Mobile,
                           ModifyDate = u.ModifyDate,
                           Password = u.Password,
                           UserId = u.UserId,
                           Photo = u.Photo,
                           RoleType = r.RoleType,
                       };
            });
        }


        public virtual async Task<IList<User>> GetRoleTypesUsersAllAsync(RoleTypes roleTypes)
        {
            return await _UserRepository.GetAllAsync(query =>
            {
                return from u in _UserRepository.Table
                       join r in _RoleRepository.Table on u.RoleId equals r.Id
                       where r.RoleType == roleTypes && u.IsActive && !u.IsDeleted
                       select new User()
                       {
                           Id = u.Id,
                           IsActive = u.IsActive,
                           IsDeleted = u.IsDeleted,
                           FirstName = u.FirstName,
                           LastName = u.LastName,
                           RoleId = u.RoleId,
                           Role = r.Name,
                           UserName = u.UserName,
                           Email = u.Email,
                           Mobile = u.Mobile,
                           ModifyDate = u.ModifyDate,
                           Password = u.Password,
                           UserId = u.UserId,
                           Photo = u.Photo,
                           RoleType = r.RoleType,
                       };
            });
        }
        public virtual async Task UpdateAsync(User user, int UserId, string Username)
        {
            user.ModifyBy = UserId;
            user.ModifyDate = DateTime.Now;
            await _UserRepository.UpdateAsync(user);
           
        }

        public virtual async Task<int> GetUsersByUserToken(string UserToken, string UserId)
        {
            return (await _UserRepository.GetAllAsync(query => query.Where(x => x.UserToken.ToLower() == UserToken.ToLower() && Convert.ToString(x.Id) == UserId)))?.ToList()?.Count ?? 0;
        }

        public virtual async Task<IPagedList<User>> GetAllAsync(GridRequestModel objGrid)
        {
            IQueryable<User> query = from u in _UserRepository.Table
                                     join r in _RoleRepository.Table on u.RoleId equals r.Id
                                     select new User()
                                     {
                                         Id = u.Id,
                                         IsActive = u.IsActive,
                                         IsDeleted = u.IsDeleted,
                                         FirstName = u.FirstName,
                                         LastName = u.LastName,
                                         RoleId = u.RoleId,
                                         Role = r.Name,
                                         UserName = u.UserName,
                                         Email = u.Email,
                                         Mobile = u.Mobile,
                                         ModifyDate = u.ModifyDate,
                                         Password = u.Password,
                                         UserId = u.UserId,
                                         Photo = u.Photo,
                                     };
            return await _UserRepository.GetAllPagedAsync(objGrid, query);
        }

        public virtual async Task<IPagedList<User>> GetUsersRoleTypewiseAllAsync(GridRequestModel objGrid)
        {

            IQueryable<User> query = from u in _UserRepository.Table
                                     join r in _RoleRepository.Table on u.RoleId equals r.Id
                                     where u.IsActive && !u.IsDeleted
                                     select new User()
                                     {

                                         Id = u.Id,
                                         IsActive = u.IsActive,
                                         IsDeleted = u.IsDeleted,
                                         FirstName = u.FirstName,
                                         UserName = u.UserName,
                                         LastName = u.LastName,
                                         Email = u.Email,
                                         RoleId = u.RoleId,
                                         Role = r.Name,
                                         RoleType = r.RoleType

                                     };
            return await _UserRepository.GetAllPagedAsync(objGrid, query);
        }
        public virtual async Task<User> GetByIdAsync(int Id)
        {
            return await _UserRepository.GetByIdAsync(Id);
        }
        public virtual async Task<IList<User>> GetByIdsAsync(IList<int> ids)
        {
            return await _UserRepository.GetByIdsAsync(ids);
        }
        public virtual async Task<IList<User>> GetUsersByRoleId(int roleId)
        {
            return await _UserRepository.GetAllAsync(query => query.Where(x => x.RoleId == roleId));
        }
        public virtual async Task<IList<User>> GetUsersByRoleIds(IList<int> ids)
        {
            return await _UserRepository.GetAllAsync(query => query.Where(entry => ids.Contains(entry.RoleId)));
        }

        public virtual async Task InsertAsync(User user, int UserId, string Username)
        {
            user.IsActive = true;
            await _UserRepository.InsertAsync(user);
        }
        public virtual async Task DeleteAsync(IList<User> user, int UserId, string Username)
        {
            await _UserRepository.DeleteAsync(user);
        
        }

        public virtual async Task<int> CheckUserNameDuplication(string UserName, string Email, int Id)
        {
            IList<User> result = await _UserRepository.GetAllAsync(query =>
            {
                return query.Where(x => (x.UserName.ToLower().Trim() == UserName.ToLower().Trim() || x.Email.ToLower().Trim() == Email.ToLower().Trim()) && x.Id != Id && !x.IsDeleted);
            });
            if ((result?.Where(x => x.UserName.ToLower().Trim() == UserName.ToLower().Trim())?.ToList()?.Count ?? 0) > 0)
            {
                return 1;
            }
            else if ((result?.Where(x => x.Email.ToLower().Trim() == Email.ToLower().Trim())?.ToList()?.Count ?? 0) > 0)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }

        public virtual async Task<User> CheckLogin(string UserName, string Password)
        {
            string p = EncryptionUtility.EncryptText(Password, SecurityHelper.OrderEncryptionKey);
            User result = await (from u in _UserRepository.Table
                                 join r in _RoleRepository.Table on u.RoleId equals r.Id
                                 where u.UserName.ToLower().Trim() == UserName.ToLower().Trim() && u.Password == p && u.IsActive && !u.IsDeleted
                                 select new User()
                                 {
                                     Id = u.Id,
                                     UserId = u.UserId,
                                     RoleId = u.RoleId,
                                     Role = r.Name,
                                     RoleType = r.RoleType,
                                     UserName = u.UserName,
                                     Password = u.Password,
                                     FirstName = u.FirstName,
                                     LastName = u.LastName,
                                     Email = u.Email,
                                     Mobile = u.Mobile,
                                     IsActive = u.IsActive,
                                     IsDeleted = u.IsDeleted,
                                     Photo = u.Photo,
                                     CameraUrl = u.CameraUrl
                                 }).FirstOrDefaultAsync();
            return result;
        }
        public virtual async Task<User> GetUserByUserName(string UserName)
        {
            IQueryable<User> query = from u in _UserRepository.Table
                                     where u.UserName.ToLower().Trim() == UserName.ToLower().Trim()
                                     select u;
            return await query?.FirstOrDefaultAsync() ?? new User();
        }


    }
}
