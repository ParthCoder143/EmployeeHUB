using System.ComponentModel.DataAnnotations;

namespace EmployeeDAA.Api.Models.Login
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public int RoleId { get; set; }

    }
}
