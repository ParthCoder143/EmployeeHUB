using EmployeeDAA.Api.Extensions;
using EmployeeDAA.Api.InfraStructure;
using EmployeeDAA.Api.Models;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Services.Category;
using EmployeeDAA.Services.Products;
using EmployeeDAA.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Permissions;
using static EmployeeDAA.Core.Permissions.Page;

namespace EmployeeDAA.Api.Controllers.DropDownConfig
{

    [Route("api/[controller]")]
    [ApiController]

    public class DropDownController : Controller
    {
        private readonly ICategoryServices _categoryServices;
        private readonly IProductServices _productServices;
        private readonly IUserService _docUserService;
        private readonly IRoleService _docRoleService;



        public DropDownController(ICategoryServices categoryServices, IProductServices productServices, IUserService docUserService, IRoleService docRoleService)
        {
            _categoryServices = categoryServices;
            _productServices = productServices;
            _docUserService = docUserService;
            _docRoleService = docRoleService;
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<ApiResponse> GetBindDropDown(string Mode)
        {
            IList<SelectListItem> Result = await GetList(Mode);
            return ApiResponseHelper.GenerateResponse(
                Result.Count > 0 ? ApiStatusCode.Status200OK : ApiStatusCode.Status404NotFound, "Get Data Successfully.", Result);

        }
        [NonAction]
        private async Task<IList<SelectListItem>> GetList(string Mode)
        {
            return Mode switch
            {
               
                "Products" => (await _productServices.GetAllAsync()).ToDropDown2(),
                "Categories" => (await _categoryServices.GetAllCategoryAsync()).ToDropDown("Id", "CategoryName"),
                "User" => (await _docUserService.GetAllAsync()).ToDropDown("Id", "UserFullName"),
                "Role" => (await _docRoleService.GetAllAsync()).ToDropDown(),
                "RoleType" => Common.DropDownBindWithEnum(typeof(RoleTypes)),
                "RoleWithType" => Common.DropDownBindWithEnum(typeof(RoleTypes)).Where(x => x.Value != "-1").ToList(),
                "RoleTypeUser" => (await _docUserService.GetRoleTypesUsersAllAsync(RoleTypes.Administrator)).ToDropDown("Id", "UserFullName"),
                "EntityNameForAudit" => Common.DropDownBindWithSameTextValue(typeof(EntityNameForAudit)),



            };
        }
        //public IList<T> ToIList<T>(List<T> t)
        //{
        //    return t;
        //}
    }
}
