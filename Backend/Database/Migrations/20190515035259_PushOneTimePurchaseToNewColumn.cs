using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class PushOneTimePurchaseToNewColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ItemStatuses",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.AddColumn<bool>(
                name: "OneTimePurchase",
                table: "GroupItems",
                nullable: false,
                defaultValueSql: "FALSE");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OneTimePurchase",
                table: "GroupItems");

            migrationBuilder.InsertData(
                table: "ItemStatuses",
                columns: new[] { "ID", "Name" },
                values: new object[] { 4, "One time purchase" });
        }
    }
}
