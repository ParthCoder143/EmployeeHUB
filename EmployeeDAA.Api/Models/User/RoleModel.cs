using EmployeeDAA.Api.Models.Common;
using FluentValidation;

namespace EmployeeDAA.Api.Models.User
{
    public class RoleModel : BaseModel
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public int IsActive { get; set; }
        public int? RoleType { get; set; }
    }
    public class RoleModelValidator : AbstractValidator<RoleModel>
    {
        public RoleModelValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Role name is required");
        }
    }
}
