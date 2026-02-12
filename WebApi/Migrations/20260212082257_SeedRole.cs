using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "id", "concurrency_stamp", "name", "normalized_name" },
                values: new object[,]
                {
                    { "6fe435ed-aff5-4839-ba23-7ee86d5b8f63", "410729bb-a13c-4d03-bf90-6e08401d1d3c", "User", "USER" },
                    { "8da0a14b-5e27-45c1-a2fe-deeb487be29e", "086b18d2-3aa2-470f-bb17-e37932559ccc", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "6fe435ed-aff5-4839-ba23-7ee86d5b8f63");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "8da0a14b-5e27-45c1-a2fe-deeb487be29e");
        }
    }
}
