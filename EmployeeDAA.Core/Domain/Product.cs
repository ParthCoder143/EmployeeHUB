using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Core.Domain
{
    public class Product : BaseEntity, ISoftDeletedEntity
    {
        public int CategoryId { get; set; }

        [NoColumnMap]
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Photo { get; set; }
        public string CameraUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public double SortOrder { get; set; }
    }
    
}
