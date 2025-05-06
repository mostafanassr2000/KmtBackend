using KmtBackend.BLL.Managers;
using KmtBackend.BLL.Managers.Interfaces;
using KmtBackend.DAL.Repositories.Interfaces;
using KmtBackend.DAL.Repositories;
using KmtBackend.Infrastructure.TokenGenerator;
using KmtBackend.DAL.Constants;

namespace KmtBackend.API.ServicesExtension
{
    public static class ServicesExtension
    {
        public static void RegisterManagers(this IServiceCollection services)
        {
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IDepartmentManager, DepartmentManager>();
            services.AddScoped<IRoleManager, RoleManager>();
            services.AddScoped<IPermissionManager, PermissionManager>();
            services.AddScoped<IUserRoleManager, UserRoleManager>();
            services.AddScoped<ITitleManager, TitleManager>();
        }

        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<ITitleRepository, TitleRepository>();
        }

        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        }

        public static void AddPermissionPolicies(this IServiceCollection services)
        {
            // Get authorization policy builder
            var authBuilder = services.AddAuthorizationBuilder();

            // Loop through all permissions from our constants
            foreach (var permission in Permissions.GetAllPermissions())
            {
                // Create policy name from permission code
                var policyName = $"Permission:{permission}";

                // Register policy with the required permission
                authBuilder.AddPolicy(policyName, policy =>
                {
                    // Require authenticated user
                    policy.RequireAuthenticatedUser();
                    // Check for specific permission claim
                    policy.RequireAssertion(context =>
                    {
                        // Check if the user has the required permission claim
                        return context.User.HasClaim(c =>
                            c.Type == "permission" && c.Value == permission);
                    });
                });
            }
        }
    }
}
