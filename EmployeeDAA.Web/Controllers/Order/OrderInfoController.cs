using Microsoft.AspNetCore.Mvc;

namespace EmployeeDAA.Web.Controllers.Order
{
    [Route("Order/{action}")]

    public class OrderInfoController : Controller
    {
        public const string BasePath = "~/Views/Order/";

        public IActionResult Index()
        {
            return View(BasePath + "OrderInfo.cshtml");
        }
        public IActionResult ManageOrderInfo()
        {
            return View(BasePath + "ManageOrderInfo.cshtml");
        }
        public IActionResult Order()
        {
            return View(BasePath + "Order.cshtml");
        }
        public IActionResult ManageOrder()
        {
            return View(BasePath + "ManageOrder.cshtml");
        }
    }
}
