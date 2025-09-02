using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace movie_explorer.Migrations
{
    /// <inheritdoc />
    public partial class ModelChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    TmdbId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.TmdbId);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    TmdbId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoteCount = table.Column<int>(type: "int", nullable: false),
                    VoteAverage = table.Column<float>(type: "real", nullable: false),
                    Popularity = table.Column<float>(type: "real", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.TmdbId);
                });

            migrationBuilder.CreateTable(
                name: "GenreMovie",
                columns: table => new
                {
                    GenresTmdbId = table.Column<int>(type: "int", nullable: false),
                    MoviesTmdbId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreMovie", x => new { x.GenresTmdbId, x.MoviesTmdbId });
                    table.ForeignKey(
                        name: "FK_GenreMovie_Genres_GenresTmdbId",
                        column: x => x.GenresTmdbId,
                        principalTable: "Genres",
                        principalColumn: "TmdbId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenreMovie_Movies_MoviesTmdbId",
                        column: x => x.MoviesTmdbId,
                        principalTable: "Movies",
                        principalColumn: "TmdbId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GenreMovie_MoviesTmdbId",
                table: "GenreMovie",
                column: "MoviesTmdbId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenreMovie");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
