using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeDAA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public ClaimsPrincipal CurrentUser => HttpContext.User;

        public int CurrentUserId
        {
            get
            {
                if (CurrentUser != null && CurrentUser.HasClaim(c => c.Type == "Id"))
                {
                    return int.Parse(EncryptionUtility.DecryptText(CurrentUser.Claims.FirstOrDefault(c => c.Type == "Id").Value, SecurityHelper.EnDeKey));
                }
                else
                {
                    return 0;
                }
            }
        }

        public int CurrentRoleId
        {
            get
            {
                if (CurrentUser != null && CurrentUser.HasClaim(c => c.Type == "RoleId"))
                {
                    return int.Parse(EncryptionUtility.DecryptText(CurrentUser.Claims.FirstOrDefault(c => c.Type == "RoleId").Value, SecurityHelper.EnDeKey));
                }
                else
                {
                    return 0;
                }
            }
        }
        public RoleTypes CurrentRoleType
        {
            get
            {
                if (CurrentUser != null && CurrentUser.HasClaim(c => c.Type == "RoleType"))
                {
                    return (RoleTypes)Enum.Parse(typeof(RoleTypes), EncryptionUtility.DecryptText(CurrentUser.Claims.FirstOrDefault(c => c.Type == "RoleType").Value, SecurityHelper.EnDeKey));
                }
                else
                {
                    return RoleTypes.None;
                }
            }
        }
        public string CurrentUserName
        {
            get
            {
                if (CurrentUser != null && CurrentUser.HasClaim(c => c.Type == "FullName"))
                {
                    return EncryptionUtility.DecryptText(CurrentUser.Claims.FirstOrDefault(c => c.Type == "FullName").Value, SecurityHelper.EnDeKey);
                }
                else
                {
                    return "";
                }
            }
        }
        public string CurrentUserFullName
        {
            get
            {
                if (CurrentUser != null && CurrentUser.HasClaim(c => c.Type == "FullName"))
                {
                    return EncryptionUtility.DecryptText(CurrentUser.Claims.FirstOrDefault(c => c.Type == "FullName").Value, SecurityHelper.EnDeKey);
                }
                else
                {
                    return "";
                }
            }
        }
        public string CurrentRoleName
        {
            get
            {
                if (CurrentUser != null && CurrentUser.HasClaim(c => c.Type == "RoleName"))
                {
                    return EncryptionUtility.DecryptText(CurrentUser.Claims.FirstOrDefault(c => c.Type == "RoleName").Value, SecurityHelper.EnDeKey);
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
