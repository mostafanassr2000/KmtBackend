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
                await SeedSuperAdminAsync(context, services);
                
                logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<KmtDbContext>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }
        
        private static async Task SeedSuperAdminAsync(KmtDbContext context, IServiceProvider services)
        {
            // Check if super admin already exists
            if (await context.Users.AnyAsync(u => u.Email == "admin@admin.com"))
            {
                return; // Super admin already exists
            }
            
            // Create password hasher
            var passwordHasher = new PasswordHasher<User>();
            
            // Create super admin user
            var superAdmin = new User
            {
                Username = "admin",
                Email = "admin@admin.com",
                //Role = "Admin",
                Title = "Super Administrator",
                CreatedAt = DateTime.UtcNow
            };
            
            // Hash password
            superAdmin.PasswordHash = passwordHasher.HashPassword(superAdmin, "Admin@123");
            
            // Add to database
            await context.Users.AddAsync(superAdmin);
            await context.SaveChangesAsync();
        }
    }
}
