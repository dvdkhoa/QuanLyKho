using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyKho.Migrations
{
    public partial class Init_luanvanB2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Customer_CustomerId",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_CartDetail_Cart_CartId",
                table: "CartDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_CartDetail_Products_ProductId",
                table: "CartDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDetailedConfig_DetailedConfig_ConfigId",
                table: "CategoryDetailedConfig");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductClassification_Classification_ClassificationId",
                table: "ProductClassification");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductClassification_Products_ProductId",
                table: "ProductClassification");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImage_Products_ProductId",
                table: "ProductImage");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPromotion_Products_ProductId",
                table: "ProductPromotion");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPromotion_Promotion_PromotionId",
                table: "ProductPromotion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Promotion",
                table: "Promotion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPromotion",
                table: "ProductPromotion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImage",
                table: "ProductImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductClassification",
                table: "ProductClassification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_New",
                table: "New");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetailedConfig",
                table: "DetailedConfig");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Classification",
                table: "Classification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartDetail",
                table: "CartDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cart",
                table: "Cart");

            migrationBuilder.RenameTable(
                name: "Promotion",
                newName: "Promotions");

            migrationBuilder.RenameTable(
                name: "ProductPromotion",
                newName: "ProductPromotions");

            migrationBuilder.RenameTable(
                name: "ProductImage",
                newName: "ProductImages");

            migrationBuilder.RenameTable(
                name: "ProductClassification",
                newName: "ProductClassifications");

            migrationBuilder.RenameTable(
                name: "New",
                newName: "News");

            migrationBuilder.RenameTable(
                name: "DetailedConfig",
                newName: "DetailedConfigs");

            migrationBuilder.RenameTable(
                name: "Classification",
                newName: "Classifications");

            migrationBuilder.RenameTable(
                name: "CartDetail",
                newName: "CartDetails");

            migrationBuilder.RenameTable(
                name: "Cart",
                newName: "Carts");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPromotion_PromotionId",
                table: "ProductPromotions",
                newName: "IX_ProductPromotions_PromotionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductImage_ProductId",
                table: "ProductImages",
                newName: "IX_ProductImages_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductClassification_ProductId",
                table: "ProductClassifications",
                newName: "IX_ProductClassifications_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductClassification_ClassificationId",
                table: "ProductClassifications",
                newName: "IX_ProductClassifications_ClassificationId");

            migrationBuilder.RenameIndex(
                name: "IX_CartDetail_ProductId",
                table: "CartDetails",
                newName: "IX_CartDetails_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Cart_CustomerId",
                table: "Carts",
                newName: "IX_Carts_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Promotions",
                table: "Promotions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPromotions",
                table: "ProductPromotions",
                columns: new[] { "ProductId", "PromotionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImages",
                table: "ProductImages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductClassifications",
                table: "ProductClassifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_News",
                table: "News",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetailedConfigs",
                table: "DetailedConfigs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Classifications",
                table: "Classifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartDetails",
                table: "CartDetails",
                columns: new[] { "CartId", "ProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Carts",
                table: "Carts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_Carts_CartId",
                table: "CartDetails",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_Products_ProductId",
                table: "CartDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Customer_CustomerId",
                table: "Carts",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDetailedConfig_DetailedConfigs_ConfigId",
                table: "CategoryDetailedConfig",
                column: "ConfigId",
                principalTable: "DetailedConfigs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductClassifications_Classifications_ClassificationId",
                table: "ProductClassifications",
                column: "ClassificationId",
                principalTable: "Classifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductClassifications_Products_ProductId",
                table: "ProductClassifications",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPromotions_Products_ProductId",
                table: "ProductPromotions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPromotions_Promotions_PromotionId",
                table: "ProductPromotions",
                column: "PromotionId",
                principalTable: "Promotions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_Carts_CartId",
                table: "CartDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_Products_ProductId",
                table: "CartDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Customer_CustomerId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDetailedConfig_DetailedConfigs_ConfigId",
                table: "CategoryDetailedConfig");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductClassifications_Classifications_ClassificationId",
                table: "ProductClassifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductClassifications_Products_ProductId",
                table: "ProductClassifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPromotions_Products_ProductId",
                table: "ProductPromotions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPromotions_Promotions_PromotionId",
                table: "ProductPromotions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Promotions",
                table: "Promotions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPromotions",
                table: "ProductPromotions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImages",
                table: "ProductImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductClassifications",
                table: "ProductClassifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_News",
                table: "News");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetailedConfigs",
                table: "DetailedConfigs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Classifications",
                table: "Classifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Carts",
                table: "Carts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartDetails",
                table: "CartDetails");

            migrationBuilder.RenameTable(
                name: "Promotions",
                newName: "Promotion");

            migrationBuilder.RenameTable(
                name: "ProductPromotions",
                newName: "ProductPromotion");

            migrationBuilder.RenameTable(
                name: "ProductImages",
                newName: "ProductImage");

            migrationBuilder.RenameTable(
                name: "ProductClassifications",
                newName: "ProductClassification");

            migrationBuilder.RenameTable(
                name: "News",
                newName: "New");

            migrationBuilder.RenameTable(
                name: "DetailedConfigs",
                newName: "DetailedConfig");

            migrationBuilder.RenameTable(
                name: "Classifications",
                newName: "Classification");

            migrationBuilder.RenameTable(
                name: "Carts",
                newName: "Cart");

            migrationBuilder.RenameTable(
                name: "CartDetails",
                newName: "CartDetail");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPromotions_PromotionId",
                table: "ProductPromotion",
                newName: "IX_ProductPromotion_PromotionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImage",
                newName: "IX_ProductImage_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductClassifications_ProductId",
                table: "ProductClassification",
                newName: "IX_ProductClassification_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductClassifications_ClassificationId",
                table: "ProductClassification",
                newName: "IX_ProductClassification_ClassificationId");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_CustomerId",
                table: "Cart",
                newName: "IX_Cart_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_CartDetails_ProductId",
                table: "CartDetail",
                newName: "IX_CartDetail_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Promotion",
                table: "Promotion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPromotion",
                table: "ProductPromotion",
                columns: new[] { "ProductId", "PromotionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImage",
                table: "ProductImage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductClassification",
                table: "ProductClassification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_New",
                table: "New",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetailedConfig",
                table: "DetailedConfig",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Classification",
                table: "Classification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cart",
                table: "Cart",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartDetail",
                table: "CartDetail",
                columns: new[] { "CartId", "ProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Customer_CustomerId",
                table: "Cart",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetail_Cart_CartId",
                table: "CartDetail",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetail_Products_ProductId",
                table: "CartDetail",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDetailedConfig_DetailedConfig_ConfigId",
                table: "CategoryDetailedConfig",
                column: "ConfigId",
                principalTable: "DetailedConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductClassification_Classification_ClassificationId",
                table: "ProductClassification",
                column: "ClassificationId",
                principalTable: "Classification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductClassification_Products_ProductId",
                table: "ProductClassification",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImage_Products_ProductId",
                table: "ProductImage",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPromotion_Products_ProductId",
                table: "ProductPromotion",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPromotion_Promotion_PromotionId",
                table: "ProductPromotion",
                column: "PromotionId",
                principalTable: "Promotion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
