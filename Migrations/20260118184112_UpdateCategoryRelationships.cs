using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BudgetingApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategoryRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Accomadation");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 7, "Transport" },
                    { 8, "Savings" },
                    { 9, "Income" },
                    { 10, "Transfer" },
                    { 11, "Other" }
                });

            migrationBuilder.UpdateData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CategoryId", "Keyword" },
                values: new object[] { 1, "LIDL" });

            migrationBuilder.UpdateData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CategoryId", "Keyword" },
                values: new object[] { 1, "ALDI" });

            migrationBuilder.UpdateData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 5,
                column: "Keyword",
                value: "SAINSBURY'S");

            migrationBuilder.UpdateData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CategoryId", "Keyword" },
                values: new object[] { 1, "SUPERVALU" });

            migrationBuilder.InsertData(
                table: "CategoryRules",
                columns: new[] { "Id", "CategoryId", "Keyword" },
                values: new object[,]
                {
                    { 7, 1, "DUNNES STORES" },
                    { 8, 1, "CO-OP" },
                    { 9, 1, "CENTRA" },
                    { 10, 1, "ASDA" },
                    { 11, 2, "FITNESS" },
                    { 12, 2, "GYM" },
                    { 13, 5, "AMAZON" },
                    { 14, 3, "RENT" },
                    { 15, 4, "VODAFONE" },
                    { 16, 4, "EIR" },
                    { 17, 4, "TESCOMOBILE" },
                    { 18, 4, "THREE" },
                    { 19, 4, "VIRGINMEDIA" },
                    { 20, 6, "HOTEL" },
                    { 21, 6, "HOSTEL" },
                    { 22, 7, "BUS" },
                    { 23, 7, "TRANSLINK" },
                    { 24, 7, "RYANAIR" },
                    { 25, 7, "FERRY" },
                    { 26, 7, "UBER" },
                    { 27, 8, "SAVINGS" },
                    { 28, 9, "PAYMENT" },
                    { 29, 9, "TRANSFER FROM" },
                    { 30, 10, "TRANSFER TO" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Other");

            migrationBuilder.UpdateData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CategoryId", "Keyword" },
                values: new object[] { 2, "FITNESS" });

            migrationBuilder.UpdateData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CategoryId", "Keyword" },
                values: new object[] { 5, "AMAZON" });

            migrationBuilder.UpdateData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 5,
                column: "Keyword",
                value: "LIDL");

            migrationBuilder.UpdateData(
                table: "CategoryRules",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CategoryId", "Keyword" },
                values: new object[] { 2, "GYM" });
        }
    }
}
