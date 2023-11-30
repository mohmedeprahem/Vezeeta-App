using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAppointmentDayToDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AppointmentDays_DayId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "AppointmentDays");

            migrationBuilder.CreateTable(
                name: "Days",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Days", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Days_DayId",
                table: "Appointments",
                column: "DayId",
                principalTable: "Days",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Days_DayId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "Days");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AppointmentDays_DayId",
                table: "Appointments",
                column: "DayId",
                principalTable: "AppointmentDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
