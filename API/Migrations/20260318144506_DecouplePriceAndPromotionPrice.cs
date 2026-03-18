using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class DecouplePriceAndPromotionPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseAuditableEntity_BaseAuditableEntity_LocationId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseAuditableEntity_BaseAuditableEntity_OwnerId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseAuditableEntity_BaseAuditableEntity_ProductId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseAuditableEntity_BaseAuditableEntity_ShopId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseAuditableEntity_BaseAuditableEntity_StockListId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseAuditableEntity_BaseAuditableEntity_StockListProduct_ProductId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseAuditableEntity_BaseAuditableEntity_UnitOfMeasureId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseAuditableEntity_BaseAuditableEntity_UserId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseAuditableEntity_BaseAuditableEntity_UserStockList_StockListId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_BaseAuditableEntity_ProductId",
                table: "Tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BaseAuditableEntity",
                table: "BaseAuditableEntity");

            migrationBuilder.DropIndex(
                name: "IX_BaseAuditableEntity_Abbreviation",
                table: "BaseAuditableEntity");

            migrationBuilder.DropIndex(
                name: "IX_BaseAuditableEntity_LocationId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropIndex(
                name: "IX_BaseAuditableEntity_Name",
                table: "BaseAuditableEntity");

            migrationBuilder.DropIndex(
                name: "IX_BaseAuditableEntity_OwnerId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropIndex(
                name: "IX_BaseAuditableEntity_Product_Name_Variant",
                table: "BaseAuditableEntity");

            migrationBuilder.DropIndex(
                name: "IX_BaseAuditableEntity_ProductId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropIndex(
                name: "IX_BaseAuditableEntity_ShopId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropIndex(
                name: "IX_BaseAuditableEntity_StockListProduct_ProductId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropIndex(
                name: "IX_BaseAuditableEntity_UnitOfMeasureId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropIndex(
                name: "IX_BaseAuditableEntity_UserId_UserStockList_StockListId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropIndex(
                name: "IX_BaseAuditableEntity_UserStockList_StockListId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "Abbreviation",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "IsBulk",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "IsPack",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "IsPromotion",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "PerBulk",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "PriceDate",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "PriceType",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "Product_Name",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "Shop_Name",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "StockListProduct_ProductId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "StockListProduct_Quantity",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "StockList_Name",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "UnitOfMeasureId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "UnitsPerPack",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "UserStockList_IsActive",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "UserStockList_StockListId",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "BaseAuditableEntity");

            migrationBuilder.DropColumn(
                name: "Variant",
                table: "BaseAuditableEntity");

            migrationBuilder.RenameTable(
                name: "BaseAuditableEntity",
                newName: "UserStockList");

            migrationBuilder.RenameIndex(
                name: "IX_BaseAuditableEntity_StockListId",
                table: "UserStockList",
                newName: "IX_UserStockList_StockListId");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserStockList",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "StockListId",
                table: "UserStockList",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserStockList",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AddedById",
                table: "UserStockList",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserStockList",
                table: "UserStockList",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeasurementUnit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementUnit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shop",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shop", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shop_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitOfMeasureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuantityPerUnit = table.Column<double>(type: "float", nullable: false),
                    Variants = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    UnitsPerPack = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_MeasurementUnit_UnitOfMeasureId",
                        column: x => x.UnitOfMeasureId,
                        principalTable: "MeasurementUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Guest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guest_User_Id",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockList_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAccount_User_Id",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Price",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PriceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Price", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Price_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Price_Shop_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shop",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StockListProduct",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockListProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockListProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockListProduct_StockList_StockListId",
                        column: x => x.StockListId,
                        principalTable: "StockList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromotionPrice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: true),
                    PriceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionPrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionPrice_Price_PriceId",
                        column: x => x.PriceId,
                        principalTable: "Price",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserStockList_UserId_StockListId",
                table: "UserStockList",
                columns: new[] { "UserId", "StockListId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementUnit_Abbreviation",
                table: "MeasurementUnit",
                column: "Abbreviation",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementUnit_Name",
                table: "MeasurementUnit",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Price_ProductId",
                table: "Price",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Price_ShopId",
                table: "Price",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_UnitOfMeasureId",
                table: "Product",
                column: "UnitOfMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionPrice_PriceId",
                table: "PromotionPrice",
                column: "PriceId");

            migrationBuilder.CreateIndex(
                name: "IX_Shop_LocationId",
                table: "Shop",
                column: "LocationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockList_OwnerId",
                table: "StockList",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_StockListProduct_ProductId",
                table: "StockListProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockListProduct_StockListId",
                table: "StockListProduct",
                column: "StockListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Product_ProductId",
                table: "Tag",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStockList_StockList_StockListId",
                table: "UserStockList",
                column: "StockListId",
                principalTable: "StockList",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStockList_User_UserId",
                table: "UserStockList",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Product_ProductId",
                table: "Tag");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStockList_StockList_StockListId",
                table: "UserStockList");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStockList_User_UserId",
                table: "UserStockList");

            migrationBuilder.DropTable(
                name: "Guest");

            migrationBuilder.DropTable(
                name: "PromotionPrice");

            migrationBuilder.DropTable(
                name: "StockListProduct");

            migrationBuilder.DropTable(
                name: "UserAccount");

            migrationBuilder.DropTable(
                name: "Price");

            migrationBuilder.DropTable(
                name: "StockList");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Shop");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "MeasurementUnit");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserStockList",
                table: "UserStockList");

            migrationBuilder.DropIndex(
                name: "IX_UserStockList_UserId_StockListId",
                table: "UserStockList");

            migrationBuilder.RenameTable(
                name: "UserStockList",
                newName: "BaseAuditableEntity");

            migrationBuilder.RenameIndex(
                name: "IX_UserStockList_StockListId",
                table: "BaseAuditableEntity",
                newName: "IX_BaseAuditableEntity_StockListId");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "BaseAuditableEntity",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "StockListId",
                table: "BaseAuditableEntity",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "BaseAuditableEntity",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<Guid>(
                name: "AddedById",
                table: "BaseAuditableEntity",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                table: "BaseAuditableEntity",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "BaseAuditableEntity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "BaseAuditableEntity",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BaseAuditableEntity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "BaseAuditableEntity",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBulk",
                table: "BaseAuditableEntity",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPack",
                table: "BaseAuditableEntity",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPromotion",
                table: "BaseAuditableEntity",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "BaseAuditableEntity",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "BaseAuditableEntity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "BaseAuditableEntity",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "BaseAuditableEntity",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BaseAuditableEntity",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "BaseAuditableEntity",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PerBulk",
                table: "BaseAuditableEntity",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PriceDate",
                table: "BaseAuditableEntity",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PriceType",
                table: "BaseAuditableEntity",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "BaseAuditableEntity",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Product_Name",
                table: "BaseAuditableEntity",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Quantity",
                table: "BaseAuditableEntity",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShopId",
                table: "BaseAuditableEntity",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Shop_Name",
                table: "BaseAuditableEntity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "BaseAuditableEntity",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StockListProduct_ProductId",
                table: "BaseAuditableEntity",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockListProduct_Quantity",
                table: "BaseAuditableEntity",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StockList_Name",
                table: "BaseAuditableEntity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UnitOfMeasureId",
                table: "BaseAuditableEntity",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UnitsPerPack",
                table: "BaseAuditableEntity",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "BaseAuditableEntity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UserStockList_IsActive",
                table: "BaseAuditableEntity",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserStockList_StockListId",
                table: "BaseAuditableEntity",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "BaseAuditableEntity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Variant",
                table: "BaseAuditableEntity",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BaseAuditableEntity",
                table: "BaseAuditableEntity",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_BaseAuditableEntity_BaseAuditableEntity_LocationId",
                table: "BaseAuditableEntity",
                column: "LocationId",
                principalTable: "BaseAuditableEntity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseAuditableEntity_BaseAuditableEntity_OwnerId",
                table: "BaseAuditableEntity",
                column: "OwnerId",
                principalTable: "BaseAuditableEntity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseAuditableEntity_BaseAuditableEntity_ProductId",
                table: "BaseAuditableEntity",
                column: "ProductId",
                principalTable: "BaseAuditableEntity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseAuditableEntity_BaseAuditableEntity_ShopId",
                table: "BaseAuditableEntity",
                column: "ShopId",
                principalTable: "BaseAuditableEntity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseAuditableEntity_BaseAuditableEntity_StockListId",
                table: "BaseAuditableEntity",
                column: "StockListId",
                principalTable: "BaseAuditableEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseAuditableEntity_BaseAuditableEntity_StockListProduct_ProductId",
                table: "BaseAuditableEntity",
                column: "StockListProduct_ProductId",
                principalTable: "BaseAuditableEntity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseAuditableEntity_BaseAuditableEntity_UnitOfMeasureId",
                table: "BaseAuditableEntity",
                column: "UnitOfMeasureId",
                principalTable: "BaseAuditableEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseAuditableEntity_BaseAuditableEntity_UserId",
                table: "BaseAuditableEntity",
                column: "UserId",
                principalTable: "BaseAuditableEntity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseAuditableEntity_BaseAuditableEntity_UserStockList_StockListId",
                table: "BaseAuditableEntity",
                column: "UserStockList_StockListId",
                principalTable: "BaseAuditableEntity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_BaseAuditableEntity_ProductId",
                table: "Tag",
                column: "ProductId",
                principalTable: "BaseAuditableEntity",
                principalColumn: "Id");
        }
    }
}
