using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingRelationBookingAndAppointmentTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppointmentTimeId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AppointmentTimeId",
                table: "Bookings",
                column: "AppointmentTimeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AppointmentTimes_AppointmentTimeId",
                table: "Bookings",
                column: "AppointmentTimeId",
                principalTable: "AppointmentTimes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AppointmentTimes_AppointmentTimeId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_AppointmentTimeId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "AppointmentTimeId",
                table: "Bookings");
        }
    }
}
