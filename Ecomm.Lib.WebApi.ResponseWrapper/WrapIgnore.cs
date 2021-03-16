using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Ecomm.Lib.WebApi.ResponseWrapper
{
    /// <summary>
    /// Apply this attribute on routes that you don't want to be wrapped
    /// </summary>
    public class WrapIgnore : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Request.Headers.Add("ResponseWrapperIgnore", new string[] { "true" });
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // our code after action executes
        }
    }
}
