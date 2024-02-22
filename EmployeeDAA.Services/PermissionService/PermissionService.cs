using DocumentFormat.OpenXml.Spreadsheet;
using EmployeeDAA.Core.Permissions;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Page = EmployeeDAA.Core.Permissions.Page;

namespace EmployeeDAA.Services.PermissionService
{
    public partial class PermissionService:IPermissionService
    {
        private readonly IRepository<Page> _PageRepository;
        private readonly IRepository<Module> _ModuleRepository;
        private readonly IRepository<EmployeeDAA.Core.Permissions.Permission> _PermissionRepository;


        public PermissionService(IRepository<Page> PageRepository, IRepository<Module> moduleRepository, IRepository<EmployeeDAA.Core.Permissions.Permission> permissionRepository)
        {
            _PageRepository = PageRepository;
            _ModuleRepository = moduleRepository;
            _PermissionRepository = permissionRepository;
        }

        public virtual async Task<List<Page>> GetAllModules(int RoleId)
        {
            IQueryable<Page> query = from PM in _PageRepository.Table
                                     join M in _ModuleRepository.Table on PM.ModuleId equals M.Id
                                     join P in _PermissionRepository.Table on
                                     new
                                     {
                                         Key1 = PM.Id,
                                         Key2 = true
                                     }
                                     equals
                                     new
                                     {
                                         Key1 = P.PageId,
                                         Key2 = P.RoleId == RoleId
                                     } into tmpPermission
                                     from P in tmpPermission.DefaultIfEmpty()
                                     where M.IsActive && PM.IsActive
                                     orderby M.Id
                                     select new Page()
                                     {
                                         ModuleName = M.ModuleName,
                                         PageId = PM.Id,
                                         PageName = PM.PageName,
                                         PageCode = PM.PageCode,
                                         ModuleId = PM.ModuleId,
                                         RoleId = RoleId,
                                         IsAdd = P.IsAdd,
                                         IsDelete = P.IsDelete,
                                         IsEdit = P.IsEdit,
                                         IsView = P.IsView,
                                         PermissionId = P.Id,
                                         PageType = PM.PageType
                                     };

            return await query.ToListAsync();
        }
        public virtual async Task InsertAsync(IList<EmployeeDAA.Core.Permissions.Permission> data, int UserId, string UserName)
        {
            foreach (Core.Permissions.Permission item in data)
            {
                IQueryable<Core.Permissions.Permission> query = from d in _PermissionRepository.Table
                                                                       where d.RoleId == item.RoleId && d.PageId == item.PageId
                                                                       select d;
                if (query.Any())
                {
                    await _PermissionRepository.UpdateAsync(item);
                }
                else
                {
                    await _PermissionRepository.InsertAsync(item);
                }
            }
            Permissions permisssions = new() { PermissionList = (IList<Core.Domain.Permission>)data };
        }
    }
    public class Permissions
    {
        public IList<EmployeeDAA.Core.Domain.Permission> PermissionList { get; set; }
    }
}

