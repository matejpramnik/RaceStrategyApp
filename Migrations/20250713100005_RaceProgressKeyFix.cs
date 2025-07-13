using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceStrategyApp.Migrations
{
    /// <inheritdoc />
    public partial class RaceProgressKeyFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaceProgresses_Races_RaceId",
                table: "RaceProgresses");

            migrationBuilder.DropIndex(
                name: "IX_RaceProgresses_RaceId",
                table: "RaceProgresses");

            migrationBuilder.DropColumn(
                name: "RaceId",
                table: "RaceProgresses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RaceId",
                table: "RaceProgresses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

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
    }
}
