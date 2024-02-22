using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Core.Infrastructure
{
    public class SystemMessage : BaseEntity
    {

        public string TagCode { get; set; }

        public string TagMsg { get; set; }

        public bool IsActive { get; set; }
    }
}
