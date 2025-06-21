using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceStrategyApp.Migrations
{
    /// <inheritdoc />
    public partial class NewUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_RaceSeries_RaceSeriesID",
                table: "Races");

            migrationBuilder.RenameColumn(
                name: "RaceSeriesID",
                table: "Races",
                newName: "RaceSeriesId");

            migrationBuilder.RenameIndex(
                name: "IX_Races_RaceSeriesID",
                table: "Races",
                newName: "IX_Races_RaceSeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Races_RaceSeries_RaceSeriesId",
                table: "Races",
                column: "RaceSeriesId",
                principalTable: "RaceSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_RaceSeries_RaceSeriesId",
                table: "Races");

            migrationBuilder.RenameColumn(
                name: "RaceSeriesId",
                table: "Races",
                newName: "RaceSeriesID");

            migrationBuilder.RenameIndex(
                name: "IX_Races_RaceSeriesId",
                table: "Races",
                newName: "IX_Races_RaceSeriesID");

            migrationBuilder.AddForeignKey(
                name: "FK_Races_RaceSeries_RaceSeriesID",
                table: "Races",
                column: "RaceSeriesID",
                principalTable: "RaceSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
