using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class SeedItemStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ItemStatuses",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 3, "Out" },
                    { 1, "Okay" },
                    { 2, "Almost out" },
                    { 4, "One time purchase" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ItemStatuses",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ItemStatuses",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ItemStatuses",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ItemStatuses",
                keyColumn: "ID",
                keyValue: 4);
        }
    }
}
