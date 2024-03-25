using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KidProEdu.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableSlotAdvise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SlotType",
                table: "Slot",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "SlotId",
                table: "AdviseRequest",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Slot",
                keyColumn: "Id",
                keyValue: new Guid("2758033d-7eef-41d6-a6ca-b9586fe3749d"),
                column: "SlotType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Slot",
                keyColumn: "Id",
                keyValue: new Guid("2c22f784-57ee-476c-a630-7c080b721db5"),
                column: "SlotType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Slot",
                keyColumn: "Id",
                keyValue: new Guid("85dd1d22-518b-4949-bf99-46d173caf3fe"),
                column: "SlotType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Slot",
                keyColumn: "Id",
                keyValue: new Guid("d42e7c30-ffd4-4a09-ac73-1d175f89a200"),
                column: "SlotType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Slot",
                keyColumn: "Id",
                keyValue: new Guid("dd5e5665-6522-4625-8d87-a807d856a318"),
                column: "SlotType",
                value: 1);

            migrationBuilder.InsertData(
                table: "Slot",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeleteBy", "DeletionDate", "EndTime", "IsDeleted", "ModificationBy", "ModificationDate", "Name", "SlotType", "StartTime" },
                values: new object[,]
                {
                    { new Guid("2c22f722-57ee-476c-a630-7c080b721db5"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new TimeSpan(0, 14, 0, 0, 0), false, null, null, "Slot5", 2, new TimeSpan(0, 13, 0, 0, 0) },
                    { new Guid("2c22f733-57ee-476c-a630-7c080b721db5"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new TimeSpan(0, 15, 0, 0, 0), false, null, null, "Slot5", 2, new TimeSpan(0, 14, 0, 0, 0) },
                    { new Guid("2c22f744-57ee-476c-a630-7c080b721db5"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new TimeSpan(0, 16, 0, 0, 0), false, null, null, "Slot5", 2, new TimeSpan(0, 15, 0, 0, 0) },
                    { new Guid("2c22f755-57ee-476c-a630-7c080b721db5"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new TimeSpan(0, 17, 0, 0, 0), false, null, null, "Slot5", 2, new TimeSpan(0, 16, 0, 0, 0) },
                    { new Guid("2c22f784-23ee-476c-a630-7c080b721db5"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new TimeSpan(0, 20, 0, 0, 0), false, null, null, "Slot5", 2, new TimeSpan(0, 19, 0, 0, 0) },
                    { new Guid("2c22f784-66ee-336c-a630-7c080b721db5"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new TimeSpan(0, 19, 0, 0, 0), false, null, null, "Slot5", 2, new TimeSpan(0, 18, 0, 0, 0) },
                    { new Guid("2c33f784-57ee-476c-a630-7c080b721db5"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new TimeSpan(0, 8, 0, 0, 0), false, null, null, "Slot5", 2, new TimeSpan(0, 7, 0, 0, 0) },
                    { new Guid("2c44f784-57ee-476c-a630-7c080b721db5"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new TimeSpan(0, 9, 0, 0, 0), false, null, null, "Slot5", 2, new TimeSpan(0, 8, 0, 0, 0) },
                    { new Guid("2c55f784-57ee-476c-a630-7c080b721db5"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new TimeSpan(0, 10, 0, 0, 0), false, null, null, "Slot5", 2, new TimeSpan(0, 9, 0, 0, 0) },
                    { new Guid("2c66f784-57ee-476c-a630-7c080b721db5"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new TimeSpan(0, 11, 0, 0, 0), false, null, null, "Slot5", 2, new TimeSpan(0, 10, 0, 0, 0) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdviseRequest_SlotId",
                table: "AdviseRequest",
                column: "SlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdviseRequest_Slot_SlotId",
                table: "AdviseRequest",
                column: "SlotId",
                principalTable: "Slot",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdviseRequest_Slot_SlotId",
                table: "AdviseRequest");

            migrationBuilder.DropIndex(
                name: "IX_AdviseRequest_SlotId",
                table: "AdviseRequest");

            migrationBuilder.DeleteData(
                table: "Slot",
                keyColumn: "Id",
                keyValue: new Guid("2c22f722-57ee-476c-a630-7c080b721db5"));

            migrationBuilder.DeleteData(
                table: "Slot",
                keyColumn: "Id",
                keyValue: new Guid("2c22f733-57ee-476c-a630-7c080b721db5"));

            migrationBuilder.DeleteData(
                table: "Slot",
                keyColumn: "Id",
                keyValue: new Guid("2c22f744-57ee-476c-a630-7c080b721db5"));

            migrationBuilder.DeleteData(
                table: "Slot",
                keyColumn: "Id",
                keyValue: new Guid("2c22f755-57ee-476c-a630-7c080b721db5"));

            migrationBuilder.DeleteData(
                table: "Slot",
                keyColumn: "Id",
                keyValue: new Guid("2c22f784-23ee-476c-a630-7c080b721db5"));

            migrationBuilder.DeleteData(
                table: "Slot",
                keyColumn: "Id",
                keyValue: new Guid("2c22f784-66ee-336c-a630-7c080b721db5"));

            migrationBuilder.DeleteData(
                table: "Slot",
                keyColumn: "Id",
                keyValue: new Guid("2c33f784-57ee-476c-a630-7c080b721db5"));

            migrationBuilder.DeleteData(
                table: "Slot",
                keyColumn: "Id",
                keyValue: new Guid("2c44f784-57ee-476c-a630-7c080b721db5"));

            migrationBuilder.DeleteData(
                table: "Slot",
                keyColumn: "Id",
                keyValue: new Guid("2c55f784-57ee-476c-a630-7c080b721db5"));

            migrationBuilder.DeleteData(
                table: "Slot",
                keyColumn: "Id",
                keyValue: new Guid("2c66f784-57ee-476c-a630-7c080b721db5"));

            migrationBuilder.DropColumn(
                name: "SlotType",
                table: "Slot");

            migrationBuilder.DropColumn(
                name: "SlotId",
                table: "AdviseRequest");
        }
    }
}
