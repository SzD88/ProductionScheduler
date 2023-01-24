

using Microsoft.EntityFrameworkCore;
using ProductionScheduler.Core.Entities;

namespace ProductionScheduler.Infrastructure.DAL
{
    internal  sealed class ProductionSchedulerDbContext : DbContext
    {
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Machine> PeriodMachineReservations { get; set; }
        public DbSet<User> Users { get; set; }  

        public ProductionSchedulerDbContext(DbContextOptions<ProductionSchedulerDbContext> dbContextOptions) 
            : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        
        }
    
    }
}
