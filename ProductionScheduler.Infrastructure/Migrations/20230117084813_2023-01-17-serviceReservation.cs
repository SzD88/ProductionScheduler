using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductionScheduler.Infrastructure.Migrations
{
    public partial class _20230117serviceReservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_PeriodMachineReservations_PeriodMachineReservationId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_PeriodMachineReservationId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "PeriodMachineReservationId",
                table: "Reservations");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_MachineId",
                table: "Reservations",
                column: "MachineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_PeriodMachineReservations_MachineId",
                table: "Reservations",
                column: "MachineId",
                principalTable: "PeriodMachineReservations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_PeriodMachineReservations_MachineId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_MachineId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Reservations");

            migrationBuilder.AddColumn<Guid>(
                name: "PeriodMachineReservationId",
                table: "Reservations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_PeriodMachineReservationId",
                table: "Reservations",
                column: "PeriodMachineReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_PeriodMachineReservations_PeriodMachineReservationId",
                table: "Reservations",
                column: "PeriodMachineReservationId",
                principalTable: "PeriodMachineReservations",
                principalColumn: "Id");
        }
    }
}
