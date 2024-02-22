using Microsoft.AspNetCore.Mvc;

namespace EmployeeDAA.Web.Controllers.Permission
{
    [Route("Permission/{action}")]
    public class PermissionController : Controller
    {
        public const string BasePath = "~/Views/Permission/";

        #region :: Permission ::
        public IActionResult Permission()
        {
            return View(BasePath + "Permission.cshtml");
        }
        #endregion
    }
}
