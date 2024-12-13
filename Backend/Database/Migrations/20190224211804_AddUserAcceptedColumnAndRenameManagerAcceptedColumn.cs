using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class AddUserAcceptedColumnAndRenameManagerAcceptedColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAccepted",
                table: "UserGroups",
                newName: "UserAccepted");

            migrationBuilder.AddColumn<bool>(
                name: "ManagerAccepted",
                table: "UserGroups",
                nullable: false,
                defaultValueSql: "FALSE");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManagerAccepted",
                table: "UserGroups");

            migrationBuilder.RenameColumn(
                name: "UserAccepted",
                table: "UserGroups",
                newName: "IsAccepted");
        }
    }
}
