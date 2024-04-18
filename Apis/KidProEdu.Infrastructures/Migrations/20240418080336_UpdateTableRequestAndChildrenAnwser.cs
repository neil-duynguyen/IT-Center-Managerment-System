using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidProEdu.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableRequestAndChildrenAnwser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChildrenAnswer_Question_QuestionId",
                table: "ChildrenAnswer");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "RequestUserAccount");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Request",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "QuestionId",
                table: "ChildrenAnswer",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "ChildrenAnswer",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_ChildrenAnswer_Question_QuestionId",
                table: "ChildrenAnswer",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChildrenAnswer_Question_QuestionId",
                table: "ChildrenAnswer");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Request");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "RequestUserAccount",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "QuestionId",
                table: "ChildrenAnswer",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "ChildrenAnswer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChildrenAnswer_Question_QuestionId",
                table: "ChildrenAnswer",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
