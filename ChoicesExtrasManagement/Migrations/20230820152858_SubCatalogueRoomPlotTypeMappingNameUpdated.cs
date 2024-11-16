using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoicesExtrasManagement.Migrations
{
    /// <inheritdoc />
    public partial class SubCatalogueRoomPlotTypeMappingNameUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogueRoomPlotTypeMappings");

            migrationBuilder.CreateTable(
                name: "SubCatalogueRoomPlotTypeMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubCatalogueId = table.Column<int>(type: "int", nullable: true),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    PlotTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCatalogueRoomPlotTypeMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCatalogueRoomPlotTypeMappings_PlotTypes_PlotTypeId",
                        column: x => x.PlotTypeId,
                        principalTable: "PlotTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubCatalogueRoomPlotTypeMappings_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubCatalogueRoomPlotTypeMappings_SubCatalogues_SubCatalogueId",
                        column: x => x.SubCatalogueId,
                        principalTable: "SubCatalogues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubCatalogueRoomPlotTypeMappings_PlotTypeId",
                table: "SubCatalogueRoomPlotTypeMappings",
                column: "PlotTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCatalogueRoomPlotTypeMappings_RoomId",
                table: "SubCatalogueRoomPlotTypeMappings",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCatalogueRoomPlotTypeMappings_SubCatalogueId",
                table: "SubCatalogueRoomPlotTypeMappings",
                column: "SubCatalogueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubCatalogueRoomPlotTypeMappings");

            migrationBuilder.CreateTable(
                name: "CatalogueRoomPlotTypeMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatalogueId = table.Column<int>(type: "int", nullable: true),
                    PlotTypeId = table.Column<int>(type: "int", nullable: true),
                    RoomId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogueRoomPlotTypeMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogueRoomPlotTypeMappings_Catalogues_CatalogueId",
                        column: x => x.CatalogueId,
                        principalTable: "Catalogues",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CatalogueRoomPlotTypeMappings_PlotTypes_PlotTypeId",
                        column: x => x.PlotTypeId,
                        principalTable: "PlotTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CatalogueRoomPlotTypeMappings_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogueRoomPlotTypeMappings_CatalogueId",
                table: "CatalogueRoomPlotTypeMappings",
                column: "CatalogueId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogueRoomPlotTypeMappings_PlotTypeId",
                table: "CatalogueRoomPlotTypeMappings",
                column: "PlotTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogueRoomPlotTypeMappings_RoomId",
                table: "CatalogueRoomPlotTypeMappings",
                column: "RoomId");
        }
    }
}
