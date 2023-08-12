using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using User.Api.Extensions;
using User.Api.IoC;
using User.Domain.Common.Security;

namespace User.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            var authSettingsSection = Configuration.GetSection("AuthSettings");
            services.Configure<AuthSettings>(authSettingsSection);

            var authSettings = authSettingsSection.Get<AuthSettings>();
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Secret));

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddPolicyScheme("weatherreport", "Authorization Bearer or AccessToken", options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    if (context.Request.Headers["Access-Token"].Any())
                    {
                        return "Access-Token";
                    }

                    return JwtBearerDefaults.AuthenticationScheme;
                };
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = "weatherreport",

                    ValidateAudience = false,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,

                    ValidateLifetime = true,
                    RequireExpirationTime = true,

                };

            });

            services.Resolve();
            services.SwaggerConfiguration();
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            //app.UseSwagger();
            //app.UseSwaggerUI();

            //app.UseRouting();
            //app.UseAuthentication();

            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});

            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.RoutePrefix = "swagger";
                setup.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Documentation");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
