using Microsoft.EntityFrameworkCore;

namespace KmtBackend.DAL.Context
{
    public class KmtDbContext : DbContext
    {
        public KmtDbContext(DbContextOptions<KmtDbContext> options)
            : base(options)
        {

        }

        //public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(KmtDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
