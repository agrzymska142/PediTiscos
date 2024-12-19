using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManager.Migrations
{
    /// <inheritdoc />
    public partial class Migration1812 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2024, 12, 15, 15, 13, 23, 299, DateTimeKind.Local).AddTicks(2061));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 2,
                column: "OrderDate",
                value: new DateTime(2024, 12, 14, 15, 13, 23, 299, DateTimeKind.Local).AddTicks(3660));
        }
    }
}
