using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceStrategyApp.Migrations
{
    /// <inheritdoc />
    public partial class RaceProgressModelUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaceProgresses_Races_RaceSnapshotId",
                table: "RaceProgresses");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_RaceProgresses_RaceId",
                table: "RaceProgresses");

            migrationBuilder.DropIndex(
                name: "IX_RaceProgresses_RaceSnapshotId",
                table: "RaceProgresses");

            migrationBuilder.DropColumn(
                name: "RaceSnapshotId",
                table: "RaceProgresses");

            migrationBuilder.CreateIndex(
                name: "IX_RaceProgresses_RaceId",
                table: "RaceProgresses",
                column: "RaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_RaceProgresses_Races_RaceId",
                table: "RaceProgresses",
                column: "RaceId",
                principalTable: "Races",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaceProgresses_Races_RaceId",
                table: "RaceProgresses");

            migrationBuilder.DropIndex(
                name: "IX_RaceProgresses_RaceId",
                table: "RaceProgresses");

            migrationBuilder.AddColumn<int>(
                name: "RaceSnapshotId",
                table: "RaceProgresses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RaceProgresses_RaceId",
                table: "RaceProgresses",
                column: "RaceId");

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
    }
}
