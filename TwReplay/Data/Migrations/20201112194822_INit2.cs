using Microsoft.EntityFrameworkCore.Migrations;

namespace TwReplay.Data.Migrations
{
    public partial class INit2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clips_ClipInfo_ClipInfoId",
                table: "Clips");

            migrationBuilder.DropIndex(
                name: "IX_Clips_ClipInfoId",
                table: "Clips");

            migrationBuilder.DropColumn(
                name: "ClipInfoId",
                table: "Clips");

            migrationBuilder.AddColumn<string>(
                name: "ClipItemId",
                table: "ClipInfo",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClipInfo_ClipItemId",
                table: "ClipInfo",
                column: "ClipItemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClipInfo_Clips_ClipItemId",
                table: "ClipInfo",
                column: "ClipItemId",
                principalTable: "Clips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClipInfo_Clips_ClipItemId",
                table: "ClipInfo");

            migrationBuilder.DropIndex(
                name: "IX_ClipInfo_ClipItemId",
                table: "ClipInfo");

            migrationBuilder.DropColumn(
                name: "ClipItemId",
                table: "ClipInfo");

            migrationBuilder.AddColumn<string>(
                name: "ClipInfoId",
                table: "Clips",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clips_ClipInfoId",
                table: "Clips",
                column: "ClipInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clips_ClipInfo_ClipInfoId",
                table: "Clips",
                column: "ClipInfoId",
                principalTable: "ClipInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
