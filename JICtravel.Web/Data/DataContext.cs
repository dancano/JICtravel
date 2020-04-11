using JICtravel.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JICtravel.Web.Data
{
    public class DataContext : IdentityDbContext<SlaveEntity>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<SlaveEntity> Slaves { get; set; }

        public DbSet<TripEntity> Trips { get; set; }

        public DbSet<TripDetailEntity> TripDetails { get; set; }

        public DbSet<ExpensiveTypeEntity> ExpensivesType { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SlaveEntity>()
                .HasIndex(d => d.Document)
                .IsUnique();
        }

    }
}
