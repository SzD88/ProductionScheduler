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
    [Migration("20230124191041_02-24-01")]
    partial class _022401
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

                    b.Property<int?>("Hour")
                        .HasColumnType("int");

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

            modelBuilder.Entity("ProductionScheduler.Core.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users");
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
