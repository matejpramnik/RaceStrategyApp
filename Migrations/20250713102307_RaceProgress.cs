using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceStrategyApp.Migrations
{
    /// <inheritdoc />
    public partial class RaceProgress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RaceSnapshotId",
                table: "RaceProgresses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RaceSnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LapCount = table.Column<int>(type: "INTEGER", nullable: false),
                    ChangeName = table.Column<string>(type: "TEXT", nullable: false),
                    Change = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceSnapshots", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RaceProgresses_RaceSnapshotId",
                table: "RaceProgresses",
                column: "RaceSnapshotId");

            migrationBuilder.AddForeignKey(
                name: "FK_RaceProgresses_RaceSnapshots_RaceSnapshotId",
                table: "RaceProgresses",
                column: "RaceSnapshotId",
                principalTable: "RaceSnapshots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaceProgresses_RaceSnapshots_RaceSnapshotId",
                table: "RaceProgresses");

            migrationBuilder.DropTable(
                name: "RaceSnapshots");

            migrationBuilder.DropIndex(
                name: "IX_RaceProgresses_RaceSnapshotId",
                table: "RaceProgresses");

            migrationBuilder.DropColumn(
                name: "RaceSnapshotId",
                table: "RaceProgresses");
        }
    }
}
