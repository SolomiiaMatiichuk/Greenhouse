using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoZoneBooking.Presentation.Migrations
{
    /// <inheritdoc />
    public partial class resstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Reservations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e3333d0a-afd5-4b63-af81-076dd1bbb7ce",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8f532099-bb09-4f87-b198-13cbaf8c70ff", "ADMIN@GMAIL.COM", "AQAAAAEAACcQAAAAEE+6SD8AKN1hfdYXl0vgi9FVlEWpI+ZcD8/9zvRZT2MRnNkTsEkLQGJBQg+s5u3bHg==", "0a536310-c670-4ab9-a2a6-8e223ef1e60d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Reservations");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e3333d0a-afd5-4b63-af81-076dd1bbb7ce",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b55777b2-aa54-443f-b5c3-89190c88359c", null, "AQAAAAEAACcQAAAAEN20YnQa/Dv+25O/F2SHeh5SmhhZ13HI0MdYLYUwQ57wdXIacObpEw8WAekvMZiVHA==", "d8a2d177-db2e-4259-9dc2-889cdafd44e9" });
        }
    }
}
