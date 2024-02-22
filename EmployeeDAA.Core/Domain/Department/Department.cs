using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Core.Domain.Department
{
    public class Department:BaseEntity
    {
        public int EmployeeId { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentCode { get; set; }

        public bool IsActive { get; set; }
    }
}
