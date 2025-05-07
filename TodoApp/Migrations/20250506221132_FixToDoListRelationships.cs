using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApp.Migrations
{
    /// <inheritdoc />
    public partial class FixToDoListRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_ToDoLists_ToDoListId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ToDoListId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ToDoListId",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastModified" },
                values: new object[] { new DateTime(2025, 5, 7, 1, 11, 32, 6, DateTimeKind.Local).AddTicks(758), new DateTime(2025, 5, 7, 1, 11, 32, 6, DateTimeKind.Local).AddTicks(814) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ToDoListId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastModified", "ToDoListId" },
                values: new object[] { new DateTime(2025, 5, 7, 0, 44, 41, 775, DateTimeKind.Local).AddTicks(7121), new DateTime(2025, 5, 7, 0, 44, 41, 775, DateTimeKind.Local).AddTicks(7168), null });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ToDoListId",
                table: "Users",
                column: "ToDoListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ToDoLists_ToDoListId",
                table: "Users",
                column: "ToDoListId",
                principalTable: "ToDoLists",
                principalColumn: "Id");
        }
    }
}
