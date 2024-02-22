using FluentValidation;

namespace EmployeeDAA.Api.Models.Login
{
    public class AuthenticateModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string CaptchaCode { get; set; }
        public string CaptchaToken { get; set; }
    }
    public class RefreshAuthenticateModel
    {
        public string UserToken { get; set; }
    }
    public class ForgotPasswordModel
    {
        public string Email { get; set; }
    }
    public class AuthenticateModelValidator : AbstractValidator<AuthenticateModel>
    {
        public AuthenticateModelValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty().WithMessage("User Name is required.");
            //RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password is required");
        }
    }
}
