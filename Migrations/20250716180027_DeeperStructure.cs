using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceStrategyApp.Migrations
{
    /// <inheritdoc />
    public partial class DeeperStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PitStops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MandatoryStops = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfStops = table.Column<int>(type: "INTEGER", nullable: false),
                    Refueling = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastRefuelLap = table.Column<int>(type: "INTEGER", nullable: false),
                    SelectedTyres = table.Column<string>(type: "TEXT", nullable: false),
                    CurrentTyre = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PitStops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RaceSeries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ParticipantCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceSeries", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "TrackInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrackWeather = table.Column<int>(type: "INTEGER", nullable: false),
                    TrackState = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RaceProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RaceId = table.Column<int>(type: "INTEGER", nullable: false),
                    RaceSnapshotId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RaceProgresses_RaceSnapshots_RaceSnapshotId",
                        column: x => x.RaceSnapshotId,
                        principalTable: "RaceSnapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Races",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    NumberOfLaps = table.Column<int>(type: "INTEGER", nullable: false),
                    LapCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    AmountOfOpponents = table.Column<int>(type: "INTEGER", nullable: false),
                    PitStopId = table.Column<int>(type: "INTEGER", nullable: false),
                    TrackInfoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Damage = table.Column<bool>(type: "INTEGER", nullable: false),
                    TerminalDamage = table.Column<bool>(type: "INTEGER", nullable: false),
                    RaceSeriesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Races", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Races_PitStops_PitStopId",
                        column: x => x.PitStopId,
                        principalTable: "PitStops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Races_RaceSeries_RaceSeriesId",
                        column: x => x.RaceSeriesId,
                        principalTable: "RaceSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Races_TrackInfo_TrackInfoId",
                        column: x => x.TrackInfoId,
                        principalTable: "TrackInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RaceProgresses_RaceSnapshotId",
                table: "RaceProgresses",
                column: "RaceSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_Races_PitStopId",
                table: "Races",
                column: "PitStopId");

            migrationBuilder.CreateIndex(
                name: "IX_Races_RaceSeriesId",
                table: "Races",
                column: "RaceSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Races_TrackInfoId",
                table: "Races",
                column: "TrackInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RaceProgresses");

            migrationBuilder.DropTable(
                name: "Races");

            migrationBuilder.DropTable(
                name: "RaceSnapshots");

            migrationBuilder.DropTable(
                name: "PitStops");

            migrationBuilder.DropTable(
                name: "RaceSeries");

            migrationBuilder.DropTable(
                name: "TrackInfo");
        }
    }
}
