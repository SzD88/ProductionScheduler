using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionScheduler.Core.Entities;

namespace ProductionScheduler.Infrastructure.DAL.Configurations
{
    internal sealed   class ServiceReservationConfiguration : IEntityTypeConfiguration<ReservationForService>
    {
        public void Configure(EntityTypeBuilder<ReservationForService> builder)
        { 
        }
    }
}
