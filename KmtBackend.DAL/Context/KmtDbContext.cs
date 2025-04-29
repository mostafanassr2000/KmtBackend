using KmtBackend.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace KmtBackend.DAL.Context
{
    public class KmtDbContext : DbContext
    {
        public KmtDbContext(DbContextOptions<KmtDbContext> options)
            : base(options)
        {
            // Empty constructor passes options to base
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User email must be unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Department has many users
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Users)
                .WithOne(u => u.Department)
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Permission code must be unique
            modelBuilder.Entity<Permission>()
                .HasIndex(p => p.Code)
                .IsUnique();
                
            // Role name must be unique
            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();

            // Call base implementation
            base.OnModelCreating(modelBuilder);
        }
    }
}
