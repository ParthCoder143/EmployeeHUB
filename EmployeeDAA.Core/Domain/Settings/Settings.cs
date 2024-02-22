using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Core.Domain.Settings
{
    public class Settings : BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
