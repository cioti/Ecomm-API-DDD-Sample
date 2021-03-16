using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecomm.Lib.WebApi.ResponseWrapper.Extensions
{
    internal static class HttpContextExtensions
    {
        public static bool IsApiRequest(this HttpContext context)
        {
            return !context.Request.Path.Value.Contains(".js") && !context.Request.Path.Value.Contains(".css");
        }

        public static bool IsSwagger(this HttpContext context)
        {
            return context.Request.Path.StartsWithSegments(new PathString("/swagger"));
        }  
    }
}
