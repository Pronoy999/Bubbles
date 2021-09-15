using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace BubblesAPI.Migrations
{
    public partial class CredentialsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(2021, 9, 15, 16, 17, 4, 510, DateTimeKind.Local).AddTicks(2090),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2021, 9, 15, 12, 41, 42, 625, DateTimeKind.Local).AddTicks(7450));

            migrationBuilder.CreateTable(
                name: "Credentials",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    Created = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2021, 9, 15, 16, 17, 4, 617, DateTimeKind.Local).AddTicks(7380))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Credentials_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Credentials_UserId",
                table: "Credentials",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Credentials");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2021, 9, 15, 12, 41, 42, 625, DateTimeKind.Local).AddTicks(7450),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 9, 15, 16, 17, 4, 510, DateTimeKind.Local).AddTicks(2090));
        }
    }
}
