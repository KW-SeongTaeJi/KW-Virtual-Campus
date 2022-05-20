using Microsoft.EntityFrameworkCore.Migrations;

namespace LobbyServer.Migrations
{
    public partial class add_Friends_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FriendRelation",
                columns: table => new
                {
                    FriendRelationDbId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeDbId = table.Column<int>(type: "int", nullable: false),
                    FriendDbId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRelation", x => x.FriendRelationDbId);
                    table.ForeignKey(
                        name: "FK_FriendRelation_Player_FriendDbId",
                        column: x => x.FriendDbId,
                        principalTable: "Player",
                        principalColumn: "PlayerDbId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendRelation_Player_MeDbId",
                        column: x => x.MeDbId,
                        principalTable: "Player",
                        principalColumn: "PlayerDbId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FriendRequest",
                columns: table => new
                {
                    FriendRequestDbId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromDbId = table.Column<int>(type: "int", nullable: false),
                    ToDbId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequest", x => x.FriendRequestDbId);
                    table.ForeignKey(
                        name: "FK_FriendRequest_Player_FromDbId",
                        column: x => x.FromDbId,
                        principalTable: "Player",
                        principalColumn: "PlayerDbId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendRequest_Player_ToDbId",
                        column: x => x.ToDbId,
                        principalTable: "Player",
                        principalColumn: "PlayerDbId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendRelation_FriendDbId",
                table: "FriendRelation",
                column: "FriendDbId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRelation_MeDbId",
                table: "FriendRelation",
                column: "MeDbId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequest_FromDbId",
                table: "FriendRequest",
                column: "FromDbId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequest_ToDbId",
                table: "FriendRequest",
                column: "ToDbId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendRelation");

            migrationBuilder.DropTable(
                name: "FriendRequest");
        }
    }
}
