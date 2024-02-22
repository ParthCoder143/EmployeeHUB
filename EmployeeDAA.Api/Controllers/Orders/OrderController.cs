using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.EMMA;
using EmployeeDAA.Api.Extensions;
using EmployeeDAA.Api.InfraStructure;
using EmployeeDAA.Api.Models;
using EmployeeDAA.Api.Models.Order;
using EmployeeDAA.Api.Models.Product;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core.Domain.Grid;
using EmployeeDAA.Core.Domain.Orders;
using EmployeeDAA.Core.Infrastructure;
using EmployeeDAA.Services.Order;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDAA.Api.Controllers.Orders
{
    [Route("api/[controller]")]
    [ApiController]

    public class OrderController : BaseController
    {
      private readonly IOrderServices _orderServices;

        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Filters(GridRequestModel objGrid)
        {
            Core.IPagedList<OrderInfo> TypesList = await _orderServices.GetAllAsync(objGrid);
            return Ok(TypesList.ToGridResponse(objGrid, "Customer List"));
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse> Get(int id)
        {
            if (id == 0)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, Message.NoDataFound);
            }

            OrderInfo data = await _orderServices.GetByIdAsync(id);
            return data.ToSingleResponse<OrderInfo, OrderInfoModel>("Customer");
        }
        [HttpPost]
        public async Task<ApiResponse> Post([FromForm] OrderInfoModel order)
        {
            if (order == null)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, Message.NoDataFound);
            }
            if (order.Id == 0)
            {
                await _orderServices.InsertAsync(order.MapTo<OrderInfo>());
            }
            else if (order.Id != 0)
            {
                OrderInfo data = await _orderServices.GetByIdAsync(order.Id);
                data.CustomerId = order.CustomerId;
                data.CustomerName = order.CustomerName;
                data.DateOfBirth = order.DateOfBirth;
                data.MobileNo = order.MobileNo;
                data.EmailAddress = order.EmailAddress;
                data.UnitNo = order.UnitNo;
                data.Block = order.Block;
                data.Street = order.Street;
                data.BuildingName = order.BuildingName;
                data.IsDeleted = order.IsDeleted;
                data.Country = order.Country;
                data.PostalCode = order.PostalCode;
                await _orderServices.UpdateAsync(data);
                await _orderServices.UpdateAsync(order.MapTo<OrderInfo>());
            }
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, order.Id == 0 ? "Customer Added" : "Customer Updated", order);

        }

        [HttpDelete]
        public async Task<ApiResponse> Delete(IList<int> Ids)
        {
            if (Ids.Count == 0)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status422UnprocessableEntity, Message.InvalidRequestParmeters);
            }

            IList<OrderInfo> obj = await _orderServices.GetByIdsAsync(Ids).ConfigureAwait(false);
            if (obj == null)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status422UnprocessableEntity, Message.NoDataFound);
            }

            await _orderServices.DeleteAsync(obj);
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, "Customer Delete");

        }
    }
}

