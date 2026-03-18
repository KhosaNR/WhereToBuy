using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseAuditableEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PriceType = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Abbreviation = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PriceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPack = table.Column<bool>(type: "bit", nullable: true),
                    UnitsPerPack = table.Column<long>(type: "bigint", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsBulk = table.Column<bool>(type: "bit", nullable: true),
                    PerBulk = table.Column<long>(type: "bigint", nullable: true),
                    IsPromotion = table.Column<bool>(type: "bit", nullable: true),
                    Product_Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitOfMeasureId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity = table.Column<double>(type: "float", nullable: true),
                    Variant = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Shop_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StockList_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    StockListProduct_Quantity = table.Column<long>(type: "bigint", nullable: true),
                    StockListProduct_ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StockListId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserStockList_StockListId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserStockList_IsActive = table.Column<bool>(type: "bit", nullable: true),
                    AddedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseAuditableEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseAuditableEntity_BaseAuditableEntity_LocationId",
                        column: x => x.LocationId,
                        principalTable: "BaseAuditableEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaseAuditableEntity_BaseAuditableEntity_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "BaseAuditableEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaseAuditableEntity_BaseAuditableEntity_ProductId",
                        column: x => x.ProductId,
                        principalTable: "BaseAuditableEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaseAuditableEntity_BaseAuditableEntity_ShopId",
                        column: x => x.ShopId,
                        principalTable: "BaseAuditableEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaseAuditableEntity_BaseAuditableEntity_StockListId",
                        column: x => x.StockListId,
                        principalTable: "BaseAuditableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BaseAuditableEntity_BaseAuditableEntity_StockListProduct_ProductId",
                        column: x => x.StockListProduct_ProductId,
                        principalTable: "BaseAuditableEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaseAuditableEntity_BaseAuditableEntity_UnitOfMeasureId",
                        column: x => x.UnitOfMeasureId,
                        principalTable: "BaseAuditableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BaseAuditableEntity_BaseAuditableEntity_UserId",
                        column: x => x.UserId,
                        principalTable: "BaseAuditableEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaseAuditableEntity_BaseAuditableEntity_UserStockList_StockListId",
                        column: x => x.UserStockList_StockListId,
                        principalTable: "BaseAuditableEntity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tag_BaseAuditableEntity_ProductId",
                        column: x => x.ProductId,
                        principalTable: "BaseAuditableEntity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseAuditableEntity_Abbreviation",
                table: "BaseAuditableEntity",
                column: "Abbreviation",
                unique: true,
                filter: "[Abbreviation] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BaseAuditableEntity_LocationId",
                table: "BaseAuditableEntity",
                column: "LocationId",
                unique: true,
                filter: "[LocationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BaseAuditableEntity_Name",
                table: "BaseAuditableEntity",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BaseAuditableEntity_OwnerId",
                table: "BaseAuditableEntity",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseAuditableEntity_Product_Name_Variant",
                table: "BaseAuditableEntity",
                columns: new[] { "Product_Name", "Variant" },
                unique: true,
                filter: "[Name] IS NOT NULL AND [Variant] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BaseAuditableEntity_ProductId",
                table: "BaseAuditableEntity",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseAuditableEntity_ShopId",
                table: "BaseAuditableEntity",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseAuditableEntity_StockListId",
                table: "BaseAuditableEntity",
                column: "StockListId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseAuditableEntity_StockListProduct_ProductId",
                table: "BaseAuditableEntity",
                column: "StockListProduct_ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseAuditableEntity_UnitOfMeasureId",
                table: "BaseAuditableEntity",
                column: "UnitOfMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseAuditableEntity_UserId_UserStockList_StockListId",
                table: "BaseAuditableEntity",
                columns: new[] { "UserId", "UserStockList_StockListId" },
                unique: true,
                filter: "[UserId] IS NOT NULL AND [StockListId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BaseAuditableEntity_UserStockList_StockListId",
                table: "BaseAuditableEntity",
                column: "UserStockList_StockListId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_ProductId",
                table: "Tag",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "BaseAuditableEntity");
        }
    }
}
