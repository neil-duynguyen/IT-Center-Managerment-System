using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidProEdu.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable_AdviseRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DivisionUserAccount_Division_DivisionsId",
                table: "DivisionUserAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_DivisionUserAccount_UserAccount_UserAccountsId",
                table: "DivisionUserAccount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DivisionUserAccount",
                table: "DivisionUserAccount");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Enrollment",
                newName: "Commission");

            migrationBuilder.RenameColumn(
                name: "UserAccountsId",
                table: "DivisionUserAccount",
                newName: "UserAccountId");

            migrationBuilder.RenameColumn(
                name: "DivisionsId",
                table: "DivisionUserAccount",
                newName: "DivisionId");

            migrationBuilder.RenameIndex(
                name: "IX_DivisionUserAccount_UserAccountsId",
                table: "DivisionUserAccount",
                newName: "IX_DivisionUserAccount_UserAccountId");

            migrationBuilder.RenameColumn(
                name: "CourseName",
                table: "AdviseRequest",
                newName: "Location");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "DivisionUserAccount",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "DivisionUserAccount",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "DivisionUserAccount",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "DeleteBy",
                table: "DivisionUserAccount",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionDate",
                table: "DivisionUserAccount",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DivisionUserAccount",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModificationBy",
                table: "DivisionUserAccount",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDate",
                table: "DivisionUserAccount",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusAdviseRequest",
                table: "AdviseRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DivisionUserAccount",
                table: "DivisionUserAccount",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionUserAccount_DivisionId",
                table: "DivisionUserAccount",
                column: "DivisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_DivisionUserAccount_Division_DivisionId",
                table: "DivisionUserAccount",
                column: "DivisionId",
                principalTable: "Division",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DivisionUserAccount_UserAccount_UserAccountId",
                table: "DivisionUserAccount",
                column: "UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DivisionUserAccount_Division_DivisionId",
                table: "DivisionUserAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_DivisionUserAccount_UserAccount_UserAccountId",
                table: "DivisionUserAccount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DivisionUserAccount",
                table: "DivisionUserAccount");

            migrationBuilder.DropIndex(
                name: "IX_DivisionUserAccount_DivisionId",
                table: "DivisionUserAccount");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DivisionUserAccount");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DivisionUserAccount");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "DivisionUserAccount");

            migrationBuilder.DropColumn(
                name: "DeleteBy",
                table: "DivisionUserAccount");

            migrationBuilder.DropColumn(
                name: "DeletionDate",
                table: "DivisionUserAccount");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DivisionUserAccount");

            migrationBuilder.DropColumn(
                name: "ModificationBy",
                table: "DivisionUserAccount");

            migrationBuilder.DropColumn(
                name: "ModificationDate",
                table: "DivisionUserAccount");

            migrationBuilder.DropColumn(
                name: "StatusAdviseRequest",
                table: "AdviseRequest");

            migrationBuilder.RenameColumn(
                name: "Commission",
                table: "Enrollment",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "UserAccountId",
                table: "DivisionUserAccount",
                newName: "UserAccountsId");

            migrationBuilder.RenameColumn(
                name: "DivisionId",
                table: "DivisionUserAccount",
                newName: "DivisionsId");

            migrationBuilder.RenameIndex(
                name: "IX_DivisionUserAccount_UserAccountId",
                table: "DivisionUserAccount",
                newName: "IX_DivisionUserAccount_UserAccountsId");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "AdviseRequest",
                newName: "CourseName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DivisionUserAccount",
                table: "DivisionUserAccount",
                columns: new[] { "DivisionsId", "UserAccountsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DivisionUserAccount_Division_DivisionsId",
                table: "DivisionUserAccount",
                column: "DivisionsId",
                principalTable: "Division",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DivisionUserAccount_UserAccount_UserAccountsId",
                table: "DivisionUserAccount",
                column: "UserAccountsId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
