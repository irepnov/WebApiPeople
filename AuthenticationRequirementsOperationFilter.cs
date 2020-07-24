using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace PeopleWebApi
{
	public class AuthenticationRequirementsOperationFilter : Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter
    {
		/*public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
		{
			if (operation.parameters != null)
			{
				operation.parameters.Add(new Swashbuckle.Swagger.Parameter
                {
					name = "Authorization",
					@in = "header",
					description = "access token",
					required = false,
					type = "string"
				});
			}
		}*/
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			/*if (operation.Security == null)
				operation.Security = new List<OpenApiSecurityRequirement>();


			var scheme = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearer" } };
			operation.Security.Add(new OpenApiSecurityRequirement
			{
				[scheme] = new List<string>()
			});*/
			if (operation.Parameters != null)
			{
				operation.Parameters.Add(new OpenApiParameter
				{					
					Name = "Authorization",
					In = ParameterLocation.Header,
					Description = "Access token",
					Required = false
				});
			}
		}
	}
}
