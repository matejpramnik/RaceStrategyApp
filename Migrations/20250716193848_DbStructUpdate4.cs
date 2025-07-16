using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceStrategyApp.Migrations
{
    /// <inheritdoc />
    public partial class DbStructUpdate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PitStops",
                table: "PitStops");

            migrationBuilder.RenameTable(
                name: "PitStops",
                newName: "PitStop");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PitStop",
                table: "PitStop",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TrackInfo_RaceId",
                table: "TrackInfo",
                column: "RaceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PitStop_RaceId",
                table: "PitStop",
                column: "RaceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PitStop_Races_RaceId",
                table: "PitStop",
                column: "RaceId",
                principalTable: "Races",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackInfo_Races_RaceId",
                table: "TrackInfo",
                column: "RaceId",
                principalTable: "Races",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PitStop_Races_RaceId",
                table: "PitStop");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackInfo_Races_RaceId",
                table: "TrackInfo");

            migrationBuilder.DropIndex(
                name: "IX_TrackInfo_RaceId",
                table: "TrackInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PitStop",
                table: "PitStop");

            migrationBuilder.DropIndex(
                name: "IX_PitStop_RaceId",
                table: "PitStop");

            migrationBuilder.RenameTable(
                name: "PitStop",
                newName: "PitStops");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PitStops",
                table: "PitStops",
                column: "Id");
        }
    }
}
