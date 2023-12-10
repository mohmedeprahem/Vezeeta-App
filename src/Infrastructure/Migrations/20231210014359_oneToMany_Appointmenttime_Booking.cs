using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class oneToMany_Appointmenttime_Booking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_AppointmentTimeId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AppointmentTimeId",
                table: "Bookings",
                column: "AppointmentTimeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_AppointmentTimeId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AppointmentTimeId",
                table: "Bookings",
                column: "AppointmentTimeId",
                unique: true);
        }
    }
}
