using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class updateReferenceContractRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaseContractId",
                table: "Contract",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contract_BaseContractId",
                table: "Contract",
                column: "BaseContractId",
                unique: true,
                filter: "[BaseContractId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_BaseContract",
                table: "Contract",
                column: "BaseContractId",
                principalTable: "Contract",
                principalColumn: "ContractId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contract_BaseContract",
                table: "Contract");

            migrationBuilder.DropIndex(
                name: "IX_Contract_BaseContractId",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "BaseContractId",
                table: "Contract");
        }
    }
}
