using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidProEdu.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationTableLessonAndCategoryEquipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentLesson");

            migrationBuilder.CreateTable(
                name: "CategoryEquipmentLesson",
                columns: table => new
                {
                    CategoryEquipmentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessonsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryEquipmentLesson", x => new { x.CategoryEquipmentsId, x.LessonsId });
                    table.ForeignKey(
                        name: "FK_CategoryEquipmentLesson_CategoryEquipment_CategoryEquipmentsId",
                        column: x => x.CategoryEquipmentsId,
                        principalTable: "CategoryEquipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryEquipmentLesson_Lesson_LessonsId",
                        column: x => x.LessonsId,
                        principalTable: "Lesson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryEquipmentLesson_LessonsId",
                table: "CategoryEquipmentLesson",
                column: "LessonsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryEquipmentLesson");

            migrationBuilder.CreateTable(
                name: "EquipmentLesson",
                columns: table => new
                {
                    EquipmentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessonsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentLesson", x => new { x.EquipmentsId, x.LessonsId });
                    table.ForeignKey(
                        name: "FK_EquipmentLesson_Equipment_EquipmentsId",
                        column: x => x.EquipmentsId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentLesson_Lesson_LessonsId",
                        column: x => x.LessonsId,
                        principalTable: "Lesson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentLesson_LessonsId",
                table: "EquipmentLesson",
                column: "LessonsId");
        }
    }
}
