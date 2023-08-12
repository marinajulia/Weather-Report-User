using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace User.Api.Extensions
{
    public static class SwaggerExtensions
    {
        public static void SwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "WeatherReport",
                    Description = "API Weather Report"
                });

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
                {
                    Description = "Beared token",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.OperationFilter<SecurityRequirementsOperationFilter>();

            });
        }
    }
}
