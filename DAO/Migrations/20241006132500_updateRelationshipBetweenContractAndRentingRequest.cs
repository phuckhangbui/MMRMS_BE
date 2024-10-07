using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class updateRelationshipBetweenContractAndRentingRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contract_RentingRequestId",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "RentingRequest");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_RentingRequestId",
                table: "Contract",
                column: "RentingRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contract_RentingRequestId",
                table: "Contract");

            migrationBuilder.AddColumn<string>(
                name: "ContractId",
                table: "RentingRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contract_RentingRequestId",
                table: "Contract",
                column: "RentingRequestId",
                unique: true);
        }
    }
}
