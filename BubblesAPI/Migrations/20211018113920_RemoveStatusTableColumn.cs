using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace BubblesAPI.Migrations
{
    public partial class RemoveStatusTableColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Status_UserStatusId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserStatusId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserStatusId",
                table: "Users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(2021, 10, 18, 17, 9, 20, 559, DateTimeKind.Local).AddTicks(6800),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2021, 9, 15, 16, 17, 4, 510, DateTimeKind.Local).AddTicks(2090));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Credentials",
                nullable: false,
                defaultValue: new DateTime(2021, 10, 18, 17, 9, 20, 663, DateTimeKind.Local).AddTicks(5200),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2021, 9, 15, 16, 17, 4, 617, DateTimeKind.Local).AddTicks(7380));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2021, 9, 15, 16, 17, 4, 510, DateTimeKind.Local).AddTicks(2090),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 10, 18, 17, 9, 20, 559, DateTimeKind.Local).AddTicks(6800));

            migrationBuilder.AddColumn<int>(
                name: "UserStatusId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Credentials",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2021, 9, 15, 16, 17, 4, 617, DateTimeKind.Local).AddTicks(7380),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 10, 18, 17, 9, 20, 663, DateTimeKind.Local).AddTicks(5200));

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    StatusName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserStatusId",
                table: "Users",
                column: "UserStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Status_UserStatusId",
                table: "Users",
                column: "UserStatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
