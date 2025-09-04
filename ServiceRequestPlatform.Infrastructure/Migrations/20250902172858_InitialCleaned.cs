using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceRequestPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCleaned : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$9Vz815h5ZgERj.t/t63Dl.xhbH.6KMEcKmsazQQGpKZrmYbalwFLa");

            migrationBuilder.UpdateData(
                table: "AvailabilitySlots",
                keyColumn: "Id",
                keyValue: 1,
                column: "AvailableDate",
                value: new DateTime(2025, 9, 3, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 2, 17, 28, 57, 950, DateTimeKind.Utc).AddTicks(1799), "$2a$11$ZxIbZkm.hzlHstXHSHEPP.idAHDrzVMxyOsxgU8BguVORzaZX/0Nu" });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 2, 17, 28, 58, 87, DateTimeKind.Utc).AddTicks(1028));

            migrationBuilder.UpdateData(
                table: "Workers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 2, 17, 28, 58, 87, DateTimeKind.Utc).AddTicks(632), "$2a$11$XxSt288QyN3e9SvtPZ3EuesuXaWfvDesCtgYlVzvczCNVnyIPachm" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "admin123");

            migrationBuilder.UpdateData(
                table: "AvailabilitySlots",
                keyColumn: "Id",
                keyValue: 1,
                column: "AvailableDate",
                value: new DateTime(2025, 8, 26, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 8, 25, 16, 9, 43, 578, DateTimeKind.Utc).AddTicks(5358), "customer123" });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 25, 16, 9, 43, 578, DateTimeKind.Utc).AddTicks(5406));

            migrationBuilder.UpdateData(
                table: "Workers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 8, 25, 16, 9, 43, 578, DateTimeKind.Utc).AddTicks(5380), "worker123" });
        }
    }
}
