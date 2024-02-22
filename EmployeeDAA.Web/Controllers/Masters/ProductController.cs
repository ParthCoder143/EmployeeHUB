using Microsoft.AspNetCore.Mvc;

namespace EmployeeDAA.Web.Controllers.Masters
{
    [Route("Product/{action}")]

    public class ProductController : Controller
    {
        public const string BasePath = "~/Views/Masters/";

        public IActionResult Index()
        {
            return View(BasePath + "Product.cshtml");
        }
        public IActionResult ManageProduct()
        {
            return View(BasePath + "ManageProduct.cshtml");
        }

    }
}
