using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using User.Api.IoC;
using User.Domain.Service.User;
using User.Domain.Token;

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
            services.AddSwaggerGen();

            services.Resolve();

            var key = Encoding.ASCII.GetBytes(Settings.Secret);

            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context => {
                        var allClaims = context.Principal.Claims;
                        var authService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        var userId = int.Parse((context.Principal.Identity.Name));
                        var tokenAuth = allClaims.FirstOrDefault(a => a.Type == ClaimTypes.Authentication)?.Value;
                        var user = authService.Allow(userId);
                        if (!user)
                            context.Fail("Unauthorized");

                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

            });
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
