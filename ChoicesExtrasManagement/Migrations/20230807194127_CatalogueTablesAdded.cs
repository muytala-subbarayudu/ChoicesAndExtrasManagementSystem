using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoicesExtrasManagement.Migrations
{
    /// <inheritdoc />
    public partial class CatalogueTablesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Catalogues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChoiceOrExtra = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalogues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubCatalogues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CatalogueId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCatalogues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCatalogues_Catalogues_CatalogueId",
                        column: x => x.CatalogueId,
                        principalTable: "Catalogues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubCatalogueRoomMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubCatalogueId = table.Column<int>(type: "int", nullable: true),
                    RoomId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCatalogueRoomMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCatalogueRoomMappings_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubCatalogueRoomMappings_SubCatalogues_SubCatalogueId",
                        column: x => x.SubCatalogueId,
                        principalTable: "SubCatalogues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubCatalogueRoomMappings_RoomId",
                table: "SubCatalogueRoomMappings",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCatalogueRoomMappings_SubCatalogueId",
                table: "SubCatalogueRoomMappings",
                column: "SubCatalogueId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCatalogues_CatalogueId",
                table: "SubCatalogues",
                column: "CatalogueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubCatalogueRoomMappings");

            migrationBuilder.DropTable(
                name: "SubCatalogues");

            migrationBuilder.DropTable(
                name: "Catalogues");
        }
    }
}
