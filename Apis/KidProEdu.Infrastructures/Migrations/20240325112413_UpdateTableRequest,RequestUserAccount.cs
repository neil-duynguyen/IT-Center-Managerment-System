using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidProEdu.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableRequestRequestUserAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Request");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "RequestUserAccount",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "RequestUserAccount");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Request",
                type: "int",
                nullable: true);
        }
    }
}
