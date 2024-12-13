using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class RemoveItemStatusAsKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupItems",
                table: "GroupItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupItems",
                table: "GroupItems",
                columns: new[] { "GroupID", "ItemID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupItems",
                table: "GroupItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupItems",
                table: "GroupItems",
                columns: new[] { "GroupID", "ItemID", "ItemStatusID" });
        }
    }
}
