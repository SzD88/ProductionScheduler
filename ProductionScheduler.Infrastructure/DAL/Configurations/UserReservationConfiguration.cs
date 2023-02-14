using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Infrastructure.DAL.Configurations
{
    internal sealed class UserReservationConfiguration : IEntityTypeConfiguration<ReservationForUser>
    {
        public void Configure(EntityTypeBuilder<ReservationForUser> builder)
        {
            builder.Property(x => x.UserId)
              .HasConversion(x => x.Value, x => new UserId(x));

            builder.Property(x => x.EmployeeName)
               .HasConversion(x => x.Value, x => new EmployeeName(x));
        }
    }
}
