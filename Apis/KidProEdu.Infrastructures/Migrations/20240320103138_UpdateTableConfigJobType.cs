using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KidProEdu.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableConfigJobType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "TeachingClassHistory",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "TeachingClassHistory",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "MinSlot",
                table: "ConfigJobType",
                newName: "Slotperweek");

            migrationBuilder.InsertData(
                table: "ConfigJobType",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeleteBy", "DeletionDate", "IsDeleted", "JobType", "ModificationBy", "ModificationDate", "Slotperweek" },
                values: new object[,]
                {
                    { new Guid("572184c6-7885-47dc-8dee-8bfad25ae8a7"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, 2, null, null, 15 },
                    { new Guid("c7761baf-4675-4d4d-b61a-584f36835064"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, 1, null, null, 30 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ConfigJobType",
                keyColumn: "Id",
                keyValue: new Guid("572184c6-7885-47dc-8dee-8bfad25ae8a7"));

            migrationBuilder.DeleteData(
                table: "ConfigJobType",
                keyColumn: "Id",
                keyValue: new Guid("c7761baf-4675-4d4d-b61a-584f36835064"));

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "TeachingClassHistory",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "TeachingClassHistory",
                newName: "EndTime");

            migrationBuilder.RenameColumn(
                name: "Slotperweek",
                table: "ConfigJobType",
                newName: "MinSlot");
        }
    }
}
