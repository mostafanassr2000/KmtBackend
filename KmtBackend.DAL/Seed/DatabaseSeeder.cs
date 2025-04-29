using KmtBackend.DAL.Constants;
using KmtBackend.DAL.Context;
using KmtBackend.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Super Admin");

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

                // Add role to context
                await context.Roles.AddAsync(adminRole);
                // Save to get role ID
                await context.SaveChangesAsync();

                // Get all permissions
                var allPermissions = await context.Permissions.ToListAsync();

                // Assign all permissions to admin role
                foreach (var permission in allPermissions)
                {
                    // Create role-permission relationship
                    var rolePermission = new RolePermission
                    {
                        Id = Guid.NewGuid(),
                        RoleId = adminRole.Id,
                        PermissionId = permission.Id,
                        CreatedAt = DateTime.UtcNow
                    };

                    // Add to context
                    await context.RolePermissions.AddAsync(rolePermission);
                }

                // Save all role permissions
                await context.SaveChangesAsync();

                // Log success
                logger.LogInformation("Super Admin role created with all permissions");
            }
            else
            {
                // Log that admin role already exists
                logger.LogInformation("Super Admin role already exists");

                // Get all permissions
                var allPermissionIds = await context.Permissions
                    .Select(p => p.Id)
                    .ToListAsync();

                // Get current admin role permissions
                var currentPermissionIds = await context.RolePermissions
                    .Where(rp => rp.RoleId == adminRole.Id)
                    .Select(rp => rp.PermissionId)
                    .ToListAsync();

                // Find permissions not assigned to admin role
                var missingPermissionIds = allPermissionIds
                    .Except(currentPermissionIds)
                    .ToList();

                // Assign any missing permissions
                if (missingPermissionIds.Any())
                {
                    // Log missing permissions being added
                    logger.LogInformation("Adding {Count} missing permissions to Super Admin role", missingPermissionIds.Count);

                    // Create role-permission relationships for missing permissions
                    foreach (var permissionId in missingPermissionIds)
                    {
                        // Create new relationship
                        var rolePermission = new RolePermission
                        {
                            Id = Guid.NewGuid(),
                            RoleId = adminRole.Id,
                            PermissionId = permissionId,
                            CreatedAt = DateTime.UtcNow
                        };

                        // Add to context
                        await context.RolePermissions.AddAsync(rolePermission);
                    }

                    // Save changes
                    await context.SaveChangesAsync();

                    // Log success
                    logger.LogInformation("Missing permissions added to Super Admin role");
                }
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
                Title = "Super Administrator",
                CreatedAt = DateTime.UtcNow
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
                // Create user-role relationship
                var userRole = new UserRole
                {
                    Id = Guid.NewGuid(),
                    UserId = superAdmin.Id,
                    RoleId = adminRole.Id,
                    CreatedAt = DateTime.UtcNow
                };

                // Add to context
                await context.UserRoles.AddAsync(userRole);
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
