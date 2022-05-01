using Microsoft.EntityFrameworkCore.Migrations;

namespace LobbyServer.Migrations
{
    public partial class custermization_color : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "FaceColor_X",
                table: "Player",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "FaceColor_Y",
                table: "Player",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "FaceColor_Z",
                table: "Player",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "HairColor",
                table: "Player",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaceColor_X",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "FaceColor_Y",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "FaceColor_Z",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "HairColor",
                table: "Player");
        }
    }
}
