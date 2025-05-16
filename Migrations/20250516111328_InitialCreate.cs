using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceStrategyApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Races",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    NumberOfLaps = table.Column<int>(type: "INTEGER", nullable: false),
                    LapCount = table.Column<int>(type: "INTEGER", nullable: false),
                    MandatoryStops = table.Column<int>(type: "INTEGER", nullable: false),
                    Refueling = table.Column<bool>(type: "INTEGER", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    AmountOfOpponents = table.Column<int>(type: "INTEGER", nullable: false),
                    SelectedTyres = table.Column<string>(type: "TEXT", nullable: false),
                    TrackState = table.Column<int>(type: "INTEGER", nullable: false),
                    Damage = table.Column<bool>(type: "INTEGER", nullable: false),
                    TerminalDamage = table.Column<bool>(type: "INTEGER", nullable: false),
                    TrackWeather = table.Column<int>(type: "INTEGER", nullable: false),
                    RaceSeriesID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Races", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Races_RaceSeries_RaceSeriesID",
                        column: x => x.RaceSeriesID,
                        principalTable: "RaceSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tyre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Compound = table.Column<int>(type: "INTEGER", nullable: false),
                    TyreDegCoeff = table.Column<float>(type: "REAL", nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false),
                    RaceId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tyre", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tyre_Races_RaceId",
                        column: x => x.RaceId,
                        principalTable: "Races",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Races_RaceSeriesID",
                table: "Races",
                column: "RaceSeriesID");

            migrationBuilder.CreateIndex(
                name: "IX_Tyre_RaceId",
                table: "Tyre",
                column: "RaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tyre");

            migrationBuilder.DropTable(
                name: "Races");

            migrationBuilder.DropTable(
                name: "RaceSeries");
        }
    }
}
