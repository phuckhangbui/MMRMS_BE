using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class updateAccountBusinessRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountBusiness_Account",
                table: "AccountBusiness");

            migrationBuilder.DropIndex(
                name: "IX_AccountBusiness_AccountId",
                table: "AccountBusiness");

            migrationBuilder.AddColumn<int>(
                name: "AccountBusinessId",
                table: "Account",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountBusiness_AccountId",
                table: "AccountBusiness",
                column: "AccountId",
                unique: true,
                filter: "[AccountId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountBusiness_Account",
                table: "AccountBusiness",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountBusiness_Account",
                table: "AccountBusiness");

            migrationBuilder.DropIndex(
                name: "IX_AccountBusiness_AccountId",
                table: "AccountBusiness");

            migrationBuilder.DropColumn(
                name: "AccountBusinessId",
                table: "Account");

            migrationBuilder.CreateIndex(
                name: "IX_AccountBusiness_AccountId",
                table: "AccountBusiness",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountBusiness_Account",
                table: "AccountBusiness",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId");
        }
    }
}
