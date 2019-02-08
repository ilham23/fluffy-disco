using System.Web.Http;
using WebActivatorEx;
using WebAPI;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System.Web.Http.Description;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Filters;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WebAPI
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
              .EnableSwagger(
                c =>
                {
                    c.SingleApiVersion("v1", "Web sample");
                    c.OperationFilter<AddRequiredHeaderParameter>();
                }
              )
              .EnableSwaggerUi(
                c =>
                {
                    c.EnableApiKeySupport("Authorization", "header");
                }
                );
        }
        private class AddRequiredHeaderParameter : IOperationFilter
        {
            public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
            {
                if (operation.parameters == null)
                    operation.parameters = new List<Parameter>();

                operation.parameters.Add(new Parameter
                {
                    name = "Authorization",
                    @in = "header",
                    type = "string",
                    description = "Authorization",
                    required = true
                });
            }
        }

    }
}
