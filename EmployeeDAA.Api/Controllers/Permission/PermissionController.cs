using EmployeeDAA.Api.Extensions;
using EmployeeDAA.Api.InfraStructure;
using EmployeeDAA.Api.Models;
using EmployeeDAA.Core.Infrastructure;
using EmployeeDAA.Services.PermissionService;
using Microsoft.AspNetCore.Mvc;
using System.Security.Permissions;

namespace EmployeeDAA.Api.Controllers.Permission
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : BaseController
    {
        #region Fields
        private readonly IPermissionService _IPermissionService;

        #endregion

        #region Ctor

        public PermissionController(IPermissionService iPermissionService)
        {
            _IPermissionService = iPermissionService;
        }

        #endregion

        [HttpGet]
        [Route("[action]")]
        //[Permission(Page = PageName.AdmRolePermission, Permission = PagePermission.View)]
        public async Task<ApiResponse> GetPermissions(int roleid)
        {
            if (roleid == 0)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, Message.NoDataFound);
            }

            List<Core.Permissions.Page> PermissionList = await _IPermissionService.GetAllModules(roleid).ConfigureAwait(false);

            return ApiResponseHelper.GenerateResponse(
                ApiStatusCode.Status200OK,
                "Permission List.",
                 new { PermissionList }
                );
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ApiResponse> SavePermission(List<EmployeeDAA.Core.Permissions.Permission> data)
        {
            // if(!CommonExtensions.CheckPermission(PageName.AdmRolePermission, data.Where(x => x.Id != 0).Any() ? PagePermission.Edit :  PagePermission.Add))
            //     return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status401Unauthorized, "You don't have permission to access this feature.");
            await _IPermissionService.InsertAsync(data, CurrentUserId, CurrentUserName);
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, ("SavePermission"));
        }
    }
}
