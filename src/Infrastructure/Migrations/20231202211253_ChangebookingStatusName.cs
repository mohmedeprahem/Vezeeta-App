using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangebookingStatusName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_bookingStatuses_BookingStatusId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bookingStatuses",
                table: "bookingStatuses");

            migrationBuilder.RenameTable(
                name: "bookingStatuses",
                newName: "BookingStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingStatuses",
                table: "BookingStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_BookingStatuses_BookingStatusId",
                table: "Bookings",
                column: "BookingStatusId",
                principalTable: "BookingStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_BookingStatuses_BookingStatusId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingStatuses",
                table: "BookingStatuses");

            migrationBuilder.RenameTable(
                name: "BookingStatuses",
                newName: "bookingStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bookingStatuses",
                table: "bookingStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_bookingStatuses_BookingStatusId",
                table: "Bookings",
                column: "BookingStatusId",
                principalTable: "bookingStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
