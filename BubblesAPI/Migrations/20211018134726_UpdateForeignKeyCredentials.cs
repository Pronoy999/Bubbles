using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BubblesAPI.Migrations
{
    public partial class UpdateForeignKeyCredentials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credentials_Users_Email",
                table: "Credentials");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(2021, 10, 18, 19, 17, 26, 678, DateTimeKind.Local).AddTicks(6760),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2021, 10, 18, 19, 8, 11, 59, DateTimeKind.Local).AddTicks(350));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Credentials",
                nullable: false,
                defaultValue: new DateTime(2021, 10, 18, 19, 17, 26, 795, DateTimeKind.Local).AddTicks(1200),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2021, 10, 18, 19, 8, 11, 172, DateTimeKind.Local).AddTicks(400));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_Email",
                table: "Users",
                column: "Email");

            migrationBuilder.AddForeignKey(
                name: "FK_Credentials_Users_Email",
                table: "Credentials",
                column: "Email",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credentials_Users_Email",
                table: "Credentials");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_Email",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2021, 10, 18, 19, 8, 11, 59, DateTimeKind.Local).AddTicks(350),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 10, 18, 19, 17, 26, 678, DateTimeKind.Local).AddTicks(6760));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Credentials",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2021, 10, 18, 19, 8, 11, 172, DateTimeKind.Local).AddTicks(400),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 10, 18, 19, 17, 26, 795, DateTimeKind.Local).AddTicks(1200));

            migrationBuilder.AddForeignKey(
                name: "FK_Credentials_Users_Email",
                table: "Credentials",
                column: "Email",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
