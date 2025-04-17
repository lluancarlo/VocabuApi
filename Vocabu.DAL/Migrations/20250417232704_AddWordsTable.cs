using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Vocabu.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddWordsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Words",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Language = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WordMeanings",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PartOfSpeech = table.Column<int>(type: "integer", maxLength: 50, nullable: false),
                    Definition = table.Column<string>(type: "text", nullable: false),
                    Example = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AudioUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    WordId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordMeanings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordMeanings_Words_WordId",
                        column: x => x.WordId,
                        principalSchema: "dbo",
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordAntonyms",
                schema: "dbo",
                columns: table => new
                {
                    WordId = table.Column<int>(type: "integer", nullable: false),
                    MeaningId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordAntonyms", x => new { x.MeaningId, x.WordId });
                    table.ForeignKey(
                        name: "FK_WordAntonyms_WordMeanings_MeaningId",
                        column: x => x.MeaningId,
                        principalSchema: "dbo",
                        principalTable: "WordMeanings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WordAntonyms_Words_WordId",
                        column: x => x.WordId,
                        principalSchema: "dbo",
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordSynonyms",
                schema: "dbo",
                columns: table => new
                {
                    WordId = table.Column<int>(type: "integer", nullable: false),
                    MeaningId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordSynonyms", x => new { x.WordId, x.MeaningId });
                    table.ForeignKey(
                        name: "FK_WordSynonyms_WordMeanings_MeaningId",
                        column: x => x.MeaningId,
                        principalSchema: "dbo",
                        principalTable: "WordMeanings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WordSynonyms_Words_WordId",
                        column: x => x.WordId,
                        principalSchema: "dbo",
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WordAntonyms_WordId",
                schema: "dbo",
                table: "WordAntonyms",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_WordMeanings_Id",
                schema: "dbo",
                table: "WordMeanings",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WordMeanings_WordId",
                schema: "dbo",
                table: "WordMeanings",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_Words_Id",
                schema: "dbo",
                table: "Words",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WordSynonyms_MeaningId",
                schema: "dbo",
                table: "WordSynonyms",
                column: "MeaningId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordAntonyms",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "WordSynonyms",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "WordMeanings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Words",
                schema: "dbo");
        }
    }
}
