using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidProEdu.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class RequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EquimentType",
                table: "Request");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryEquipmentId",
                table: "Request",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Request",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnDeadline",
                table: "Request",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoomId",
                table: "Request",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryEquipmentId",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "ReturnDeadline",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Request");

            migrationBuilder.AddColumn<string>(
                name: "EquimentType",
                table: "Request",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
