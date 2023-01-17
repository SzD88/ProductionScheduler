﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProductionScheduler.Infrastructure.DAL;

#nullable disable

namespace ProductionScheduler.Infrastructure.Migrations
{
    [DbContext(typeof(ProductionSchedulerDbContext))]
    [Migration("20230117084813_2023-01-17-serviceReservation")]
    partial class _20230117serviceReservation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ProductionScheduler.Core.Entities.Machine", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("TimeForward")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("PeriodMachineReservations");
                });

            modelBuilder.Entity("ProductionScheduler.Core.Entities.Reservation", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("Date")
                        .HasColumnType("datetimeoffset");

                    b.Property<short?>("Hour")
                        .HasColumnType("smallint");

                    b.Property<Guid?>("MachineId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MachineId");

                    b.ToTable("Reservations");

                    b.HasDiscriminator<string>("Type").HasValue("Reservation");
                });

            modelBuilder.Entity("ProductionScheduler.Core.Entities.MachineReservation", b =>
                {
                    b.HasBaseType("ProductionScheduler.Core.Entities.Reservation");

                    b.Property<string>("EmployeeName")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("MachineReservation");
                });

            modelBuilder.Entity("ProductionScheduler.Core.Entities.ServiceReservation", b =>
                {
                    b.HasBaseType("ProductionScheduler.Core.Entities.Reservation");

                    b.HasDiscriminator().HasValue("ServiceReservation");
                });

            modelBuilder.Entity("ProductionScheduler.Core.Entities.Reservation", b =>
                {
                    b.HasOne("ProductionScheduler.Core.Entities.Machine", null)
                        .WithMany("Reservations")
                        .HasForeignKey("MachineId");
                });

            modelBuilder.Entity("ProductionScheduler.Core.Entities.Machine", b =>
                {
                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}
