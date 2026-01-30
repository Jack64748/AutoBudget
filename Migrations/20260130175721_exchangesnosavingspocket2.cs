using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetingApi.Migrations
{
    /// <inheritdoc />
    public partial class exchangesnosavingspocket2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.InsertData(
                table: "CategoryRules",
                columns: new[] { "Id", "CategoryId", "Keyword" },
                values: new object[] { 31, 12, "POCKET" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.InsertData(
                table: "CategoryRules",
                columns: new[] { "Id", "CategoryId", "Keyword" },
                values: new object[] { 32, 12, "POCKET" });
        }
    }
}
