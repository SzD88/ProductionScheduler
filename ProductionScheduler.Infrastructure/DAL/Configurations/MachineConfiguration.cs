﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Infrastructure.DAL.Configurations
{
    internal sealed class MachineConfiguration : IEntityTypeConfiguration<Machine>
    {
        public void Configure(EntityTypeBuilder<Machine> builder)
        { 
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
               .HasConversion(x => x.Value, x => new MachineId(x));

            builder.Property(x => x.TimeForward)
               .HasConversion(x => x.From.Value, x => new ReservationTimeForward(x));

            builder.Property(x => x.Name)
               .HasConversion(x => x.Value, x => new MachineName(x)); 
        }
    }
}
