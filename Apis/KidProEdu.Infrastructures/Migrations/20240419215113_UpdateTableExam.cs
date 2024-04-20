using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidProEdu.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClassId",
                table: "Exam",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exam_ClassId",
                table: "Exam",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_Class_ClassId",
                table: "Exam",
                column: "ClassId",
                principalTable: "Class",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exam_Class_ClassId",
                table: "Exam");

            migrationBuilder.DropIndex(
                name: "IX_Exam_ClassId",
                table: "Exam");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Exam");
        }
    }
}
