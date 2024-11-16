using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoicesExtrasManagement.Migrations
{
    /// <inheritdoc />
    public partial class SavedChoicesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SavedChoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlotId = table.Column<int>(type: "int", nullable: false),
                    PlotRoomId = table.Column<int>(type: "int", nullable: false),
                    PlotTypeRoomMappingId = table.Column<int>(type: "int", nullable: false),
                    CatalogueId = table.Column<int>(type: "int", nullable: false),
                    SubCatalogueId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedChoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedChoices_Catalogues_CatalogueId",
                        column: x => x.CatalogueId,
                        principalTable: "Catalogues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedChoices_PlotTypeRoomMappings_PlotTypeRoomMappingId",
                        column: x => x.PlotTypeRoomMappingId,
                        principalTable: "PlotTypeRoomMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedChoices_Plots_PlotId",
                        column: x => x.PlotId,
                        principalTable: "Plots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedChoices_SubCatalogues_SubCatalogueId",
                        column: x => x.SubCatalogueId,
                        principalTable: "SubCatalogues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavedChoices_CatalogueId",
                table: "SavedChoices",
                column: "CatalogueId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedChoices_PlotId",
                table: "SavedChoices",
                column: "PlotId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedChoices_PlotTypeRoomMappingId",
                table: "SavedChoices",
                column: "PlotTypeRoomMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedChoices_SubCatalogueId",
                table: "SavedChoices",
                column: "SubCatalogueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedChoices");
        }
    }
}
