using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidProEdu.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationTableRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdviseRequest_User_UserId",
                table: "AdviseRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_Blog_User_UserId",
                table: "Blog");

            migrationBuilder.DropForeignKey(
                name: "FK_ChildrenProfile_User_UserId",
                table: "ChildrenProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_Class_User_UserId",
                table: "Class");

            migrationBuilder.DropForeignKey(
                name: "FK_Contract_User_UserId",
                table: "Contract");

            migrationBuilder.DropForeignKey(
                name: "FK_DivisionUserAccount_User_UserAccountsId",
                table: "DivisionUserAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_User_UserAccountId",
                table: "Enrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_User_UserId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationUser_User_UserId",
                table: "NotificationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_UserId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_User_UserAccountId",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestUserAccount_User_RecieverId",
                table: "RequestUserAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_User_UserId",
                table: "Skill");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Location_LocationId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_RoleId",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "UserAccount");

            migrationBuilder.RenameIndex(
                name: "IX_User_RoleId",
                table: "UserAccount",
                newName: "IX_UserAccount_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_User_LocationId",
                table: "UserAccount",
                newName: "IX_UserAccount_LocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAccount",
                table: "UserAccount",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdviseRequest_UserAccount_UserId",
                table: "AdviseRequest",
                column: "UserId",
                principalTable: "UserAccount",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_UserAccount_UserId",
                table: "Blog",
                column: "UserId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChildrenProfile_UserAccount_UserId",
                table: "ChildrenProfile",
                column: "UserId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Class_UserAccount_UserId",
                table: "Class",
                column: "UserId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_UserAccount_UserId",
                table: "Contract",
                column: "UserId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DivisionUserAccount_UserAccount_UserAccountsId",
                table: "DivisionUserAccount",
                column: "UserAccountsId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_UserAccount_UserAccountId",
                table: "Enrollment",
                column: "UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_UserAccount_UserId",
                table: "Feedback",
                column: "UserId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationUser_UserAccount_UserId",
                table: "NotificationUser",
                column: "UserId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_UserAccount_UserId",
                table: "Order",
                column: "UserId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_UserAccount_UserAccountId",
                table: "Rating",
                column: "UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestUserAccount_UserAccount_RecieverId",
                table: "RequestUserAccount",
                column: "RecieverId",
                principalTable: "UserAccount",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_UserAccount_UserId",
                table: "Skill",
                column: "UserId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccount_Location_LocationId",
                table: "UserAccount",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccount_Role_RoleId",
                table: "UserAccount",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdviseRequest_UserAccount_UserId",
                table: "AdviseRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_Blog_UserAccount_UserId",
                table: "Blog");

            migrationBuilder.DropForeignKey(
                name: "FK_ChildrenProfile_UserAccount_UserId",
                table: "ChildrenProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_Class_UserAccount_UserId",
                table: "Class");

            migrationBuilder.DropForeignKey(
                name: "FK_Contract_UserAccount_UserId",
                table: "Contract");

            migrationBuilder.DropForeignKey(
                name: "FK_DivisionUserAccount_UserAccount_UserAccountsId",
                table: "DivisionUserAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_UserAccount_UserAccountId",
                table: "Enrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_UserAccount_UserId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationUser_UserAccount_UserId",
                table: "NotificationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_UserAccount_UserId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_UserAccount_UserAccountId",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestUserAccount_UserAccount_RecieverId",
                table: "RequestUserAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_UserAccount_UserId",
                table: "Skill");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAccount_Location_LocationId",
                table: "UserAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAccount_Role_RoleId",
                table: "UserAccount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAccount",
                table: "UserAccount");

            migrationBuilder.RenameTable(
                name: "UserAccount",
                newName: "User");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccount_RoleId",
                table: "User",
                newName: "IX_User_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccount_LocationId",
                table: "User",
                newName: "IX_User_LocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdviseRequest_User_UserId",
                table: "AdviseRequest",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_User_UserId",
                table: "Blog",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChildrenProfile_User_UserId",
                table: "ChildrenProfile",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Class_User_UserId",
                table: "Class",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_User_UserId",
                table: "Contract",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DivisionUserAccount_User_UserAccountsId",
                table: "DivisionUserAccount",
                column: "UserAccountsId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_User_UserAccountId",
                table: "Enrollment",
                column: "UserAccountId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_User_UserId",
                table: "Feedback",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationUser_User_UserId",
                table: "NotificationUser",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_UserId",
                table: "Order",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_User_UserAccountId",
                table: "Rating",
                column: "UserAccountId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestUserAccount_User_RecieverId",
                table: "RequestUserAccount",
                column: "RecieverId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_User_UserId",
                table: "Skill",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Location_LocationId",
                table: "User",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_RoleId",
                table: "User",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
