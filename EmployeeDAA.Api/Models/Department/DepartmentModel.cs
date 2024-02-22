using EmployeeDAA.Api.Models.Common;
using EmployeeDAA.Core;
using FluentValidation;

namespace EmployeeDAA.Api.Models.Department
{
    public class DepartmentModel:BaseModel
    {
        public int EmployeeId { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentCode { get; set; }

        public int IsActive { get; set; }
    }
    public class DepartmentModelValidator: AbstractValidator<DepartmentModel>
    {
        public DepartmentModelValidator()
        {
            RuleFor(x =>x.EmployeeId).NotNull().NotEmpty().WithMessage("EmployeeId is Required");
            RuleFor(x =>x.DepartmentName).NotNull().NotEmpty().WithMessage("EmployeeId is Required");
            RuleFor(x =>x.DepartmentCode).NotNull().NotEmpty().WithMessage("EmployeeId is Required");
        }
    }
}
