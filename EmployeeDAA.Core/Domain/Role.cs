using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Core.Domain
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public RoleTypes RoleType { get; set; }
        [NoColumnMap]
        public string RoleTypeName { get; set; }

    }
    public enum RoleTypes
    {
        [Description("Admin")]
        Admin = -1,
        [Description("None")]
        None = 0,
        [Description("Administrator")]
        Administrator = 1,
        [Description("Content Manager")]
        ContentManager = 2,
        [Description("User Manager")]
        UserManager = 3,
        [Description("Reports Manager")]
        ReportsManager = 4, 
        [Description("Security Manager")]
        SecurityManager = 5,
        [Description("Inventory Manager")]
        InventoryManager = 6,
        [Description("Analytics Manager")]
        AnalyticsManager = 7,
        [Description("Order Manager")]
        OrderManager = 8,


    }

}
