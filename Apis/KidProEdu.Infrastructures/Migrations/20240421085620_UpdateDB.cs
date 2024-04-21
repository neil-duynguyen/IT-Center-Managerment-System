using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidProEdu.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ConfigTheme",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ConfigSystem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserAccountId",
                table: "ConfigJobType",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ConfigJobType",
                keyColumn: "Id",
                keyValue: new Guid("572184c6-7885-47dc-8dee-8bfad25ae8a7"),
                column: "UserAccountId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ConfigJobType",
                keyColumn: "Id",
                keyValue: new Guid("c7761baf-4675-4d4d-b61a-584f36835064"),
                column: "UserAccountId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_ConfigTheme_UserId",
                table: "ConfigTheme",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigSystem_UserId",
                table: "ConfigSystem",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigJobType_UserAccountId",
                table: "ConfigJobType",
                column: "UserAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfigJobType_UserAccount_UserAccountId",
                table: "ConfigJobType",
                column: "UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfigSystem_UserAccount_UserId",
                table: "ConfigSystem",
                column: "UserId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfigTheme_UserAccount_UserId",
                table: "ConfigTheme",
                column: "UserId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfigJobType_UserAccount_UserAccountId",
                table: "ConfigJobType");

            migrationBuilder.DropForeignKey(
                name: "FK_ConfigSystem_UserAccount_UserId",
                table: "ConfigSystem");

            migrationBuilder.DropForeignKey(
                name: "FK_ConfigTheme_UserAccount_UserId",
                table: "ConfigTheme");

            migrationBuilder.DropIndex(
                name: "IX_ConfigTheme_UserId",
                table: "ConfigTheme");

            migrationBuilder.DropIndex(
                name: "IX_ConfigSystem_UserId",
                table: "ConfigSystem");

            migrationBuilder.DropIndex(
                name: "IX_ConfigJobType_UserAccountId",
                table: "ConfigJobType");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ConfigTheme");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ConfigSystem");

            migrationBuilder.DropColumn(
                name: "UserAccountId",
                table: "ConfigJobType");
        }
    }
}
