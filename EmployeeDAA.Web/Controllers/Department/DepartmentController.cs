using DocumentFormat.OpenXml.Math;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDAA.Web.Controllers.Department
{
    [Route("Department/{action}")]
    public class DepartmentController : Controller
    {
        public const string BasePath = "~/Views/Department/";
        public IActionResult Index()
        {
            return View(BasePath + "Department.cshtml");
        }
        public IActionResult ManageDepartment()
        {
            return View(BasePath + "ManageDepartment.cshtml");
        }
    }
}
