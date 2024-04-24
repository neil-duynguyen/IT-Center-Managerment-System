using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KidProEdu.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class Init2Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserAccount",
                columns: new[] { "Id", "Address", "Avatar", "BankAccountName", "BankAccountNumber", "BankName", "CreatedBy", "CreationDate", "DateOfBirth", "DeleteBy", "DeletionDate", "Email", "FullName", "GenderType", "IsDeleted", "LocationId", "ModificationBy", "ModificationDate", "OTP", "PasswordHash", "Phone", "RoleId", "Status", "UserName" },
                values: new object[,]
                {
                    { new Guid("434d275c-ff7d-35fa-84e3-bed5ecadca84"), null, null, null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddTicks(2008), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddTicks(1979), null, null, "teacher@gmail.com", "Nguyen Minh Ngoc", null, false, null, null, null, null, "d041c3d3ca4ed64c5b54c5d807bd9a0bd2d6ae3609ecd2d06ac383db449360e1", "0356534796", new Guid("d5fa55c7-315d-4634-9c73-08dbbc3f3a53"), 1, "Teacher" },
                    { new Guid("434d275c-ff7d-72fa-84e3-bed5ecadca84"), null, null, null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddTicks(2008), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddTicks(1979), null, null, "minhtuan234@gmail.com", "Pham Minh Tuan", null, false, null, null, null, null, "02e213a1388234c768cd561c4114d124eaa9cca64cf9d8b118b52001c93952d7", "0398324796", new Guid("d5fa55c7-315d-4634-9c73-08dbbc3f3a54"), 1, "Parent" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserAccount",
                keyColumn: "Id",
                keyValue: new Guid("434d275c-ff7d-35fa-84e3-bed5ecadca84"));

            migrationBuilder.DeleteData(
                table: "UserAccount",
                keyColumn: "Id",
                keyValue: new Guid("434d275c-ff7d-72fa-84e3-bed5ecadca84"));
        }
    }
}
