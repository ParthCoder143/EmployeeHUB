using EmployeeDAA.Core.Domain;
using FluentValidation;

namespace EmployeeDAA.Api.Models.User
{
    public class CreateUserModel
    {
        public RoleTypes RoleId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string UserTokenKey { get; set; }
    }
    public class CreateUserModelValidator : AbstractValidator<CreateUserModel>
    {
        public CreateUserModelValidator()
        {
            RuleFor(x => x.UserTokenKey).NotNull().NotEmpty().WithMessage("UserToken is required");
            RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("First name is required").MaximumLength(250).WithMessage("First name maximumLength 250 character is required");
            RuleFor(x => x.LastName).NotNull().NotEmpty().WithMessage("Last name is required").MaximumLength(250).WithMessage("Last name maximumLength 250 character is required");
            RuleFor(x => x.UserName).NotNull().NotEmpty().WithMessage("User name is required").MaximumLength(250).WithMessage("User name maximumLength 250 character is required");
            RuleFor(x => x.Mobile).MaximumLength(15).WithMessage("Mobile maximumLength 15 character is required");
            RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Invalid email format.");

        }
    }
}
