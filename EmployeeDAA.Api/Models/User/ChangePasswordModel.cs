using FluentValidation;

namespace EmployeeDAA.Api.Models.User
{
    public class ChangePasswordModel
    {
        public string Oldpassword { get; set; }
        public string Newpassword { get; set; }
        public string Confirmpassword { get; set; }
    }
    public class ChangePasswordModellValidator : AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordModellValidator()
        {
            RuleFor(x => x.Oldpassword).NotNull().NotEmpty().WithMessage("Old password is required");
            RuleFor(x => x.Newpassword).NotNull().NotEmpty().WithMessage("New password is required").MinimumLength(5).WithMessage("New Password minimum 5 character is required");
            RuleFor(x => x.Confirmpassword).NotNull().NotEmpty().WithMessage("Confirm password is required").MinimumLength(5).WithMessage("Confirm Password minimum 5 character is required");
            RuleFor(x => x.Newpassword).Equal(x => x.Confirmpassword).WithMessage("New Password and confirmation password does not match.");
        }
    }
}
