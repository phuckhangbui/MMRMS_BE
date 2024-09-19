using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class updatePromotion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionPromotion",
                table: "Promotion");

            migrationBuilder.DropColumn(
                name: "DiscountTypeId",
                table: "Promotion");

            migrationBuilder.DropColumn(
                name: "PromotionTypeId",
                table: "Promotion");

            migrationBuilder.RenameColumn(
                name: "PromotionPack",
                table: "Promotion",
                newName: "DiscountTypeName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DiscountTypeName",
                table: "Promotion",
                newName: "PromotionPack");

            migrationBuilder.AddColumn<int>(
                name: "ActionPromotion",
                table: "Promotion",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountTypeId",
                table: "Promotion",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PromotionTypeId",
                table: "Promotion",
                type: "int",
                nullable: true);
        }
    }
}
