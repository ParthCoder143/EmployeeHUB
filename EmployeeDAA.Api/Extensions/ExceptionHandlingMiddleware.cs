using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Logging;


namespace EmployeeDAA.Api.Extensions
{
    public class ErrorHandlingFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            string additionalMsg = string.Empty;

            var exception = context.Exception;

            if (context.HttpContext != null)
            {
                if (context.HttpContext.Request != null)
                {
                    additionalMsg = "URL: " + context.HttpContext.Request.Path + ", Method: " + context.HttpContext.Request.Method;
                }
            }

            if (exception.InnerException != null)
            {
                string desc = "Message:" + exception.Message + ",InnerException:" + exception.InnerException + ",StackTrace:" + exception.StackTrace + ", Additional Message: " + additionalMsg;
                //LogHelper.CreateLog("Unhandled Exception: " + desc, "EXCEPTION", "API_REQUEST");
            }
            else
            {
                string desc = "Message:" + exception.Message + ",StackTrace:" + exception.StackTrace + ", Additional Message: " + additionalMsg;
                //LogHelper.CreateLog("Unhandled Exception: " + desc, "EXCEPTION", "API_REQUEST");
            }
        }
    }
}
