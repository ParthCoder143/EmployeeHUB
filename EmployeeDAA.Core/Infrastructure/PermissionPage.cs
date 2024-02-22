using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Core.Infrastructure
{
    public enum PageName
    {
        [Description("Employee")]
        NoPage,
        [Description("Employee")]
        AdmMasEmployee,
        [Description("Role")]
        AdmMasRoles,
        [Description("User")]
        AdmMasUsers,
        [Description("Permission")]
        AdmRolePermission,
        [Description("Category")]
        AdmMasCategory,
        [Description("Product")]
        AdmMasProduct,
       
    }
    public enum PagePermission
    {
        Add = 1, Edit = 2, Delete = 3, View = 4, Upload = 5, Print = 6
    }
}
