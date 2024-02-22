using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Core.Domain
{
    public class Categories : BaseEntity, ISoftDeletedEntity
    {
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public double SortOrder { get; set; }
    }
}
