using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidProEdu.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChildrenProfileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChildrenCode",
                table: "ChildrenProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChildrenCode",
                table: "ChildrenProfile");
        }
    }
}
