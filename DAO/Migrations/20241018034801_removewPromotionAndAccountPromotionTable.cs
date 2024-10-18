using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class removewPromotionAndAccountPromotionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountPromotion");

            migrationBuilder.DropTable(
                name: "Promotion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Promotion",
                columns: table => new
                {
                    PromotionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountPercentage = table.Column<double>(type: "float", nullable: true),
                    DiscountTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotion", x => x.PromotionId);
                });

            migrationBuilder.CreateTable(
                name: "AccountPromotion",
                columns: table => new
                {
                    AccountPromotionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    PromotionId = table.Column<int>(type: "int", nullable: true),
                    DateReceive = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPromotion", x => x.AccountPromotionId);
                    table.ForeignKey(
                        name: "FK_AccountPromotion_Account",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_AccountPromotion_Promotion",
                        column: x => x.PromotionId,
                        principalTable: "Promotion",
                        principalColumn: "PromotionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountPromotion_AccountId",
                table: "AccountPromotion",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountPromotion_PromotionId",
                table: "AccountPromotion",
                column: "PromotionId");
        }
    }
}
