using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionScheduler.Core.Entities;

namespace ProductionScheduler.Infrastructure.DAL.Configurations
{
    internal sealed   class ServiceReservationConfiguration : IEntityTypeConfiguration<ServiceReservation>
    {
        public void Configure(EntityTypeBuilder<ServiceReservation> builder)
        { 
        }
    }
}
