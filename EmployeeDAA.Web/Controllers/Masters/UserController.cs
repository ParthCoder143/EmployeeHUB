using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDAA.Web.Controllers.Masters
{
    [Route("User/{action}")]
    public class UserController : Controller
    {
        public const string BasePath = "~/Views/Users/";

        public IActionResult Index()
        {
            return View(BasePath + "User.cshtml");
        }
        public IActionResult ManageUsers()
        {
            return View(BasePath + "ManageUser.cshtml");
        }
        public IActionResult Profile()
        {
            return View(BasePath + "Profile.cshtml");
        }
    }
}
