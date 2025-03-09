using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Filters.Language
{

    public class HiddenAcceptLanguageHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            // اضافه کردن هدر به درخواست، اما مخفی کردن آن در UI
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Accept-Language",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new Microsoft.OpenApi.Any.OpenApiString("fa")
                },
                Extensions = new Dictionary<string, IOpenApiExtension>
            {
                { "x-hidden", new Microsoft.OpenApi.Any.OpenApiBoolean(false) } } // Swagger این را نمایش نمیدهد
            });
        }
    }
}