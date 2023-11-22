using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Translations.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexesToPhraseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Text",
                schema: "Translations",
                table: "Phrase",
                type: "nvarchar(450)",

                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Phrase_Text",
                schema: "Translations",
                table: "Phrase",
                column: "Text",
                unique: true,
                filter: "[Text] IS NOT NULL")
                .Annotation("SqlServer:Clustered", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Phrase_Text",
                schema: "Translations",
                table: "Phrase");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                schema: "Translations",
                table: "Phrase",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
