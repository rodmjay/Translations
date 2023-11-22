using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Translations.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class PopluateNormalizedTextColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("update [Translations].[Phrase] set [NormalizedText] = UPPER([Text])");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
