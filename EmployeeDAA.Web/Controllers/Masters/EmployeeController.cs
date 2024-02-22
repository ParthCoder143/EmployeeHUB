using Microsoft.AspNetCore.Mvc;

namespace EmployeeDAA.Web.Controllers.Masters
{
    [Route("Employee/{action}")]

    public class EmployeeController : Controller
    {
        public const string BasePath = "~/Views/Masters/";
        public IActionResult Index()
        {
            return View(BasePath + "Employee.cshtml");
        }
        public IActionResult ManageEmployee()
        {
            return View(BasePath + "ManageEmployee.cshtml");
        }
    }
}
