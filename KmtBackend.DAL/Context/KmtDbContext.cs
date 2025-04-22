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

        // DbSet for Users entity
        public DbSet<User> Users { get; set; } = null!;
        // DbSet for Departments entity
        public DbSet<Department> Departments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all configurations from assembly
            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(KmtDbContext).Assembly);

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

            // Call base implementation
            base.OnModelCreating(modelBuilder);
        }
    }
}
