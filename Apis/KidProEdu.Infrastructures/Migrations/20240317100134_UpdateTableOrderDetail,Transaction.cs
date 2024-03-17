using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidProEdu.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableOrderDetailTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChildCourse",
                table: "OrderDetail",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChildCourse",
                table: "OrderDetail");
        }
    }
}
