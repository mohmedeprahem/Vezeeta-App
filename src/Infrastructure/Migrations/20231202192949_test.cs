using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "Gender", "Image", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "SpecializationId", "TwoFactorEnabled", "UserName" },
                values: new object[] { "a3a82e8e-6043-4084-b4ce-f6f87e3e1ef5", 0, "a4f950c0-2067-4443-bf5a-f54c6c540b8d", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@localhost", true, "mohamed", "Male", null, "ibrahem", false, null, null, null, "admin", null, false, "259bb56f-4605-4ff8-932e-151c59a0a29c", null, false, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a3a82e8e-6043-4084-b4ce-f6f87e3e1ef5");
        }
    }
}
