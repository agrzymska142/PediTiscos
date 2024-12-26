using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StoreManager.Migrations
{
    /// <inheritdoc />
    public partial class NewData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 2);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Description", "Name" },
                values: new object[,]
                {
                    { 3, "Fresh fruits", "Fruits" },
                    { 4, "Fresh vegetables", "Vegetables" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryId", "Description", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 3, 3, "Fresh apple", "Apple", 0.5m, 200 },
                    { 4, 4, "Fresh carrot", "Carrot", 0.3m, 150 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4);

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "ClientEmail", "ClientName", "OrderDate", "Status", "TotalAmount" },
                values: new object[,]
                {
                    { 1, "johndoe@example.com", "John Doe", new DateTime(2024, 12, 22, 23, 22, 13, 74, DateTimeKind.Local).AddTicks(8576), "Pending", 7.5m },
                    { 2, "janesmith@example.com", "Jane Smith", new DateTime(2024, 12, 21, 23, 22, 13, 75, DateTimeKind.Local).AddTicks(278), "Confirmed", 3.0m }
                });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "OrderDetailId", "OrderId", "ProductId", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 1, 1, 2, 1.5m },
                    { 2, 1, 2, 1, 2.0m },
                    { 3, 2, 2, 1, 2.0m }
                });
        }
    }
}
