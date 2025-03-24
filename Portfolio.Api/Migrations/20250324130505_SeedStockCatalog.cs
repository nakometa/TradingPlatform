using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Portfolio.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedStockCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "StockCatalogs",
                columns: new[] { "Id", "Ticker" },
                values: new object[,]
                {
                    { 1, "AAPL" },
                    { 2, "TSLA" },
                    { 3, "NVDA" },
                    { 4, "AMZN" },
                    { 5, "GOOGL" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StockCatalogs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "StockCatalogs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "StockCatalogs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "StockCatalogs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "StockCatalogs",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
