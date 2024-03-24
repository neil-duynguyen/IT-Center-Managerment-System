using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidProEdu.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableAdviseRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "AdviseRequest");

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "AdviseRequest",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdviseRequest_LocationId",
                table: "AdviseRequest",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdviseRequest_Location_LocationId",
                table: "AdviseRequest",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdviseRequest_Location_LocationId",
                table: "AdviseRequest");

            migrationBuilder.DropIndex(
                name: "IX_AdviseRequest_LocationId",
                table: "AdviseRequest");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "AdviseRequest");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "AdviseRequest",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
