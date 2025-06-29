using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceStrategyApp.Migrations
{
    /// <inheritdoc />
    public partial class RP_AlternateKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_RaceProgresses_RaceId",
                table: "RaceProgresses",
                column: "RaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_RaceProgresses_RaceId",
                table: "RaceProgresses");
        }
    }
}
