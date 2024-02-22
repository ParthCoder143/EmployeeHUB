using EmployeeDAA.Api.Extensions;
using EmployeeDAA.Api.Models.Category;
using EmployeeDAA.Api.Models;
using EmployeeDAA.Core.Domain.Grid;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Services.Employee;
using Microsoft.AspNetCore.Mvc;
using EmployeeDAA.Services.Category;
using EmployeeDAA.Api.InfraStructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Permissions;
using EmployeeDAA.Core.Infrastructure;
using EmployeeDAA.Services.Products;

namespace EmployeeDAA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryServices _categoryServices;
        private readonly IProductServices _productServices;


        public CategoriesController(ICategoryServices categoryServices, IProductServices productServices)
        {
            _categoryServices = categoryServices;
            _productServices = productServices;
        }
        [HttpPost]
        [Route("[action]")]
        [Permission(Page = PageName.AdmMasCategory, Permission = PagePermission.View)]

        public async Task<IActionResult> Filters(GridRequestModel objGrid)
        {
            Core.IPagedList<Categories> DocList = await _categoryServices.GetAllAsync(objGrid);
            return Ok(DocList.ToGridResponse(objGrid, "Categories List"));
        }
        [HttpGet("{id}")]
        [Permission(Page = PageName.AdmMasCategory, Permission = PagePermission.View)]

        public async Task<ApiResponse> Get(int id)
        {
            if (id == 0)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, Message.NoDataFound);
            }

            Categories data = await _categoryServices.GetByIdAsync(id);
            return data.ToSingleResponse<Categories, CategoryModel>("Categories");
        }
        [HttpPost]
        public async Task<ApiResponse> Post(CategoryModel obj)
        {
            if (obj.Id == 0)
            {
                await _categoryServices.InsertAsync(obj.MapTo<Categories>());
            }
            else if (obj.Id != 0)
            {
                await _categoryServices.UpdateAsync(obj.MapTo<Categories>());
            }
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, obj.Id == 0 ? "Category Added" : "Category Updated", obj);

        }

        [HttpDelete]
        [Permission(Page = PageName.AdmMasCategory, Permission = PagePermission.Delete)]

        public async Task<ApiResponse> Delete(IList<int> Ids)
        {
            if (Ids.Count == 0)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status422UnprocessableEntity, Message.InvalidRequestParmeters);
            }

            IList<Categories> obj = await _categoryServices.GetByIdAsync(Ids).ConfigureAwait(false);
            if (obj == null)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status422UnprocessableEntity, Message.NoDataFound);
            }

            await _categoryServices.DeleteAsync(obj);
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, "Category Delete");
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ApiResponse> UpdateStatus([FromBody] int id)
        {
            if (id > 0)
            {
                Categories UpdateObj = await _categoryServices.GetByIdAsync(id);
                UpdateObj.IsActive = !UpdateObj.IsActive;
                await _categoryServices.UpdateAsync(UpdateObj);
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, Message.GetMessage("CommUpdate"));
            }
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, "No data found.");
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAll(string productId)
        {
            try
            {
                var categories = await _categoryServices.GetAllCategoryAsync(productId);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
