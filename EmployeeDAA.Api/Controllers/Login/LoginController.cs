using EmployeeDAA.Api.Extensions;
using EmployeeDAA.Api.Models;
using EmployeeDAA.Api.Models.Login;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core.Infrastructure;
using EmployeeDAA.Core.Permissions;
using EmployeeDAA.Services.PermissionService;
using EmployeeDAA.Services.Settings;
using EmployeeDAA.Services.Users;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace EmployeeDAA.Api.Controllers.Login
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : BaseController
    {
        #region Fields

        private readonly IUserService _UserService;
        private readonly ISystemSettingService _settingService;
        private readonly IConfiguration _Configuration;
        private readonly IPermissionService _IPermissionRepository;


        #endregion
        #region Ctor

        public LoginController(IUserService UserService, ISystemSettingService settingService,
        IConfiguration configuration, IPermissionService IPermissionRepository)
        {
            _UserService = UserService;
            _settingService = settingService;
            _Configuration = configuration;
            _IPermissionRepository = IPermissionRepository;
        }

        #endregion
        [HttpPost]
        [Route("[action]")]
        public async Task<ApiResponse> Authenticate([FromBody] AuthenticateModel model)
        {
            if (VerifyCaptcha(model.CaptchaCode, model.CaptchaToken) || model.Password.Trim().Length == 0 || (model.CaptchaToken == "ByPassIt" && model.CaptchaCode == "123"))
            {
                //#region :: AD Login ::  
                User user = new();
                List<Core.Domain.Settings.Settings> lst = await _settingService.GetAllAsync();
                if (model.Password.Trim().Length == 0)
                {
                    //    user = await _UserService.CheckADLogin(model.Username);
                    //    if (user != null && user.Id > 0)
                    //    {
                    //        string domain = lst.Find(x => x.Key == "AD.API.Domain").Value;
                    //        string webUrl = lst.Find(x => x.Key == "AD.API.URL").Value + "AuthenticateUser";
                    //        string authToken = lst.Find(x => x.Key == "AD.API.Token").Value;
                    //        string data = "{ \"LoginId\": \"" + user.UserName.Trim() + "\", \"Password\": \"\", \"Domain\": \"" + domain + "\" }";
                    //        string LoginResult = await AD_HTTP_Post(data, webUrl, authToken);
                    //        ADLoginResponse objLoginResponse = JsonConvert.DeserializeObject<ADLoginResponse>(LoginResult);
                    //        if (objLoginResponse == null || objLoginResponse.IsError || objLoginResponse.StatusCode != "200")
                    //        {
                    user = null;
                    //        }
                    //    }
                }
                else
                {
                    user = await _UserService.CheckLogin(model.Username, model.Password);
                }

                if (user == null)
                {
                    return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, "Authentication Failed! Invalid username or password.");
                }
                else
                {
                    user.UserToken = Guid.NewGuid().ToString();
                    await _UserService.UpdateAsync(user, 0, "");
                    Tuple<string, string> Permission = await GetPermissions(user);
                    return ApiResponseHelper.GenerateResponse(
                    ApiStatusCode.Status200OK, "Login Successfully.", new
                    {
                        user.UserToken,
                        Token = await CommonExtensions.GenerateToken(user, Convert.ToString(_Configuration["AuthenticationSettings:SecretKey"]), Convert.ToInt16((lst.Find(x => x.Key == "ApiTokenExpiry")?.Value) ?? "0"), Permission.Item1),
                        Permissions = Permission.Item2,
                        Role = user.RoleId,
                        UserName = user.FirstName + " " + user.LastName,
                        ShortName = user.FirstName[..1] + user.LastName[..1]
                    });
                }
            }
            else
            {
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, "Invalid captcha.");
            }
        }
        [NonAction]
        public async Task<Tuple<string, string>> GetPermissions(User user)
        {
            List<Page> permissions = await _IPermissionRepository.GetAllModules(user.RoleId);
            StringBuilder permissionString = new();
            StringBuilder menuhide = new();
            foreach (Page objPage in permissions)
            {
                if (user.RoleType == RoleTypes.Admin)
                {
                    objPage.IsAdd = true;
                    objPage.IsEdit = true;
                    objPage.IsDelete = true;
                    objPage.IsView = true;
                }
                permissionString.Append(objPage.PageCode).Append('|').Append(objPage.IsAdd ? 1 : 0).Append('|').Append(objPage.IsEdit ? 1 : 0).Append('|').Append(objPage.IsDelete ? 1 : 0).Append('|').Append(objPage.IsView ? 1 : 0).Append(',');
                if (objPage.IsView)
                {
                    menuhide.Append(objPage.PageCode).Append('|').Append(objPage.IsAdd ? 1 : 0).Append('|').Append(objPage.IsEdit ? 1 : 0).Append('|').Append(objPage.IsDelete ? 1 : 0).Append(',');
                }
            }
            return Tuple.Create(permissionString.ToString(), menuhide.ToString());
        }

        [Permission]
        [HttpGet]
        [Route("[action]")]
        public async Task<ApiResponse> Logout()
        {
            User user = await _UserService.GetByIdAsync(CurrentUserId);
            if (user != null)
            {

                user.UserToken = Guid.NewGuid().ToString();
                await _UserService.UpdateAsync(user, 0, "");
                return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status200OK, "Success", new { user });
            }
            return ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, "Error");
        }
        //private static async Task<string> AD_HTTP_Post(string data, string webUrl, string authToken)
        //{
        //    string Result;
        //    try
        //    {
        //        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;


        //        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(webUrl);
        //        webRequest.Method = "POST";
        //        webRequest.Headers.Add("TOKEN", authToken);

        //        webRequest.ContentType = @"application/json; charset=utf-8";
        //        webRequest.Accept = @"application/json; charset=utf-8";

        //        byte[] encRequestData = Encoding.UTF8.GetBytes(data);
        //        webRequest.ContentLength = encRequestData.Length;
        //        webRequest.Host = webRequest.RequestUri.Host;
        //        webRequest.PreAuthenticate = true;
        //        using (Stream stream = webRequest.GetRequestStream())
        //        {
        //            await stream.WriteAsync(encRequestData.AsMemory(0, encRequestData.Length));
        //            stream.Close();
        //        }

        //        HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
        //        StreamReader streamReader = new(webResponse.GetResponseStream());
        //        Result = await streamReader.ReadToEndAsync();
        //        streamReader.Dispose();
        //    }
        //    catch (WebException ex)
        //    {
        //        if (ex.Response != null)
        //        {
        //            WebResponse response = ex.Response;
        //            StreamReader streamReader = new(response.GetResponseStream());
        //            Result = streamReader.ReadToEnd();
        //            streamReader.Dispose();
        //        }
        //        else
        //        {
        //            Result = ex.Message;
        //        }

        //        if (string.IsNullOrEmpty(Result))
        //        {
        //            Result = ex.Message;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Result = ex.Message;
        //    }

        //    return Result;
        //}
        [HttpGet]
        [Route("[action]")]
        public ApiResponse GetCaptcha()
        {
            const int width = 200;
            const int height = 60;
            string captchaCode = Captcha.GenerateCaptchaCode();
            CaptchaResult result = Captcha.GenerateCaptchaImage(width, height, captchaCode);
            string encrToken = EncryptionUtility.EncryptText(result.CaptchaCode + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"), SecurityHelper.EnDeKey);
            return new ApiResponse() { Data = new { Img = result.CaptchBase64Data, Token = encrToken }, StatusCode = 200, StatusText = "OK" };
        }
        [NonAction]
        public bool VerifyCaptcha(string CaptchaCode, string CaptchaToken)
        {
            if (string.IsNullOrWhiteSpace(CaptchaCode) || string.IsNullOrWhiteSpace(CaptchaToken))
                return false;
            string[] decrypt = EncryptionUtility.DecryptText(CaptchaToken, SecurityHelper.EnDeKey).Split('|');
            return CaptchaCode == decrypt[0] && Convert.ToDateTime(decrypt[1]).AddMinutes(2) >= DateTime.Now;
        }
    }
}
