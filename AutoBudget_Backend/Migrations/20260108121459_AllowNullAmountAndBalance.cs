using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetingApi.Migrations
{
    /// <inheritdoc />
    public partial class AllowNullAmountAndBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("ALTER TABLE \"Transactions\" ALTER COLUMN \"StartedDate\" TYPE timestamp without time zone USING \"StartedDate\"::timestamp without time zone;");
            migrationBuilder.Sql("ALTER TABLE \"Transactions\" ALTER COLUMN \"CompletedDate\" TYPE timestamp without time zone USING \"CompletedDate\"::timestamp without time zone;");
            //migrationBuilder.AlterColumn<DateTime>(
                //name: "StartedDate",
                //table: "Transactions",
                //type: "timestamp without time zone",
                //nullable: true,
                //oldClrType: typeof(string),
                //oldType: "text",
                //oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Fee",
                table: "Transactions",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedDate",
                table: "Transactions",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Transactions",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Transactions",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StartedDate",
                table: "Transactions",
                type: "text",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Fee",
                table: "Transactions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompletedDate",
                table: "Transactions",
                type: "text",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Transactions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Transactions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);
        }
    }
}
