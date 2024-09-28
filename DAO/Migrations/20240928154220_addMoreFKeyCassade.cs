using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class addMoreFKeyCassade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductComponentStatus_Log",
                table: "ProductComponentStatusLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImage_Product",
                table: "ProductImage");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComponentStatus_Log",
                table: "ProductComponentStatusLog",
                column: "ProductComponentStatusId",
                principalTable: "ProductComponentStatus",
                principalColumn: "ProductComponentStatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImage_Product",
                table: "ProductImage",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductComponentStatus_Log",
                table: "ProductComponentStatusLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImage_Product",
                table: "ProductImage");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComponentStatus_Log",
                table: "ProductComponentStatusLog",
                column: "ProductComponentStatusId",
                principalTable: "ProductComponentStatus",
                principalColumn: "ProductComponentStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImage_Product",
                table: "ProductImage",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId");
        }
    }
}
