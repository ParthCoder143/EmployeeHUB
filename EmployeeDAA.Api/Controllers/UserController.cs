using EmployeeDAA.Api.Extensions;
using EmployeeDAA.Core.Domain.Grid;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Permissions;
using EmployeeDAA.Api.Models;
using EmployeeDAA.Api.InfraStructure;
using EmployeeDAA.Api.Models.User;
using EmployeeDAA.Core.Infrastructure;

namespace EmployeeDAA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        public UserController(IConfiguration configuration, IUserService userService, IRoleService roleService)
        {
            _configuration = configuration;
            _userService = userService;
            _roleService = roleService;
        }

        [HttpPost]
        [Route("[action]")]
        [Permission(Page = PageName.AdmMasUsers, Permission = PagePermission.View)]

        public async Task<IActionResult> Filters(GridRequestModel objGrid)
        {
            Core.IPagedList<User> UserList = await _userService.GetAllAsync(objGrid);
            return Ok(UserList.ToGridResponse(objGrid, "Users List"));
        }

        [HttpGet("{id?}")]
        [Permission(Page = PageName.AdmMasUsers, Permission = PagePermission.View)]

        public async Task<ApiResponse> Get(int id)
        {
            if (id == 0)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, Message.NoDataFound);
            }

            User data = await _userService.GetByIdAsync(id);
            return data.ToSingleResponse<User, UserModel>("Users");
        }

        [HttpPost]
        public async Task<ApiResponse> Post([FromForm] UserModel model)
        {
            if (!CommonExtensions.CheckPermission(PageName.AdmMasUsers, model.Id == 0 ? PagePermission.Add : PagePermission.Edit))
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status401Unauthorized, "You don't have permission to access this feature.");
            }

            int Result = await _userService.CheckUserNameDuplication(model.UserName, model.Email, model.Id);
            if (Result == 1)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, "InsExistUserName");
            }
            //else if (Result == 2)
            //{
            //    return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, Message.GetMessage("InsExistUserEmail"));
            //}

            #region  :: Image Upload ::
            if (model.Imgupload != null)
            {
                string path = Path.Combine(_configuration["FileUploadSetting:UserProfileImgUploadBasePath"]);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                model.Photo = System.Guid.NewGuid().ToString() + Path.GetExtension(model.Imgupload.FileName);
                FileStream stream = new($"{path}/{model.Photo}", FileMode.Create);
                await model.Imgupload.CopyToAsync(stream);
                stream.Dispose();
            }
            if (model.Id == 0)
            {
                await _userService.InsertAsync(model.MapTo<User>(), CurrentUserId, CurrentUserName);
            }
            #endregion
            //if (model.Id == 0)
            //{
            //    if (model.IsAdLogin == 0)
            //    {
            //        model.Password = EncryptionUtility.EncryptText(model.Password, SecurityHelper.OrderEncryptionKey);
            //    }

            //    await _userService.InsertAsync(model.MapTo<User>(), CurrentUserId, CurrentUserName);
            //}
            else if (model.Id != 0)
            {
                User data = await _userService.GetByIdAsync(model.Id);
                #region :: Update Details ::
                //if (model.IsAdLogin == 0)
                //{
                //    data.Password = model.Password == null ? data.Password : EncryptionUtility.EncryptText(model.Password, SecurityHelper.OrderEncryptionKey);
                //}

                data.FirstName = model.FirstName;
                data.LastName = model.LastName;
                data.UserName = model.UserName;
                data.RoleId = model.RoleId;
                data.Email = model.Email;
                data.Mobile = model.Mobile;
                data.Photo = model.Photo;
                data.CameraUrl = model.CameraUrl;

                #endregion
                await _userService.UpdateAsync(data, CurrentUserId, CurrentUserName);
            }
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, model.Id == 0 ? "User Added" : "User Updated", model);
        }

        [HttpPost]
        [Route("[action]")]
        [Permission(Page = PageName.AdmMasUsers, Permission = PagePermission.Edit)]

        public async Task<ApiResponse> UpdateStatus([FromBody] int id)
        {
            if (id == 0)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, Message.NoDataFound);
            }

            User UpdateObj = await _userService.GetByIdAsync(id);
            UpdateObj.IsActive = !UpdateObj.IsActive;
            await _userService.UpdateAsync(UpdateObj, CurrentUserId, CurrentUserName);
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, "UserStatusUpdate");
        }

        [HttpDelete]
        [Permission(Page = PageName.AdmMasUsers, Permission = PagePermission.Delete)]

        public async Task<ApiResponse> Delete(IList<int> Ids)
        {
            if (Ids.Count == 0)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status422UnprocessableEntity, Message.InvalidRequestParmeters);
            }

            IList<User> obj = await _userService.GetByIdsAsync(Ids).ConfigureAwait(false);
            if (obj == null)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status422UnprocessableEntity, Message.NoDataFound);
            }

            await _userService.DeleteAsync(obj, CurrentUserId, CurrentUserName);
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, "UserDelete");

        }

        [HttpGet]
        [Route("[action]")]
        [Permission]

        public async Task<FileResult> GetUserProfileDocument(string filename)
        {
            MemoryStream mst = new();
            string folderpath = Path.Combine(_configuration["FileUploadSetting:UserProfileImgUploadBasePath"], filename);
            if (System.IO.File.Exists(folderpath))
            {
                byte[] imgByte = await System.IO.File.ReadAllBytesAsync(folderpath);
                return new FileStreamResult(new MemoryStream(imgByte), "image/jpeg");  // You can use your own method over here.         
            }
            return new FileStreamResult(mst, "image/jpeg");
        }

        [HttpPost]
        [Route("[action]")]
        [Permission]

        public async Task<ApiResponse> ChangePassword(ChangePasswordModel changePasswordmodel)
        {
            User PasswordUpdateObj = await _userService.GetByIdAsync(CurrentUserId);
            changePasswordmodel.Oldpassword = EncryptionUtility.EncryptText(changePasswordmodel.Oldpassword, SecurityHelper.OrderEncryptionKey);
            if (PasswordUpdateObj.Password != changePasswordmodel.Oldpassword)
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, "Invalid Old Password.");
            }
            PasswordUpdateObj.Password = EncryptionUtility.EncryptText(changePasswordmodel.Confirmpassword, SecurityHelper.OrderEncryptionKey);
            await _userService.UpdateAsync(PasswordUpdateObj, CurrentUserId, CurrentUserName);
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, "Password Updated");
        }

        [HttpPost]
        [Route("[action]")]
        [Permission]

        public async Task<ApiResponse> UpdateProfile(UserProfileModel userProfileModel)
        {
            User userProfileObj = await _userService.GetByIdAsync(CurrentUserId);
            userProfileObj.FirstName = userProfileModel.FirstName;
            userProfileObj.LastName = userProfileModel.LastName;
            userProfileObj.UserName = userProfileModel.UserName;
            userProfileObj.Email = userProfileModel.Email;
            userProfileObj.Mobile = userProfileModel.Mobile;
            await _userService.UpdateAsync(userProfileObj, CurrentUserId, CurrentUserName);
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, "Profile Updated");
        }

        [HttpGet]
        [Route("[action]")]
        [Permission]

        public async Task<ApiResponse> GetUserProfile()
        {
            User userProfileObj = await _userService.GetByIdAsync(CurrentUserId);
            return userProfileObj.ToSingleResponse("Users");
        }

        #region :: Create New User with Third Party ::
        [HttpPost]
        [Route("[action]")]
        public async Task<ApiResponse> AddUpdateUser([FromForm] CreateUserModel model)
        {
            string[] result = EncryptionUtility.DecryptText(model.UserTokenKey, SecurityHelper.UserEnKey)?.Split("|") ?? System.Array.Empty<string>();
            if (result.Length == 3)
            {
                //USerName|Email|Password
                if (model.UserName == result[0] && model.Email == result[1] && model.Password == result[2])
                {
                    Role roles = await _roleService.GetRoleByRoleType(model.RoleId);
                    if (roles.Id > 0)
                    {
                        User userProfileObj = await _userService.GetUserByUserName(model.UserName);
                        if (userProfileObj.Id == 0)
                        {
                            User user = model.MapTo<User>();
                            user.RoleId = roles.Id;
                            await _userService.InsertAsync(user, CurrentUserId, CurrentUserName);
                        }
                        else if (userProfileObj.Id != 0)
                        {
                            #region :: Update Details ::
                            userProfileObj.FirstName = model.FirstName;
                            userProfileObj.LastName = model.LastName;
                            userProfileObj.UserName = model.UserName;
                            userProfileObj.RoleId = roles.Id;
                            userProfileObj.Email = model.Email;
                            userProfileObj.Mobile = model.Mobile;
                            #endregion
                            await _userService.UpdateAsync(userProfileObj, 0, "");
                        }
                        return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, Message.GetMessage("UserSave"));
                    }
                }
            }
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, Message.InvalidRequestParmeters); // await AddUser(model.MapTo<UserModel>());            
        }
        #endregion
    }
}

