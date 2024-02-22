using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Core.Domain.Orders
{
    public class OrderInfo:BaseEntity,ISoftDeletedEntity
    {
        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public DateTime?DateOfBirth { get; set; }

        public string MobileNo { get; set; }
        public string EmailAddress { get; set; }
        public string UnitNo { get; set; }
        public string Block { get; set; }
        public string Street { get; set; }
        public string BuildingName { get; set; }
        public string Country { get; set; }
        public int PostalCode { get; set; }
        public decimal FinalAmount { get; set; }

        public decimal DiscountAmount { get; set; }
        public decimal TotalPayableAmount { get; set; }
        public string PaymentOption { get; set; }

        public DateTime?OrderDate { get; set; }
        public bool IsDeleted { get; set; }


    }
}
    