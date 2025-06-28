using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceStrategyApp.Migrations
{
    /// <inheritdoc />
    public partial class RaceProgresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RaceProgressId",
                table: "Races",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RaceProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RaceId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceProgresses", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_RaceProgresses_RaceProgressId",
                table: "Races");

            migrationBuilder.DropTable(
                name: "RaceProgresses");

            migrationBuilder.DropIndex(
                name: "IX_Races_RaceProgressId",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "RaceProgressId",
                table: "Races");
        }
    }
}
