using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidProEdu.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable_Schedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Schedule",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Schedule");
        }
    }
}
