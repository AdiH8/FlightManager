using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightManager.Data.Migrations
{
    public partial class database : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeavingFrom = table.Column<string>(nullable: true),
                    GoingTo = table.Column<string>(nullable: true),
                    AirplaneType = table.Column<string>(nullable: true),
                    PassengersCapacity = table.Column<int>(nullable: false),
                    TicketsLeft = table.Column<int>(nullable: false),
                    BusinessClassCapacity = table.Column<int>(nullable: false),
                    BusinessTicketsLeft = table.Column<int>(nullable: false),
                    PlaneId = table.Column<int>(nullable: false),
                    Departure = table.Column<DateTime>(nullable: false),
                    Arrival = table.Column<DateTime>(nullable: false),
                    PilotName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlightBookings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Nationality = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    SSN = table.Column<string>(nullable: true),
                    FlightId = table.Column<int>(nullable: true),
                    TicketType = table.Column<string>(nullable: true),
                    IsConfirmed = table.Column<bool>(nullable: false),
                    TicketsCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlightBookings_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });


            migrationBuilder.CreateIndex(
                name: "IX_FlightBookings_FlightId",
                table: "FlightBookings",
                column: "FlightId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightBookings");

            migrationBuilder.DropTable(
                name: "Flights");
        }
    }
}
