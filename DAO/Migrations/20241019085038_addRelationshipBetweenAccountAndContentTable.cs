using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class addRelationshipBetweenAccountAndContentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Content",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Content_AccountId",
                table: "Content",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Content_Account",
                table: "Content",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Content_Account",
                table: "Content");

            migrationBuilder.DropIndex(
                name: "IX_Content_AccountId",
                table: "Content");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Content");
        }
    }
}
