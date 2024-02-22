using Microsoft.AspNetCore.Mvc;

namespace EmployeeDAA.Web.Controllers.Masters
{
   [Route("Role/{action}")]
    public class RoleController : Controller
    {
        public const string BasePath = "~/Views/Users/";
        public IActionResult Index()
        {
            return View(BasePath + "Role.cshtml");
        }
        public IActionResult ManageRole()
        {
            return View(BasePath + "ManageRole.cshtml");
        }
    }

}
