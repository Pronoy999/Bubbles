using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BubblesAPI.Migrations
{
    public partial class ChangeCascadeMode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credentials_Users_Email",
                table: "Credentials");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(2021, 10, 18, 19, 8, 11, 59, DateTimeKind.Local).AddTicks(350),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2021, 10, 18, 18, 50, 18, 651, DateTimeKind.Local).AddTicks(9940));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Credentials",
                nullable: false,
                defaultValue: new DateTime(2021, 10, 18, 19, 8, 11, 172, DateTimeKind.Local).AddTicks(400),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2021, 10, 18, 18, 50, 18, 765, DateTimeKind.Local).AddTicks(9090));

            migrationBuilder.AddForeignKey(
                name: "FK_Credentials_Users_Email",
                table: "Credentials",
                column: "Email",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credentials_Users_Email",
                table: "Credentials");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2021, 10, 18, 18, 50, 18, 651, DateTimeKind.Local).AddTicks(9940),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 10, 18, 19, 8, 11, 59, DateTimeKind.Local).AddTicks(350));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Credentials",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2021, 10, 18, 18, 50, 18, 765, DateTimeKind.Local).AddTicks(9090),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 10, 18, 19, 8, 11, 172, DateTimeKind.Local).AddTicks(400));

            migrationBuilder.AddForeignKey(
                name: "FK_Credentials_Users_Email",
                table: "Credentials",
                column: "Email",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
