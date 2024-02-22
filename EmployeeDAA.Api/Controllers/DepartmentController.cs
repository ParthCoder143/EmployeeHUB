using EmployeeDAA.Api.Extensions;
using EmployeeDAA.Api.InfraStructure;
using EmployeeDAA.Api.Models.Employee;
using EmployeeDAA.Api.Models;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core.Domain.Department;
using EmployeeDAA.Core.Domain.Grid;
using EmployeeDAA.Core.Infrastructure;
using EmployeeDAA.Services;
using Microsoft.AspNetCore.Mvc;
using EmployeeDAA.Api.Models.Department;
using EmployeeDAA.Api.Models.Product;

namespace EmployeeDAA.Api.Controllers
{
    public class DepartmentController : BaseController
    {
        private readonly DepartmentServices _departmentServices;

        public DepartmentController(DepartmentServices departmentServices)
        {
            _departmentServices = departmentServices;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Filters(GridRequestModel objgrid)
        {
            Core.IPagedList<Department> DepartmentList = await _departmentServices.GetAllAsync(objgrid);
            return Ok(DepartmentList.ToGridResponse(objgrid, "Department List"));
        }

        [HttpGet("{id}")]

        public async Task<ApiResponse> Get(int id)
        {
            if (id == 0)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, Message.NoDataFound);
            }

            Department data = await _departmentServices.GetByIdAsync(id);
            return data.ToSingleResponse<Department, DepartmentModel>("Department");
        }

        [HttpPost]
        public async Task<ApiResponse> Post(DepartmentModel obj)
        {
            //if (!CommonExtensions.CheckPermission(PageName.AdmMasEmployee, obj.Id == 0 ? PagePermission.Add : PagePermission.Edit))
            //{
            //    return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status401Unauthorized, "You don't have permission to access this feature.");
            //}

            if (obj.Id == 0)
            {
                await _departmentServices.InsertAsync(obj.MapTo<Department>());
            }
            else if (obj.Id != 0)
            {
                await _departmentServices.UpdateAsync(obj.MapTo<Department>());
            }
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, obj.Id == 0 ? "Department Added" : "Department Updated", obj);


        }

        [HttpDelete]
        public async Task<ApiResponse> Delete(IList<int> ids)
        {
            //if (Ids.Count == 0)
            //{
            //    return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status422UnprocessableEntity, Message.InvalidRequestParmeters);
            //}

            IList<Department> obj = await _departmentServices.GetByIdAsync(ids).ConfigureAwait(false);
            if (obj != null)
            {
                await _departmentServices.DeleteAsync(obj);
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, "Department Delete");
            }
            else
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status422UnprocessableEntity, "Invalid Id");
            }

        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ApiResponse> UpdateStatus([FromBody]int id)
        {
            if (id > 0)
            {
                Department obj = await _departmentServices.GetByIdAsync(id);
                obj.IsActive = !obj.IsActive;
                await _departmentServices.UpdateAsync(obj);
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, "Status Updated");
            }
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, "No Data Found");

        }
    }
}
