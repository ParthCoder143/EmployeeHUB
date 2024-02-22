using EmployeeDAA.Api.Models.Common;
using EmployeeDAA.Core;
using EmployeeDAA.Core.Domain;
using FluentValidation;

namespace EmployeeDAA.Api.Models.User
{
    public class UserModel:BaseModel
    {

        public int RoleId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int? UserTypeId { get; set; }
        public string Photo { get; set; }
        public IFormFile Imgupload { get; set; }
        public string CameraUrl { get; set; }


    }

    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("First name is required");
            RuleFor(x => x.LastName).NotNull().NotEmpty().WithMessage("Last name is required");
            RuleFor(x => x.UserName).NotNull().NotEmpty().WithMessage("User name is required");
            RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Invalid email format.");
            
        }
    }
}
