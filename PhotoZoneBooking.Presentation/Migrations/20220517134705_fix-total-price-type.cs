using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoZoneBooking.Presentation.Migrations
{
    /// <inheritdoc />
    public partial class fixtotalpricetype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Total",
                table: "Reservations",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e3333d0a-afd5-4b63-af81-076dd1bbb7ce",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2970999a-23ba-41d4-b78d-0a1aaa9f3dee", "AQAAAAEAACcQAAAAEPERfUrt2zSVrJD2M/GcetYIBcfYsjDP/0n3iIFwZmd23CU2OjbFhwiGyNyG/bEwQg==", "90bfa122-b9d3-4d29-970d-2e730065620e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Total",
                table: "Reservations",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e3333d0a-afd5-4b63-af81-076dd1bbb7ce",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b2d1168c-975f-450d-a46e-527742211553", "AQAAAAEAACcQAAAAEId6lE9yzGgqUyjWHkd5HQ85XM4oK/pljD0g5/AFfj1E7VcIHWcvnpjgynk1amKQGw==", "ce7b5b12-45d0-4ba6-8c25-fb632ee79e70" });
        }
    }
}
