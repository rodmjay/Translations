using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Translations.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Translations");

            migrationBuilder.CreateTable(
                name: "Engine",
                schema: "Translations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Engine", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                schema: "Translations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Phrase",
                schema: "Translations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phrase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EngineLanguage",
                schema: "Translations",
                columns: table => new
                {
                    LanguageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EngineId = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineLanguage", x => new { x.LanguageId, x.EngineId });
                    table.ForeignKey(
                        name: "FK_EngineLanguage_Engine_EngineId",
                        column: x => x.EngineId,
                        principalSchema: "Translations",
                        principalTable: "Engine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EngineLanguage_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "Translations",
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MachineTranslation",
                schema: "Translations",
                columns: table => new
                {
                    EngineId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhraseId = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    TranslationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineTranslation", x => new { x.EngineId, x.LanguageId, x.PhraseId });
                    table.ForeignKey(
                        name: "FK_MachineTranslation_EngineLanguage_LanguageId_EngineId",
                        columns: x => new { x.LanguageId, x.EngineId },
                        principalSchema: "Translations",
                        principalTable: "EngineLanguage",
                        principalColumns: new[] { "LanguageId", "EngineId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MachineTranslation_Engine_EngineId",
                        column: x => x.EngineId,
                        principalSchema: "Translations",
                        principalTable: "Engine",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MachineTranslation_Phrase_PhraseId",
                        column: x => x.PhraseId,
                        principalSchema: "Translations",
                        principalTable: "Phrase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Translations",
                table: "Engine",
                columns: new[] { "Id", "Enabled", "Name" },
                values: new object[,]
                {
                    { 1, true, "Google Cloud Translate" },
                    { 2, true, "Azure Translator by Microsoft" },
                    { 3, false, "Amazon Translate" },
                    { 4, false, "DeepL" }
                });

            migrationBuilder.InsertData(
                schema: "Translations",
                table: "Language",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "af", "Afrikaans" },
                    { "am", "Amharic" },
                    { "ar", "Arabic" },
                    { "as", "Assamese" },
                    { "az", "Azerbaijani (Latin)" },
                    { "ba", "Bashkir" },
                    { "bg", "Bulgarian" },
                    { "bho", "Bhojpuri" },
                    { "bn", "Bangla" },
                    { "bo", "Tibetan" },
                    { "brx", "Bodo" },
                    { "bs", "Bosnian (Latin)" },
                    { "ca", "Catalan" },
                    { "cs", "Czech" },
                    { "cy", "Welsh" },
                    { "da", "Danish" },
                    { "de", "German" },
                    { "doi", "Dogri" },
                    { "dsb", "Lower Sorbian" },
                    { "dv", "Divehi" },
                    { "el", "Greek" },
                    { "en", "English" },
                    { "es", "Spanish" },
                    { "es-MX", "Spanish (Mexico)" },
                    { "et", "Estonian" },
                    { "eu", "Basque" },
                    { "fa", "Persian" },
                    { "fi", "Finnish" },
                    { "fil", "Filipino" },
                    { "fj", "Fijian" },
                    { "fo", "Faroese" },
                    { "fr", "French" },
                    { "fr-ca", "French (Canada)" },
                    { "gl", "Galician" },
                    { "gom", "Konkani" },
                    { "gu", "Gujarati" },
                    { "ha", "Hausa" },
                    { "hi", "Hindi" },
                    { "hr", "Croatian" },
                    { "hsb", "Upper Sorbian" },
                    { "ht", "Haitian Creole" },
                    { "hu", "Hungarian" },
                    { "hy", "Armenian" },
                    { "id", "Indonesian" },
                    { "ig", "Igbo" },
                    { "ikt", "Inuinnaqtun" },
                    { "ir", "Irish" },
                    { "is", "Icelandic" },
                    { "it", "Italian" },
                    { "itu-Latin", "Inuktitut (Latin)" },
                    { "iu", "Inuktitut" },
                    { "iw", "Hebrew" },
                    { "ja", "Japanese" },
                    { "ka", "Georgian" },
                    { "kk", "Kazahk" },
                    { "km", "Kymer" },
                    { "kmr", "Kurdish (Northern)" },
                    { "kn", "Kannada" },
                    { "ko", "Korean" },
                    { "ks", "Kashmiri" },
                    { "ku", "Kurdish (Central)" },
                    { "ky", "Kyrgyz" },
                    { "ln", "Lingala" },
                    { "lo", "Lao" },
                    { "lt", "Lithuanian" },
                    { "lug", "Liganda" },
                    { "lv", "Latvian" },
                    { "mai", "Maithili" },
                    { "mg", "Malagasy" },
                    { "mi", "Maori" },
                    { "mk", "Macedonian" },
                    { "ml", "Malayalam" },
                    { "mn-Cyrl", "Mongolian (Cyrillic)" },
                    { "mn-Mong", "Mongilian (Traditional)" },
                    { "mr", "Marathi" },
                    { "ms", "Malay (Latin)" },
                    { "mt", "Maltese" },
                    { "mww", "Hmong Daw (Latin)" },
                    { "my", "Myanmar" },
                    { "ne", "Nepali" },
                    { "nl", "Dutch" },
                    { "no", "Norwegian" },
                    { "nso", "Sesotho sa Leboa" },
                    { "nya", "Nyanja" },
                    { "or", "Odia" },
                    { "otq", "Queretaro" },
                    { "pa", "Punjabi" },
                    { "pl", "Polish" },
                    { "prs", "Dari" },
                    { "ps", "Pashto" },
                    { "pt", "Portugese (Brazil)" },
                    { "pt-pt", "Portugese (Portugal)" },
                    { "ro", "Romanian" },
                    { "ru", "Russian" },
                    { "run", "Rundi" },
                    { "rw", "Kiyarwanda" },
                    { "sd", "Sindhi" },
                    { "si", "Sinhala" },
                    { "sk", "Slovak" },
                    { "sl", "Slovenian" },
                    { "sm", "Samoan (Latin)" },
                    { "sn", "ChiShona" },
                    { "so", "Somali (Arabic)" },
                    { "sq", "Albanian" },
                    { "sr-Cyrl", "Serbian (Cyrillic)" },
                    { "sr-Latn", "Serbian (Latin)" },
                    { "st", "Sesotho" },
                    { "sv", "Swedish" },
                    { "sw", "Swahili (Latin)" },
                    { "ta", "Tamil" },
                    { "te", "Telugu" },
                    { "th", "Thai" },
                    { "ti", "Tigrinya" },
                    { "tk", "Tirkmen (Latin)" },
                    { "tl", "Filipino (Tagalog)" },
                    { "tlh-Latn", "Klingon" },
                    { "tn", "Setswana" },
                    { "to", "Tongan" },
                    { "tr", "Turkish" },
                    { "tt", "Tatar (Latin)" },
                    { "ty", "Tahitian" },
                    { "ug", "Uyghur (Arabic)" },
                    { "uk", "Ukranian" },
                    { "ur", "Urdu" },
                    { "uz", "Uzbek (Latin)" },
                    { "vi", "Vietnamese" },
                    { "xh", "Zhosa" },
                    { "yo", "Yoruba" },
                    { "yua", "Yucatec Maya" },
                    { "zh-CN", "Chinese (Simplified)" },
                    { "zh-TW", "Chinese (Traditional)" },
                    { "zu", "Zulu" }
                });

            migrationBuilder.InsertData(
                schema: "Translations",
                table: "EngineLanguage",
                columns: new[] { "EngineId", "LanguageId", "Weight" },
                values: new object[,]
                {
                    { 2, "af", 0 },
                    { 3, "af", 0 },
                    { 2, "am", 0 },
                    { 3, "am", 0 },
                    { 1, "ar", 0 },
                    { 2, "ar", 0 },
                    { 3, "ar", 0 },
                    { 2, "as", 0 },
                    { 2, "az", 0 },
                    { 3, "az", 0 },
                    { 2, "ba", 0 },
                    { 2, "bg", 0 },
                    { 3, "bg", 0 },
                    { 2, "bho", 0 },
                    { 2, "bn", 0 },
                    { 3, "bn", 0 },
                    { 2, "bo", 0 },
                    { 2, "brx", 0 },
                    { 2, "bs", 0 },
                    { 3, "bs", 0 },
                    { 2, "ca", 0 },
                    { 3, "ca", 0 },
                    { 1, "cs", 0 },
                    { 2, "cs", 0 },
                    { 3, "cs", 0 },
                    { 2, "cy", 0 },
                    { 3, "cy", 0 },
                    { 1, "da", 0 },
                    { 2, "da", 0 },
                    { 3, "da", 0 },
                    { 1, "de", 0 },
                    { 2, "de", 0 },
                    { 3, "de", 0 },
                    { 2, "doi", 0 },
                    { 2, "dsb", 0 },
                    { 2, "dv", 0 },
                    { 1, "el", 0 },
                    { 2, "el", 0 },
                    { 3, "el", 0 },
                    { 1, "en", 0 },
                    { 2, "en", 0 },
                    { 3, "en", 0 },
                    { 1, "es", 0 },
                    { 2, "es", 0 },
                    { 3, "es", 0 },
                    { 2, "et", 0 },
                    { 3, "et", 0 },
                    { 2, "eu", 0 },
                    { 2, "fa", 0 },
                    { 3, "fa", 0 },
                    { 1, "fi", 0 },
                    { 2, "fi", 0 },
                    { 3, "fi", 0 },
                    { 2, "fil", 0 },
                    { 2, "fj", 0 },
                    { 2, "fo", 0 },
                    { 1, "fr", 0 },
                    { 2, "fr", 0 },
                    { 3, "fr", 0 },
                    { 2, "fr-ca", 0 },
                    { 3, "fr-CA", 0 },
                    { 2, "gl", 0 },
                    { 2, "gom", 0 },
                    { 2, "gu", 0 },
                    { 3, "gu", 0 },
                    { 2, "ha", 0 },
                    { 3, "ha", 0 },
                    { 1, "hi", 0 },
                    { 2, "hi", 0 },
                    { 3, "hi", 0 },
                    { 2, "hr", 0 },
                    { 3, "hr", 0 },
                    { 2, "hsb", 0 },
                    { 2, "ht", 0 },
                    { 3, "ht", 0 },
                    { 1, "hu", 0 },
                    { 2, "hu", 0 },
                    { 3, "hu", 0 },
                    { 2, "hy", 0 },
                    { 3, "hy", 0 },
                    { 2, "id", 0 },
                    { 3, "id", 0 },
                    { 2, "ig", 0 },
                    { 2, "ikt", 0 },
                    { 2, "ir", 0 },
                    { 3, "ir", 0 },
                    { 2, "is", 0 },
                    { 3, "is", 0 },
                    { 1, "it", 0 },
                    { 2, "it", 0 },
                    { 3, "it", 0 },
                    { 2, "iu", 0 },
                    { 1, "iw", 0 },
                    { 2, "iw", 0 },
                    { 3, "iw", 0 },
                    { 1, "ja", 0 },
                    { 2, "ja", 0 },
                    { 3, "ja", 0 },
                    { 2, "ka", 0 },
                    { 3, "ka", 0 },
                    { 2, "kk", 0 },
                    { 3, "kk", 0 },
                    { 2, "km", 0 },
                    { 2, "kmr", 0 },
                    { 2, "kn", 0 },
                    { 3, "kn", 0 },
                    { 1, "ko", 0 },
                    { 2, "ko", 0 },
                    { 3, "ko", 0 },
                    { 2, "ks", 0 },
                    { 2, "ku", 0 },
                    { 2, "ky", 0 },
                    { 2, "ln", 0 },
                    { 2, "lo", 0 },
                    { 2, "lt", 0 },
                    { 3, "lt", 0 },
                    { 2, "lug", 0 },
                    { 2, "lv", 0 },
                    { 3, "lv", 0 },
                    { 2, "mai", 0 },
                    { 2, "mg", 0 },
                    { 2, "mi", 0 },
                    { 2, "mk", 0 },
                    { 3, "mk", 0 },
                    { 2, "ml", 0 },
                    { 3, "ml", 0 },
                    { 3, "mn-Cyrl", 0 },
                    { 2, "mr", 0 },
                    { 3, "mr", 0 },
                    { 2, "ms", 0 },
                    { 3, "ms", 0 },
                    { 2, "mt", 0 },
                    { 3, "mt", 0 },
                    { 2, "mww", 0 },
                    { 2, "my", 0 },
                    { 2, "ne", 0 },
                    { 1, "nl", 0 },
                    { 2, "nl", 0 },
                    { 3, "nl", 0 },
                    { 1, "no", 0 },
                    { 2, "no", 0 },
                    { 3, "no", 0 },
                    { 2, "nso", 0 },
                    { 2, "nya", 0 },
                    { 2, "or", 0 },
                    { 2, "otq", 0 },
                    { 2, "pa", 0 },
                    { 3, "pa", 0 },
                    { 1, "pl", 0 },
                    { 2, "pl", 0 },
                    { 3, "pl", 0 },
                    { 2, "prs", 0 },
                    { 2, "ps", 0 },
                    { 3, "ps", 0 },
                    { 1, "pt", 0 },
                    { 2, "pt", 0 },
                    { 3, "pt", 0 },
                    { 2, "pt-pt", 0 },
                    { 3, "pt-PT", 0 },
                    { 2, "ro", 0 },
                    { 3, "ro", 0 },
                    { 1, "ru", 0 },
                    { 2, "ru", 0 },
                    { 3, "ru", 0 },
                    { 2, "run", 0 },
                    { 2, "rw", 0 },
                    { 2, "sd", 0 },
                    { 2, "si", 0 },
                    { 3, "si", 0 },
                    { 2, "sk", 0 },
                    { 3, "sk", 0 },
                    { 2, "sl", 0 },
                    { 3, "sl", 0 },
                    { 2, "sm", 0 },
                    { 2, "sn", 0 },
                    { 2, "so", 0 },
                    { 3, "so", 0 },
                    { 2, "sq", 0 },
                    { 3, "sq", 0 },
                    { 2, "st", 0 },
                    { 1, "sv", 0 },
                    { 2, "sv", 0 },
                    { 3, "sv", 0 },
                    { 2, "sw", 0 },
                    { 3, "sw", 0 },
                    { 2, "ta", 0 },
                    { 3, "ta", 0 },
                    { 2, "te", 0 },
                    { 3, "te", 0 },
                    { 1, "th", 0 },
                    { 2, "th", 0 },
                    { 3, "th", 0 },
                    { 2, "ti", 0 },
                    { 2, "tk", 0 },
                    { 3, "tl", 0 },
                    { 2, "tn", 0 },
                    { 2, "to", 0 },
                    { 1, "tr", 0 },
                    { 2, "tr", 0 },
                    { 3, "tr", 0 },
                    { 2, "tt", 0 },
                    { 2, "ty", 0 },
                    { 2, "ug", 0 },
                    { 2, "uk", 0 },
                    { 3, "uk", 0 },
                    { 2, "ur", 0 },
                    { 3, "ur", 0 },
                    { 2, "uz", 0 },
                    { 3, "uz", 0 },
                    { 1, "vi", 0 },
                    { 2, "vi", 0 },
                    { 3, "vi", 0 },
                    { 2, "yo", 0 },
                    { 2, "yua", 0 },
                    { 1, "zh-CN", 0 },
                    { 3, "zh-CN", 0 },
                    { 1, "zh-TW", 0 },
                    { 3, "zh-TW", 0 },
                    { 2, "zu", 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EngineLanguage_EngineId",
                schema: "Translations",
                table: "EngineLanguage",
                column: "EngineId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineTranslation_LanguageId_EngineId",
                schema: "Translations",
                table: "MachineTranslation",
                columns: new[] { "LanguageId", "EngineId" });

            migrationBuilder.CreateIndex(
                name: "IX_MachineTranslation_PhraseId",
                schema: "Translations",
                table: "MachineTranslation",
                column: "PhraseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MachineTranslation",
                schema: "Translations");

            migrationBuilder.DropTable(
                name: "EngineLanguage",
                schema: "Translations");

            migrationBuilder.DropTable(
                name: "Phrase",
                schema: "Translations");

            migrationBuilder.DropTable(
                name: "Engine",
                schema: "Translations");

            migrationBuilder.DropTable(
                name: "Language",
                schema: "Translations");
        }
    }
}
