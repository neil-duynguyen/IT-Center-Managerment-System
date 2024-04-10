using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KidProEdu.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class AddTableConfigPointMultiplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfigPointMultipliers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestType = table.Column<int>(type: "int", nullable: false),
                    Multiplier = table.Column<double>(type: "float", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificationBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigPointMultipliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfigPointMultipliers_UserAccount_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ConfigPointMultipliers",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeleteBy", "DeletionDate", "IsDeleted", "ModificationBy", "ModificationDate", "Multiplier", "TestType", "UserId" },
                values: new object[,]
                {
                    { new Guid("5f40573d-f937-4aad-bfff-ae56d9f9057f"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, null, null, 0.40000000000000002, 4, new Guid("434d275c-ff7d-48fa-84e3-bed5ecadca82") },
                    { new Guid("7f312ad3-9ee2-4fbb-94cc-3a9c114aed25"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, null, null, 0.29999999999999999, 3, new Guid("434d275c-ff7d-48fa-84e3-bed5ecadca82") },
                    { new Guid("c7fd8087-90af-4d1d-bbcb-76825055ba7e"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, null, null, 0.14999999999999999, 2, new Guid("434d275c-ff7d-48fa-84e3-bed5ecadca82") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigPointMultipliers_UserId",
                table: "ConfigPointMultipliers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigPointMultipliers");
        }
    }
}
