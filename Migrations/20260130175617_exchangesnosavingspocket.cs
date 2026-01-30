using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetingApi.Migrations
{
    /// <inheritdoc />
    public partial class exchangesnosavingspocket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Accommodation");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Exchanges");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[] { 12, "Pockets" });

            migrationBuilder.UpdateData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 27,
                column: "Keyword",
                value: "EXCHANGED");

            migrationBuilder.InsertData(
                table: "CategoryRules",
                columns: new[] { "Id", "CategoryId", "Keyword" },
                values: new object[] { 32, 12, "POCKET" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Accomadation");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Savings");

            migrationBuilder.UpdateData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 27,
                column: "Keyword",
                value: "SAVINGS");
        }
    }
}
