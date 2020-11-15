using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TwReplay.Data.Migrations
{
    public partial class AddData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClipInfo_Clips_ClipItemId",
                table: "ClipInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_ClipLinkInfo_Clips_ClipItemId",
                table: "ClipLinkInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClipLinkInfo",
                table: "ClipLinkInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClipInfo",
                table: "ClipInfo");

            migrationBuilder.RenameTable(
                name: "ClipLinkInfo",
                newName: "ClipLinkInfos");

            migrationBuilder.RenameTable(
                name: "ClipInfo",
                newName: "ClipInfos");

            migrationBuilder.RenameIndex(
                name: "IX_ClipLinkInfo_ClipItemId",
                table: "ClipLinkInfos",
                newName: "IX_ClipLinkInfos_ClipItemId");

            migrationBuilder.RenameIndex(
                name: "IX_ClipInfo_ClipItemId",
                table: "ClipInfos",
                newName: "IX_ClipInfos_ClipItemId");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AddedAt",
                table: "Clips",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClipLinkInfos",
                table: "ClipLinkInfos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClipInfos",
                table: "ClipInfos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClipInfos_Clips_ClipItemId",
                table: "ClipInfos",
                column: "ClipItemId",
                principalTable: "Clips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClipLinkInfos_Clips_ClipItemId",
                table: "ClipLinkInfos",
                column: "ClipItemId",
                principalTable: "Clips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClipInfos_Clips_ClipItemId",
                table: "ClipInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_ClipLinkInfos_Clips_ClipItemId",
                table: "ClipLinkInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClipLinkInfos",
                table: "ClipLinkInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClipInfos",
                table: "ClipInfos");

            migrationBuilder.DropColumn(
                name: "AddedAt",
                table: "Clips");

            migrationBuilder.RenameTable(
                name: "ClipLinkInfos",
                newName: "ClipLinkInfo");

            migrationBuilder.RenameTable(
                name: "ClipInfos",
                newName: "ClipInfo");

            migrationBuilder.RenameIndex(
                name: "IX_ClipLinkInfos_ClipItemId",
                table: "ClipLinkInfo",
                newName: "IX_ClipLinkInfo_ClipItemId");

            migrationBuilder.RenameIndex(
                name: "IX_ClipInfos_ClipItemId",
                table: "ClipInfo",
                newName: "IX_ClipInfo_ClipItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClipLinkInfo",
                table: "ClipLinkInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClipInfo",
                table: "ClipInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClipInfo_Clips_ClipItemId",
                table: "ClipInfo",
                column: "ClipItemId",
                principalTable: "Clips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClipLinkInfo_Clips_ClipItemId",
                table: "ClipLinkInfo",
                column: "ClipItemId",
                principalTable: "Clips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
