using EmployeeDAA.Api.Models.Common;
using EmployeeDAA.Api.Models.Employee;
using FluentValidation;

namespace EmployeeDAA.Api.Models.Order
{
    public class OrderInfoModel: BaseModel
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
        public bool IsDeleted { get; set; }


    }
    public class OrderInfoModelValidator : AbstractValidator<OrderInfoModel>
    {
        public OrderInfoModelValidator()
        {
            RuleFor(x => x.CustomerName).NotNull().NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.BuildingName).NotNull().NotEmpty().WithMessage("Address is required.");
            RuleFor(x => x.EmailAddress).NotNull().NotEmpty().WithMessage("EmailId is required.");
            RuleFor(x => x.MobileNo).NotNull().NotEmpty().WithMessage("Enter valid PhoneNo");
        }
    }

}
