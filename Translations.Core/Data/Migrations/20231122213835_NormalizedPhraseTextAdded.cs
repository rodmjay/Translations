using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Translations.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class NormalizedPhraseTextAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedText",
                schema: "Translations",
                table: "Phrase",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedText",
                schema: "Translations",
                table: "Phrase");
        }
    }
}
