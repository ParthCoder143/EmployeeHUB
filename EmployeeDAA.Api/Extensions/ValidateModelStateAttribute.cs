using EmployeeDAA.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmployeeDAA.Api.Extensions
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid && !IsAttributeExist(context))
            {
                string _message = string.Join(",", context.ModelState.Values
                                               .SelectMany(x => x.Errors)
                                               .Select(x => x.ErrorMessage).Distinct());

                context.Result = new BadRequestObjectResult(ApiResponseHelper.GenerateResponse(ApiStatusCode.Status400BadRequest, _message));
            }
        }
        private static bool IsAttributeExist(ActionExecutingContext context)
        {
            return (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(typeof(DontValidateAttribute), false).Any();
        }
    }
}
