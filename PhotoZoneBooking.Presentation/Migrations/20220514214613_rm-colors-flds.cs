using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoZoneBooking.Presentation.Migrations
{
    /// <inheritdoc />
    public partial class rmcolorsflds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryColor",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "SecondaryColor",
                table: "Reservations");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e3333d0a-afd5-4b63-af81-076dd1bbb7ce",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b2d1168c-975f-450d-a46e-527742211553", "AQAAAAEAACcQAAAAEId6lE9yzGgqUyjWHkd5HQ85XM4oK/pljD0g5/AFfj1E7VcIHWcvnpjgynk1amKQGw==", "ce7b5b12-45d0-4ba6-8c25-fb632ee79e70" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrimaryColor",
                table: "Reservations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SecondaryColor",
                table: "Reservations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e3333d0a-afd5-4b63-af81-076dd1bbb7ce",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8f532099-bb09-4f87-b198-13cbaf8c70ff", "AQAAAAEAACcQAAAAEE+6SD8AKN1hfdYXl0vgi9FVlEWpI+ZcD8/9zvRZT2MRnNkTsEkLQGJBQg+s5u3bHg==", "0a536310-c670-4ab9-a2a6-8e223ef1e60d" });
        }
    }
}
