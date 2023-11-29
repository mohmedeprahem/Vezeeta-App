using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingAppointmentDayEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DayId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AppointmentDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentDays", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DayId",
                table: "Appointments",
                column: "DayId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AppointmentDays_DayId",
                table: "Appointments",
                column: "DayId",
                principalTable: "AppointmentDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AppointmentDays_DayId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "AppointmentDays");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_DayId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "DayId",
                table: "Appointments");
        }
    }
}
