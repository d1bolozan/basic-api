using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class CommentOneToOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3de26b4d-a170-4b00-864a-48877a59bc08");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c0469f45-6d40-4d71-b0d5-d0b2371b5e0b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5f121f2e-8b4e-4539-a8a3-3ab8b571061b", null, "User", "USER" },
                    { "e0745547-72d3-4cfa-a5c1-bd57dbfa3f22", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5f121f2e-8b4e-4539-a8a3-3ab8b571061b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e0745547-72d3-4cfa-a5c1-bd57dbfa3f22");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3de26b4d-a170-4b00-864a-48877a59bc08", null, "User", "USER" },
                    { "c0469f45-6d40-4d71-b0d5-d0b2371b5e0b", null, "Admin", "ADMIN" }
                });
        }
    }
}
