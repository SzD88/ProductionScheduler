using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductionScheduler.Infrastructure.Migrations
{
    public partial class _1302 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Reservations",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reservations");
        }
    }
}
