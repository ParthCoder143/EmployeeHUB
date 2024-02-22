using EmployeeDAA.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security;
using System.Collections.Generic;
using EmployeeDAA.Api.Models.Employee;
using EmployeeDAA.Api.InfraStructure;
using EmployeeDAA.Api.Extensions;
using System.Security.Permissions;
using static LinqToDB.Common.Configuration;
using Azure;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Services.Employee;
using EmployeeDAA.Core.Domain.Grid;
using EmployeeDAA.Core.Infrastructure;
using Azure.Core;

namespace EmployeeDAA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : BaseController
    {
        private readonly IEmployeeServices _employeeServices;

        public EmployeesController(IEmployeeServices employeeServices)
        {
            _employeeServices = employeeServices;
        }
        [HttpPost]
        [Route("[action]")]

        [Permission(Page = PageName.AdmMasEmployee, Permission = PagePermission.View)]
        public async Task<IActionResult> Filters(GridRequestModel objGrid)
        {
            Core.IPagedList<Employees> DocList = await _employeeServices.GetAllAsync(objGrid);
            return Ok(DocList.ToGridResponse(objGrid, "Employees List"));
        }
        [HttpGet("{id}")]
        [Permission(Page = PageName.AdmMasEmployee, Permission = PagePermission.View)]

        public async Task<ApiResponse> Get(int id)
        {

            if (id == 0)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, Message.NoDataFound);
            }

            Employees data = await _employeeServices.GetByIdAsync(id);
            return data.ToSingleResponse<Employees, EmployeeModel>("Employees");
        }
        [HttpPost]
        public async Task<ApiResponse> Post(EmployeeModel obj)
        {
            if (!CommonExtensions.CheckPermission(PageName.AdmMasEmployee, obj.Id == 0 ? PagePermission.Add : PagePermission.Edit))
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status401Unauthorized, "You don't have permission to access this feature.");
            }

            
            if (obj.Id == 0)
            {
                await _employeeServices.InsertAsync(obj.MapTo<Employees>());
            }
            else if (obj.Id != 0)
            {
                await _employeeServices.UpdateAsync(obj.MapTo<Employees>());
            }
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, obj.Id == 0 ? "Employee Added": "Employee Updated", obj);

        }

        [HttpDelete]
        [Permission(Page = PageName.AdmMasEmployee, Permission = PagePermission.Delete)]

        public async Task<ApiResponse> Delete(IList<int> Ids)
        {
            if (Ids.Count == 0)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status422UnprocessableEntity, Message.InvalidRequestParmeters);
            }

            IList<Employees> obj = await _employeeServices.GetByIdAsync(Ids).ConfigureAwait(false);
            if (obj != null)
            {
                await _employeeServices.DeleteAsync(obj, CurrentUserId, CurrentUserName);
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, Message.GetMessage("Employee Delete"));
            }
            else
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status422UnprocessableEntity, "Invalid Id");
            }
            //if (obj == null)
            //{
            //    return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status422UnprocessableEntity, Message.NoDataFound);
            //}

            //await _employeeServices.DeleteAsync(obj, CurrentUserId, CurrentUserName));
            //return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, "Employee Delete");


        }

    }
}
