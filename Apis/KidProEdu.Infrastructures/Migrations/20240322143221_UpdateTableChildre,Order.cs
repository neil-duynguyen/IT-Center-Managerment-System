﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidProEdu.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableChildreOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Order_OrderId",
                table: "OrderDetail");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDay",
                table: "ChildrenProfile",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Order_OrderId",
                table: "OrderDetail",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Order_OrderId",
                table: "OrderDetail");

            migrationBuilder.AlterColumn<string>(
                name: "BirthDay",
                table: "ChildrenProfile",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Order_OrderId",
                table: "OrderDetail",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");
        }
    }
}
