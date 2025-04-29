using KmtBackend.BLL.Managers;
using KmtBackend.BLL.Managers.Interfaces;
using KmtBackend.DAL.Repositories.Interfaces;
using KmtBackend.DAL.Repositories;
using KmtBackend.Infrastructure.TokenGenerator;

namespace KmtBackend.API.ServicesExtension
{
    public static class ServicesExtension
    {
        public static void RegisterManagers(this IServiceCollection services)
        {
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IDepartmentManager, DepartmentManager>();
        }

        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        }

        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        }
    }
}
