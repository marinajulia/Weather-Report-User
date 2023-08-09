using AutoMapper;
using User.Domain.Service.User;
using User.Infra.Data;
using User.Infra.Repositories.User;
using User.SharedKernel.Utils.Notifications;
using User.SharedKernel.Utils;
using User.Domain.Mapper;

namespace User.Api.IoC
{
    public static class DependencyResolver
    {
        public static void Resolve(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(m =>
            {
                m.AddProfile(new AutoMapperProfile());
            });

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddDbContext<ApplicationContext>();

            Context(services);
            Repositories(services);
            Services(services);
        }
        public static void Context(IServiceCollection services)
        {
            services.AddScoped<ApplicationContext, ApplicationContext>();
            services.AddScoped<UserLoggedData>();
            services.AddScoped<INotification, Notification>();
        }
        public static void Repositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }

        public static void Services(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }
    }
}