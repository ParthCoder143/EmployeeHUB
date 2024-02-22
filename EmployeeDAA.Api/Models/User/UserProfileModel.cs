using FluentValidation;

namespace EmployeeDAA.Api.Models.User
{
    public class UserProfileModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string UserName { get; set; }
    }
    public class UserProfileModelValidator : AbstractValidator<UserProfileModel>
    {
        public UserProfileModelValidator()
        {
            RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("First name is required");
            RuleFor(x => x.LastName).NotNull().NotEmpty().WithMessage("Last name is required");
            RuleFor(x => x.UserName).NotNull().NotEmpty().WithMessage("User name is required");
            RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
