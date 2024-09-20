using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class updatePromotionRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Promotion_PromotionId",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_PromotionId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "PromotionId",
                table: "Account");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Promotion",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Promotion");

            migrationBuilder.AddColumn<int>(
                name: "PromotionId",
                table: "Account",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_PromotionId",
                table: "Account",
                column: "PromotionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Promotion_PromotionId",
                table: "Account",
                column: "PromotionId",
                principalTable: "Promotion",
                principalColumn: "PromotionId");
        }
    }
}
