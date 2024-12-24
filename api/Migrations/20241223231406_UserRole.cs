using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class UserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "39cbd529-73a7-4b07-ad20-2a21e8d8a477", "09720327-aeab-4f04-bbcc-2e18934900b0", "User", "USER" },
                    { "5630c7dd-affc-4b39-8958-324517971641", "bcf54a32-87b7-407a-a654-d78879cbea4f", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39cbd529-73a7-4b07-ad20-2a21e8d8a477");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5630c7dd-affc-4b39-8958-324517971641");
        }
    }
}
