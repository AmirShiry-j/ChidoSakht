using Application.Interfaces.ConfigService;
using Application.Interfaces.Messagers.SmsService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Filters.Language
{
    public class AcceptLanguageHeaderFilter : IOperationFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public AcceptLanguageHeaderFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();
            //
            var configService = _serviceProvider.GetRequiredService<IConfigService>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Accept-Language",
                In = ParameterLocation.Header,
                Required = false,
                //Description = "fa یا en",
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new Microsoft.OpenApi.Any.OpenApiString(configService.Config.Localization.DefaultCulture)
                }
            });
        }
    }
}