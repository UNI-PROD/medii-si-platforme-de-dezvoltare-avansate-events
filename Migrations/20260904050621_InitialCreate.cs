using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace medii_si_platforme_de_dezvoltare_avansate_events.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RegistrationDeadline = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MaxParticipants = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventRegistrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventRegistrations_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventRegistrations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "IsActive", "Role" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 9, 4, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3520), "admin@sibiuevents.com", "Administrator", true, 1 },
                    { 2, new DateTime(2026, 9, 4, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3520), "user@sibiuevents.com", "John Doe", true, 0 }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "CreatedAt", "CreatedByUserId", "Description", "IsActive", "Location", "MaxParticipants", "RegistrationDeadline", "StartDate", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 9, 4, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580), 1, "Învață tehnici avansate de ASP.NET Core, inclusiv Entity Framework și design patterns.", true, "Sibiu - Centrul Cultural", 30, new DateTime(2026, 9, 11, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580), new DateTime(2026, 9, 14, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580), "Workshop: ASP.NET Core Avançat", null },
                    { 2, new DateTime(2026, 9, 4, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580), 1, "Descoperă tendințele și best practices în cloud computing.", true, "Sibiu - Săli de conferințe", 50, new DateTime(2026, 9, 19, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580), new DateTime(2026, 9, 24, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580), "Conferință: Cloud Computing în 2025", null },
                    { 3, new DateTime(2026, 9, 4, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580), 1, "Participă la competiția de programare cu premii valoroase.", true, "Sibiu - Spaț de lucru comun", 20, new DateTime(2026, 9, 29, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580), new DateTime(2026, 10, 4, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580), "Hackathon: Sibiu Tech Challenge", null },
                    { 4, new DateTime(2026, 9, 4, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580), 1, "Reuni-te cu alți developeri C# și discută proiecte interesante.", true, "Sibiu - Cafeneaua Tech", 40, new DateTime(2026, 9, 16, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580), new DateTime(2026, 9, 19, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580), "Meetup: C# Developers", null }
                });

            migrationBuilder.InsertData(
                table: "EventRegistrations",
                columns: new[] { "Id", "EventId", "RegistrationDate", "Status", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 9, 2, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580), 0, 2 },
                    { 2, 2, new DateTime(2026, 9, 3, 5, 6, 20, 882, DateTimeKind.Utc).AddTicks(3580), 0, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistrations_EventId",
                table: "EventRegistrations",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistrations_UserId_EventId",
                table: "EventRegistrations",
                columns: new[] { "UserId", "EventId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_CreatedByUserId",
                table: "Events",
                column: "CreatedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventRegistrations");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
