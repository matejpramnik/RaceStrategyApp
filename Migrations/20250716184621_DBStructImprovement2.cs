using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceStrategyApp.Migrations
{
    /// <inheritdoc />
    public partial class DBStructImprovement2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_PitStops_PitStopId",
                table: "Races");

            migrationBuilder.DropForeignKey(
                name: "FK_Races_TrackInfo_TrackInfoId",
                table: "Races");

            migrationBuilder.DropIndex(
                name: "IX_Races_PitStopId",
                table: "Races");

            migrationBuilder.DropIndex(
                name: "IX_Races_TrackInfoId",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "PitStopId",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "TrackInfoId",
                table: "Races");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PitStopId",
                table: "Races",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrackInfoId",
                table: "Races",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Races_PitStopId",
                table: "Races",
                column: "PitStopId");

            migrationBuilder.CreateIndex(
                name: "IX_Races_TrackInfoId",
                table: "Races",
                column: "TrackInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Races_PitStops_PitStopId",
                table: "Races",
                column: "PitStopId",
                principalTable: "PitStops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Races_TrackInfo_TrackInfoId",
                table: "Races",
                column: "TrackInfoId",
                principalTable: "TrackInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
