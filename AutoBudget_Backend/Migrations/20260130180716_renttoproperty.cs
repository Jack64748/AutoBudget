using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetingApi.Migrations
{
    /// <inheritdoc />
    public partial class renttoproperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 14,
                column: "Keyword",
                value: "PROPERTY");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 14,
                column: "Keyword",
                value: "RENT");
        }
    }
}
