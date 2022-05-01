using Microsoft.EntityFrameworkCore.Migrations;

namespace LobbyServer.Migrations
{
    public partial class custermization_info : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FaceType",
                table: "Player",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HairType",
                table: "Player",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JacketType",
                table: "Player",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaceType",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "HairType",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "JacketType",
                table: "Player");
        }
    }
}
