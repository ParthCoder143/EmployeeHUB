using EmployeeDAA.Api.Models.Common;
using FluentValidation;

namespace EmployeeDAA.Api.Models.Employee
{
    public class EmployeeModel:BaseModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string EmailId { get; set; }
        public string PhoneNo { get; set; }
    }
    public class EmployeeModelValidator : AbstractValidator<EmployeeModel>
    {
        public EmployeeModelValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Address).NotNull().NotEmpty().WithMessage("Address is required.");
            RuleFor(x => x.EmailId).NotNull().NotEmpty().WithMessage("EmailId is required.");
            RuleFor(x => x.PhoneNo).NotNull().NotEmpty().WithMessage("Enter valid PhoneNo");
        }
    }
}
