using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManager.Migrations
{
    /// <inheritdoc />
    public partial class AddCartItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2024, 12, 21, 23, 56, 50, 621, DateTimeKind.Local).AddTicks(7415));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 2,
                column: "OrderDate",
                value: new DateTime(2024, 12, 20, 23, 56, 50, 621, DateTimeKind.Local).AddTicks(8439));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2024, 12, 17, 22, 12, 11, 157, DateTimeKind.Local).AddTicks(5159));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 2,
                column: "OrderDate",
                value: new DateTime(2024, 12, 16, 22, 12, 11, 157, DateTimeKind.Local).AddTicks(6601));
        }
    }
}
