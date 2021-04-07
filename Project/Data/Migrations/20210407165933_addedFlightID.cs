using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightManager.Data.Migrations
{
    public partial class addedFlightID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlightBookings_Flights_FlightId",
                table: "FlightBookings");

            migrationBuilder.RenameColumn(
                name: "FlightId",
                table: "FlightBookings",
                newName: "FlightID");

            migrationBuilder.RenameIndex(
                name: "IX_FlightBookings_FlightId",
                table: "FlightBookings",
                newName: "IX_FlightBookings_FlightID");

            migrationBuilder.AlterColumn<int>(
                name: "FlightID",
                table: "FlightBookings",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FlightBookings_Flights_FlightID",
                table: "FlightBookings",
                column: "FlightID",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlightBookings_Flights_FlightID",
                table: "FlightBookings");

            migrationBuilder.RenameColumn(
                name: "FlightID",
                table: "FlightBookings",
                newName: "FlightId");

            migrationBuilder.RenameIndex(
                name: "IX_FlightBookings_FlightID",
                table: "FlightBookings",
                newName: "IX_FlightBookings_FlightId");

            migrationBuilder.AlterColumn<int>(
                name: "FlightId",
                table: "FlightBookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_FlightBookings_Flights_FlightId",
                table: "FlightBookings",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
