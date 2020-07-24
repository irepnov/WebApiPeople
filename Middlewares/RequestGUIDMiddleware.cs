using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleWebApi.Middleware
{
    internal class RequestGUIDMiddleware
    {
        private readonly RequestDelegate _next;
        public RequestGUIDMiddleware(RequestDelegate next) => _next = next;       
        public async Task InvokeAsync(HttpContext context)
        {
            string RGUID = Guid.NewGuid().ToString();
            context.Request.Headers.Add("RGUID", RGUID);
            context.Response.Headers.Add("RGUID", RGUID);
            await _next.Invoke(context);
        }
    }
	
	public static class RequestGUIDExt
    {
        public static IApplicationBuilder UseRequestGUID(this IApplicationBuilder builder) => builder.UseMiddleware<RequestGUIDMiddleware>();        
    }

}
