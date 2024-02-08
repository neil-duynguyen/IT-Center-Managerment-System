using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidProEdu.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClassEquipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_Classes_ClassId",
                table: "Equipment");

            migrationBuilder.DropIndex(
                name: "IX_Equipment_ClassId",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Equipment");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Equipment",
                newName: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_RoomId",
                table: "Equipment",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipment_Rooms_RoomId",
                table: "Equipment",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_Rooms_RoomId",
                table: "Equipment");

            migrationBuilder.DropIndex(
                name: "IX_Equipment_RoomId",
                table: "Equipment");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Equipment",
                newName: "OrderId");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Equipment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ClassId",
                table: "Equipment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_ClassId",
                table: "Equipment",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipment_Classes_ClassId",
                table: "Equipment",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
