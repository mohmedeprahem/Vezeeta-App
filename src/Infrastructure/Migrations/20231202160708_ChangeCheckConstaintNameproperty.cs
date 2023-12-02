using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCheckConstaintNameproperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_AspNetUsers_Gender1",
                table: "bookingStatuses");

            migrationBuilder.AddCheckConstraint(
                name: "CK_BookingStatuses_Name",
                table: "bookingStatuses",
                sql: "Name IN ('Binding', 'Completed', 'Cancelled')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_BookingStatuses_Name",
                table: "bookingStatuses");

            migrationBuilder.AddCheckConstraint(
                name: "CK_AspNetUsers_Gender1",
                table: "bookingStatuses",
                sql: "Name IN ('Binding', 'Completed', 'Cancelled')");
        }
    }
}
