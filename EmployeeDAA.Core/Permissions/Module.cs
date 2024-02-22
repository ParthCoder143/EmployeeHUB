using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Core.Permissions
{
    public class Module : BaseEntity
    {
        public string ModuleName { get; set; }
        public bool IsActive { get; set; }
    }
}
