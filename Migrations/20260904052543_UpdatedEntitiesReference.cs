using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace medii_si_platforme_de_dezvoltare_avansate_events.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedEntitiesReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EventRegistrations",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegistrationDate",
                value: new DateTime(2026, 9, 2, 5, 25, 42, 820, DateTimeKind.Utc).AddTicks(8730));

            migrationBuilder.UpdateData(
                table: "EventRegistrations",
                keyColumn: "Id",
                keyValue: 2,
                column: "RegistrationDate",
                value: new DateTime(2026, 9, 3, 5, 25, 42, 820, DateTimeKind.Utc).AddTicks(8730));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "RegistrationDeadline", "StartDate" },
                values: new object[] { new DateTime(2026, 9, 4, 5, 25, 42, 820, DateTimeKind.Utc).AddTicks(8730), new DateTime(2026, 9, 11, 5, 25, 42, 820, DateTimeKind.Utc).AddTicks(8730), new DateTime(2026, 9, 14, 5, 25, 42, 820, DateTimeKind.Utc).AddTicks(8730) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "RegistrationDeadline", "StartDate" },
                values: new object[] { new DateTime(2026, 9, 4, 5, 25, 42, 820, DateTimeKind.Utc).AddTicks(8730), new DateTime(2026, 9, 19, 5, 25, 42, 820, DateTimeKind.Utc).AddTicks(8730), new DateTime(2026, 9, 24, 5, 25, 42, 820, DateTimeKind.Utc).AddTicks(8730) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "RegistrationDeadline", "StartDate" },
                values: new object[] { new DateTime(2026, 9, 4, 5, 25, 42, 820, DateTimeKind.Utc).AddTicks(8730), new DateTime(2026, 9, 29, 5, 25, 42, 820, DateTimeKind.Utc).AddTicks(8730), new DateTime(2026, 10, 4, 5, 25, 42, 820, DateTimeKind.Utc).AddTicks(8730) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "RegistrationDeadline", "StartDate" },
                values: new object[] { new DateTime(2026, 9, 4, 5, 25, 42, 820, DateTimeKind.Utc).AddTicks(8730), new DateTime(2026, 9, 16, 5, 25, 42, 820, DateTimeKind.Utc).AddTicks(8730), new DateTime(2026, 9, 19, 5, 25, 42, 820, DateTimeKind.Utc).AddTicks(8730) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 4, 5, 25, 42, 820, DateTimeKind.Utc).AddTicks(8660));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 4, 5, 25, 42, 820, DateTimeKind.Utc).AddTicks(8660));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EventRegistrations",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegistrationDate",
                value: new DateTime(2026, 9, 2, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580));

            migrationBuilder.UpdateData(
                table: "EventRegistrations",
                keyColumn: "Id",
                keyValue: 2,
                column: "RegistrationDate",
                value: new DateTime(2026, 9, 3, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "RegistrationDeadline", "StartDate" },
                values: new object[] { new DateTime(2026, 9, 4, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580), new DateTime(2026, 9, 11, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580), new DateTime(2026, 9, 14, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "RegistrationDeadline", "StartDate" },
                values: new object[] { new DateTime(2026, 9, 4, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580), new DateTime(2026, 9, 19, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580), new DateTime(2026, 9, 24, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "RegistrationDeadline", "StartDate" },
                values: new object[] { new DateTime(2026, 9, 4, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580), new DateTime(2026, 9, 29, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580), new DateTime(2026, 10, 4, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "RegistrationDeadline", "StartDate" },
                values: new object[] { new DateTime(2026, 9, 4, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580), new DateTime(2026, 9, 16, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580), new DateTime(2026, 9, 19, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 4, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3520));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 4, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3520));
        }
    }
}
