using EmployeeDAA.Api.Models;
using EmployeeDAA.Core.Infrastructure;
using EmployeeDAA.Services.Users;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;

namespace EmployeeDAA.Api.Extensions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PermissionAttribute : Attribute, IAuthorizationFilter
    {

        public PageName Page { get; set; }
        public PagePermission Permission { get; set; }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isAuthorised = false;
            bool isPermission = true;

            IPrincipal user = context.HttpContext.User;
            UserService IUserMasterRepository = context.HttpContext.RequestServices.GetService(typeof(IUserService)) as UserService;
            if ((user.Identity as ClaimsIdentity).Claims.Any())
            {
                int result = Task.Run(() => IUserMasterRepository.GetUsersByUserToken(GetTokenValue(user, "UserToken"), GetTokenValue(user, "Id"))).Result;
                if (result != 0 && user.Identity.IsAuthenticated)
                {
                    isAuthorised = true;
                    if (DateTime.Compare(CommonExtensions.FromUnixTime(long.Parse((user.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == "exp").Value)), DateTime.UtcNow) < 0)
                    {
                        isAuthorised = false;
                    }
                    else if (Page != PageName.NoPage)
                    {
                        isPermission = CommonExtensions.CheckPermission(Page, Permission);
                    }
                }
            }
            if (!isAuthorised)
            {
                string actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;
                if (actionName.ToUpper() != "REFRESHTOKEN")
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Not Authorized";
                    context.Result = new JsonResult("NotAuthorized")
                    {
                        Value = new
                        {
                            Status = ApiStatusCode.Status401Unauthorized,
                            Message = "Authentication Failed! Invalid Token."
                        },
                    };
                }
            }
            else if (!isPermission)
            {
                string actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;
                if (actionName.ToUpper() != "LOGOUT")
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Bad Request";
                    context.Result = new JsonResult("BadRequest")
                    {
                        Value = new
                        {
                            Status = ApiStatusCode.Status400BadRequest,
                            Message = "You don't have permission to access this feature."
                        },
                    };
                }
            }
        }
        public string GetTokenValue(IPrincipal user, string climType)
        {
            if (!string.IsNullOrEmpty((user.Identity as ClaimsIdentity).Claims?.FirstOrDefault(c => c.Type == climType)?.Value ?? ""))
                return EncryptionUtility.DecryptText((user.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == climType).Value, SecurityHelper.EnDeKey);
            else
                return "";
        }
    }
}
