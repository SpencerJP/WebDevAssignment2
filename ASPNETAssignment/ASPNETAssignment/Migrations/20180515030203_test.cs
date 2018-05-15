using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ASPNETAssignment.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<int>(
                name: "StoreID",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StoreID",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_StoreID",
                table: "AspNetUsers",
                column: "StoreID");
        }
    }
}
