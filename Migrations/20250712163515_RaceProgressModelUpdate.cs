using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceStrategyApp.Migrations
{
    /// <inheritdoc />
    public partial class RaceProgressModelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_RaceProgresses_RaceProgressId",
                table: "Races");

            migrationBuilder.DropIndex(
                name: "IX_Races_RaceProgressId",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "AvailableTyres",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "RaceProgressId",
                table: "Races");

            migrationBuilder.AddColumn<int>(
                name: "RaceSnapshotId",
                table: "RaceProgresses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RaceProgresses_RaceSnapshotId",
                table: "RaceProgresses",
                column: "RaceSnapshotId");

            migrationBuilder.AddForeignKey(
                name: "FK_RaceProgresses_Races_RaceSnapshotId",
                table: "RaceProgresses",
                column: "RaceSnapshotId",
                principalTable: "Races",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaceProgresses_Races_RaceSnapshotId",
                table: "RaceProgresses");

            migrationBuilder.DropIndex(
                name: "IX_RaceProgresses_RaceSnapshotId",
                table: "RaceProgresses");

            migrationBuilder.DropColumn(
                name: "RaceSnapshotId",
                table: "RaceProgresses");

            migrationBuilder.AddColumn<string>(
                name: "AvailableTyres",
                table: "Races",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RaceProgressId",
                table: "Races",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Races_RaceProgressId",
                table: "Races",
                column: "RaceProgressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Races_RaceProgresses_RaceProgressId",
                table: "Races",
                column: "RaceProgressId",
                principalTable: "RaceProgresses",
                principalColumn: "Id");
        }
    }
}
