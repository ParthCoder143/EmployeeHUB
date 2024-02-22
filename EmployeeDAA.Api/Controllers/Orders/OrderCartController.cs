using EmployeeDAA.Api.Extensions;
using EmployeeDAA.Api.InfraStructure;
using EmployeeDAA.Api.Models.Order;
using EmployeeDAA.Api.Models;
using EmployeeDAA.Core.Domain.Grid;
using EmployeeDAA.Core.Domain.Orders;
using EmployeeDAA.Services.Order;
using EmployeeDAA.Services.Products;
using Microsoft.AspNetCore.Mvc;
using EmployeeDAA.Api.Models.Product;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core.Infrastructure;

namespace EmployeeDAA.Api.Controllers.Orders
{
    [Route("api/[controller]")]
    [ApiController]


    public class OrderCartController : Controller
    {
        private readonly IOrderCartService _orderCartService;
        private readonly IProductServices _productServices;

        public OrderCartController(IOrderCartService orderCartService, IProductServices productServices)
        {
            _orderCartService = orderCartService;
            _productServices = productServices;
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Filters(GridRequestModel objGrid)
        {
            Core.IPagedList<OrderCart> TypesList = await _orderCartService.GetAllAsync(objGrid);
            return Ok(TypesList.ToGridResponse(objGrid, "Customer List"));
        }


        [HttpGet("{id}")]
        public async Task<ApiResponse> Get(int id)
        {
            if (id == 0)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, Message.NoDataFound);
            }

            OrderCart data = await _orderCartService.GetByIdAsync(id);
            return data.ToSingleResponse<OrderCart, OrderCartModel>("Customer");
        }

        [HttpPost]

        public async Task<ApiResponse> Post([FromForm] OrderCart model)
        {
            if (model.Id == 0)
            {

                await _orderCartService.InsertAsync(model.MapTo<OrderCart>());
            }
            else if (model.Id != 0)
            {
                OrderCart data = await _orderCartService.GetByIdAsync(model.Id);
                data.OrderId = model.OrderId;
                data.ProductName = model.ProductName;

                data.Quantity = model.Quantity;
                data.UnitPrice = model.UnitPrice;
                data.TotalPrice = model.TotalPrice;
                await _orderCartService.UpdateAsync(data);
                await _orderCartService.UpdateAsync(model.MapTo<OrderCart>());

            }
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, model.Id == 0 ? "Product Added" : "Product Updated", model);

        }
        [HttpDelete]

        public async Task<ApiResponse> Delete(IList<int> Ids)
        {
            if (Ids.Count == 0)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status422UnprocessableEntity, Message.InvalidRequestParmeters);
            }

            IList<OrderCart> obj = await _orderCartService.GetByIdsAsync(Ids).ConfigureAwait(false);
            if (obj == null)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status422UnprocessableEntity, Message.NoDataFound);
            }

            await _orderCartService.DeleteAsync(obj);
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, "Product Delete");

        }
        //[HttpPost]
        //[Route("[action]")]
        //public async Task<ApiResponse> AddToCart([FromBody] List<int> productIds)
        //{

        //    try
        //    {
        //        await _orderCartService.AddToCartAsync(productIds);
        //        return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, "Products added to the order cart");
        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, Message.NoDataFound);
        //    }
        //}

    }
}
