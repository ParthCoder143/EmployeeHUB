using Microsoft.AspNetCore.Mvc;

namespace EmployeeDAA.Web.Controllers.Masters
{
    [Route("Category/{action}")]

    public class CategoryController : Controller
    {

        public const string BasePath = "~/Views/Masters/";
        public IActionResult Index()
        {
            return View(BasePath + "Category.cshtml");
        }
        public IActionResult ManageCategory()
        {
            return View(BasePath + "ManageCategory.cshtml");
        }
    }
}

