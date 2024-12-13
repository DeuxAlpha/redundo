using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class AddGroupOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerID",
                table: "Groups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_OwnerID",
                table: "Groups",
                column: "OwnerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_OwnerID",
                table: "Groups",
                column: "OwnerID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_OwnerID",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_OwnerID",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Groups");
        }
    }
}
