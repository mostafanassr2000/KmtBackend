using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KmtBackend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixUserLeaveTypeMission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransportationMethod",
                table: "Missions");

            migrationBuilder.DropColumn(
                name: "VehicleNumber",
                table: "Missions");

            migrationBuilder.RenameColumn(
                name: "Comments",
                table: "Missions",
                newName: "Notes");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "MinServiceMonths",
                table: "LeaveTypes",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MinServiceMonths",
                table: "LeaveTypes");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Missions",
                newName: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "TransportationMethod",
                table: "Missions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleNumber",
                table: "Missions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
