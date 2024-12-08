using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManager.Migrations
{
    /// <inheritdoc />
    public partial class Orders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2024, 12, 7, 21, 50, 14, 743, DateTimeKind.Local).AddTicks(3456));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 2,
                column: "OrderDate",
                value: new DateTime(2024, 12, 6, 21, 50, 14, 743, DateTimeKind.Local).AddTicks(5172));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2024, 12, 6, 17, 48, 25, 11, DateTimeKind.Local).AddTicks(9722));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 2,
                column: "OrderDate",
                value: new DateTime(2024, 12, 5, 17, 48, 25, 12, DateTimeKind.Local).AddTicks(1046));
        }
    }
}
