using KmtBackend.DAL.Constants;
using KmtBackend.DAL.Context;
using KmtBackend.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KmtBackend.DAL.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            
            try
            {
                var context = services.GetRequiredService<KmtDbContext>();
                var logger = services.GetRequiredService<ILogger<KmtDbContext>>();
                
                logger.LogInformation("Applying pending migrations if any...");
                await context.Database.MigrateAsync();
                
                logger.LogInformation("Seeding database...");

                // Seed permissions first (no dependencies)
                await SeedPermissionsAsync(context, logger);

                // Seed roles next (depends on permissions)
                await SeedRolesAsync(context, logger);

                // Seed admin user last (depends on roles)
                await SeedSuperAdminAsync(context, logger);

                logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<KmtDbContext>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        /// <summary>
        /// Seeds all permissions to the database
        /// </summary>
        private static async Task SeedPermissionsAsync(KmtDbContext context, ILogger logger)
        {
            // Log start of permission seeding
            logger.LogInformation("Seeding permissions...");

            // Get all permissions with descriptions
            var allPermissions = Permissions.GetAllPermissionsWithDescriptions();

            // Check if permissions need to be seeded
            if (!await context.Permissions.AnyAsync())
            {
                // Log that permissions are being created
                logger.LogInformation("Creating {Count} permissions", allPermissions.Count);

                // Create each permission from our constants
                foreach (var permissionPair in allPermissions)
                {
                    // Create new permission entity
                    var permission = new Permission
                    {
                        // Generate new ID
                        Id = Guid.NewGuid(),
                        Code = permissionPair.Key,
                        Description = permissionPair.Value.Description,
                        DescriptionAr = permissionPair.Value.DescriptionAr,
                        CreatedAt = DateTime.UtcNow
                    };

                    await context.Permissions.AddAsync(permission);
                }

                // Save all permissions to database
                await context.SaveChangesAsync();

                // Log success
                logger.LogInformation("Permissions seeded successfully");
            }
            else
            {
                // Log that permissions already exist
                logger.LogInformation("Permissions already exist in database");
            }
        }

        /// <summary>
        /// Seeds the Super Admin role with all permissions
        /// </summary>
        private static async Task SeedRolesAsync(KmtDbContext context, ILogger logger)
        {
            // Log start of role seeding
            logger.LogInformation("Seeding roles...");

            // Check if admin role needs to be created
            //var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Super Admin");
            var adminRole = await context.Roles.Where(r => r.Name == "Super Admin")
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync();
            // Get all permissions
            var allPermissions = await context.Permissions.ToListAsync();

            if (adminRole == null)
            {
                // Log admin role creation
                logger.LogInformation("Creating Super Admin role");

                // Create the Super Admin role
                adminRole = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "Super Admin",
                    NameAr = "مدير النظام",
                    Description = "Has full access to all system features",
                    DescriptionAr = "لديه وصول كامل إلى جميع ميزات النظام",
                    CreatedAt = DateTime.UtcNow
                };

                adminRole.Permissions = allPermissions;

                // Add role to context
                await context.Roles.AddAsync(adminRole);

                // Save to get role ID
                await context.SaveChangesAsync();

                // Log success
                logger.LogInformation("Super Admin role created with all permissions");
            }
            else
            {
                // Fetch the permissions already assigned to the admin role
                var assignedPermissions = adminRole.Permissions.ToList();

                // Perform the Except operation in memory
                var missingPermissions = allPermissions.Except(assignedPermissions).ToList();

                // Add the missing permissions to the admin role
                foreach (var missingPermission in missingPermissions)
                {
                    adminRole.Permissions.Add(missingPermission);
                }
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Seeds the Super Admin user and assigns admin role
        /// </summary>
        private static async Task SeedSuperAdminAsync(KmtDbContext context, ILogger logger)
        {
            // Log start of admin user seeding
            logger.LogInformation("Seeding super admin user...");

            // Check if super admin already exists
            if (await context.Users.AnyAsync(u => u.Email == "admin@admin.com"))
            {
                // Log that admin already exists
                logger.LogInformation("Super admin user already exists");
                return;
            }

            // Create password hasher
            var passwordHasher = new PasswordHasher<User>();

            // Create super admin user
            var superAdmin = new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                Email = "admin@admin.com",
                CreatedAt = DateTime.UtcNow,
                PhoneNumber = "+201010101010"
            };

            // Hash password
            superAdmin.PasswordHash = passwordHasher.HashPassword(superAdmin, "Admin@123");

            // Add to database
            await context.Users.AddAsync(superAdmin);
            // Save to get user ID
            await context.SaveChangesAsync();

            // Get the admin role
            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Super Admin");

            // If admin role exists, assign it to the user
            if (adminRole != null)
            {
                superAdmin.Roles.Add(adminRole);

                // Save changes
                await context.SaveChangesAsync();

                // Log success
                logger.LogInformation("Assigned Super Admin role to admin user");
            }
            else
            {
                // Log warning that role doesn't exist
                logger.LogWarning("Super Admin role not found, unable to assign to admin user");
            }

            // Log success
            logger.LogInformation("Super admin user created successfully");
        }
    }
}
