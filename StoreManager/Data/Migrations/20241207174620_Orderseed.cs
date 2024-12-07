using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StoreManager.Migrations
{
    /// <inheritdoc />
    public partial class Orderseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "ClientEmail", "ClientName", "OrderDate", "Status", "TotalAmount" },
                values: new object[,]
                {
                    { 1, "johndoe@example.com", "John Doe", new DateTime(2024, 12, 6, 17, 46, 19, 465, DateTimeKind.Local).AddTicks(4227), "Pending", 7.5m },
                    { 2, "janesmith@example.com", "Jane Smith", new DateTime(2024, 12, 5, 17, 46, 19, 465, DateTimeKind.Local).AddTicks(6176), "Confirmed", 3.0m }
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
