using DocumentFormat.OpenXml.Spreadsheet;
using EmployeeDAA.Core;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core.Domain.Grid;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Services.Users
{
    public partial class RoleService : IRoleService
    {
        private readonly IRepository<Role> _RoleRepository;

        public RoleService(IRepository<Role> roleRepository)
        {
            _RoleRepository = roleRepository;
        }
        public virtual async Task<Role> GetByIdAsync(int Id)
        {
            return await _RoleRepository.GetByIdAsync(Id);
        }
        public virtual async Task<IList<Role>> GetByIdsAsync(IList<int> ids)
        {
            return await _RoleRepository.GetByIdsAsync(ids);
        }
        public virtual async Task<IPagedList<Role>> GetAllAsync(GridRequestModel objGrid)
        {
            IQueryable<Role> query = from d in _RoleRepository.Table
                                     where d.IsActive
                                     select new Role()
                                     {
                                         Id = d.Id,
                                         IsActive = d.IsActive,
                                         Name = d.Name,
                                         RoleType = d.RoleType,
                                         RoleTypeName = Convert.ToString(d.RoleType)
                                     };
            return await _RoleRepository.GetAllPagedAsync(objGrid, query);

        }
        public virtual async Task<IList<Role>> GetAllAsync(bool? isActive = null)
        {
            return await _RoleRepository.GetAllAsync(query =>
            {
                return query.Where(x => x.IsActive);
            });
        }
        public virtual async Task InsertAsync(Role role, int UserId, string Username)
        {
            await _RoleRepository.InsertAsync(role);
        }
        public virtual async Task UpdateAsync(Role role, int UserId, string Username)
        {
            await _RoleRepository.UpdateAsync(role);
        }
        public virtual async Task UpdateAsync(IList<Role> roleList, int UserId, string Username)
        {
            await _RoleRepository.UpdateAsync(roleList);
        }
        public virtual async Task<bool> IsNameExist(string Name, int Id)
        {
            IList<Role> result = await _RoleRepository.GetAllAsync(query =>
            {
                return query.Where(x => x.Name.ToLower().Trim() == Name.ToLower().Trim() && x.Id != Id && x.IsActive);
            });
            return result.Count > 0;
        }
        public virtual async Task<Role> GetRoleByRoleType(RoleTypes RoleType)
        {
            IQueryable<Role> query = from r in _RoleRepository.Table
                                     where r.RoleType == RoleType
                                     select r;
            return await query?.FirstOrDefaultAsync() ?? new Role();
        }

    }


}
