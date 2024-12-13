using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class RenameOkayToStocked : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ItemStatuses",
                keyColumn: "ID",
                keyValue: 1,
                column: "Name",
                value: "Stocked");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ItemStatuses",
                keyColumn: "ID",
                keyValue: 1,
                column: "Name",
                value: "Okay");
        }
    }
}
