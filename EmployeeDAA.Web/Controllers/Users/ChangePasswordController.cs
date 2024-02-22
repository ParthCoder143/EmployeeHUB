using Microsoft.AspNetCore.Mvc;

namespace EmployeeDAA.Web.Controllers.Users
{
    [Route("User/{action}")]
    public class ChangePasswordController : Controller
    {
        public const string BasePath = "~/Views/Users/";
        public IActionResult ChangePassword()
        {
            return View(BasePath + "ChangePassword.cshtml");
        }
    }
}
