using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Core.Permissions
{
    public class Page : BaseEntity
    {
        public int ModuleId { get; set; }
        public string PageName { get; set; }
        public string PageCode { get; set; }
        public bool IsShowMenu { get; set; }
        public bool IsActive { get; set; }
        public bool IsShowSearch { get; set; }
        public string TableNames { get; set; }

        [NoColumnMap]
        public string ModuleName { get; set; }
        [NoColumnMap]
        public int PageId { get; set; }
        [NoColumnMap]
        public int RoleId { get; set; }
        [NoColumnMap]
        public bool IsAdd { get; set; }
        [NoColumnMap]
        public bool IsDelete { get; set; }
        [NoColumnMap]
        public bool IsEdit { get; set; }
        [NoColumnMap]
        public bool IsView { get; set; }
        [NoColumnMap]
        public int PermissionId { get; set; }
        [NoColumnMap]
        public int PageType { get; set; }
        public enum EntityNameForAudit
        {
            Employee =0,
            Category =1,
            Product=2,
            User=3,
            Role=4,
            Page = 5,
            Permission = 6,
            Module = 7,

        }
    }
}
