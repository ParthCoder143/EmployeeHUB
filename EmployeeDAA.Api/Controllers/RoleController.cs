using EmployeeDAA.Api.Extensions;
using EmployeeDAA.Api.Models.User;
using EmployeeDAA.Api.Models;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core.Domain.Grid;
using EmployeeDAA.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Permissions;
using EmployeeDAA.Api.InfraStructure;
using EmployeeDAA.Core.Infrastructure;

namespace EmployeeDAA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : BaseController
    {
        #region Fields

        private readonly IRoleService _roleService;

        #endregion
        #region Ctor

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        #endregion
        [HttpPost]
        [Route("[action]")]
        [Permission(Page = PageName.AdmMasRoles, Permission = PagePermission.View)]

        public async Task<IActionResult> Filters(GridRequestModel objGrid)
        {
            Core.IPagedList<Role> RoleList = await _roleService.GetAllAsync(objGrid);
            return Ok(RoleList.ToGridResponse(objGrid, "Role List"));
        }
        [HttpGet("{id?}")]
        [Permission(Page = PageName.AdmMasRoles, Permission = PagePermission.View)]

        public async Task<ApiResponse> Get(int id)
        {
            if (id == 0)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, Message.NoDataFound);
            }

            Role data = await _roleService.GetByIdAsync(id);
            return data.ToSingleResponse<Role, RoleModel>("Role");
        }

        [HttpPost]
        public async Task<ApiResponse> Post(RoleModel model)
        {

            if (await _roleService.IsNameExist(model.Name, model.Id))
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, "Role Exist");
            }

            model.RoleType = model.RoleType > 0 ? model.RoleType : null;
            if (model.Id == 0)
            {
                await _roleService.InsertAsync(model.MapTo<Role>(), CurrentUserId, CurrentUserName);
            }
            else
            {
                await _roleService.UpdateAsync(model.MapTo<Role>(), CurrentUserId, CurrentUserName);
            }

            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK,"Role Save");
        }

        [HttpPost]
        [Route("[action]")]
        [Permission(Page = PageName.AdmMasRoles, Permission = PagePermission.Edit)]

        public async Task<ApiResponse> DeleteStatus([FromBody] int id)
        {
            if (id == 0)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, Message.NoDataFound);
            }

            Role UpdateObj = await _roleService.GetByIdAsync(id);
            UpdateObj.IsActive = !UpdateObj.IsActive;
            await _roleService.UpdateAsync(UpdateObj, CurrentUserId, CurrentUserName);
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK,"Role Delete");
        }

        [HttpDelete]
        [Permission(Page = PageName.AdmMasRoles, Permission = PagePermission.Delete)]

        public async Task<ApiResponse> Delete(IList<int> Ids)
        {
            if (Ids.Count == 0)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status422UnprocessableEntity, Message.InvalidRequestParmeters);
            }

            IList<Role> obj = await _roleService.GetByIdsAsync(Ids).ConfigureAwait(false);
            if (obj == null)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status422UnprocessableEntity, Message.NoDataFound);
            }

            obj.ToList().ForEach(s => s.IsActive = false);
            await _roleService.UpdateAsync(obj, CurrentUserId, CurrentUserName);
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, "Role Delete");

        }
    }
}
