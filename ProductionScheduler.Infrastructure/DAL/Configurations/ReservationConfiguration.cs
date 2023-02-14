using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Infrastructure.DAL.Configurations
{
    internal sealed class ReservationConfiguration
        : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            //Table per Hierarchy (TPH)
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(x => x.Value, x => new ReservationId(x));

            builder.Property(x => x.MachineId)
               .HasConversion(x => x.Value, x => new MachineId(x));

            

            builder.Property(x => x.Hour)
               .HasConversion(x => x.Value, x => new Hour(x));

            builder.Property(x => x.Date)
              .HasConversion(x => x.Value, x => new Date(x));

            builder
                .HasDiscriminator<string>("Type")
                .HasValue<ReservationForService>(nameof(ReservationForService))
                .HasValue<ReservationForUser>(nameof(ReservationForUser));
            

        }
    }
}
