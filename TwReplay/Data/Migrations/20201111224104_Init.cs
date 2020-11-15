using Microsoft.EntityFrameworkCore.Migrations;

namespace TwReplay.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClipInfo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Slug = table.Column<string>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClipInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clips",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ClipInfoId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clips_ClipInfo_ClipInfoId",
                        column: x => x.ClipInfoId,
                        principalTable: "ClipInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClipLinkInfo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Provider = table.Column<string>(type: "TEXT", nullable: true),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    ClipItemId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClipLinkInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClipLinkInfo_Clips_ClipItemId",
                        column: x => x.ClipItemId,
                        principalTable: "Clips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClipLinkInfo_ClipItemId",
                table: "ClipLinkInfo",
                column: "ClipItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Clips_ClipInfoId",
                table: "Clips",
                column: "ClipInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClipLinkInfo");

            migrationBuilder.DropTable(
                name: "Clips");

            migrationBuilder.DropTable(
                name: "ClipInfo");
        }
    }
}
