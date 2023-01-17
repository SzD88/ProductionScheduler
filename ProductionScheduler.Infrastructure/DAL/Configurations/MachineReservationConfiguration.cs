using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Infrastructure.DAL.Configurations
{
    internal sealed class MachineReservationConfiguration : IEntityTypeConfiguration<MachineReservation>
    {
        public void Configure(EntityTypeBuilder<MachineReservation> builder)
        {
            builder.Property(x => x.EmployeeName)
               .HasConversion(x => x.Value, x => new EmployeeName(x));
        }
    }
}
