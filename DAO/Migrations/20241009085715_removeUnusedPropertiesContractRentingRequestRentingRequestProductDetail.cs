using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class removeUnusedPropertiesContractRentingRequestRentingRequestProductDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contract_Account_Create",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "DepositPrice",
                table: "RentingRequestProductDetail");

            migrationBuilder.DropColumn(
                name: "RentPrice",
                table: "RentingRequestProductDetail");

            migrationBuilder.RenameColumn(
                name: "AccountCreateId",
                table: "Contract",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Contract_AccountCreateId",
                table: "Contract",
                newName: "IX_Contract_AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_Account_AccountId",
                table: "Contract",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contract_Account_AccountId",
                table: "Contract");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Contract",
                newName: "AccountCreateId");

            migrationBuilder.RenameIndex(
                name: "IX_Contract_AccountId",
                table: "Contract",
                newName: "IX_Contract_AccountCreateId");

            migrationBuilder.AddColumn<double>(
                name: "DepositPrice",
                table: "RentingRequestProductDetail",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RentPrice",
                table: "RentingRequestProductDetail",
                type: "float",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_Account_Create",
                table: "Contract",
                column: "AccountCreateId",
                principalTable: "Account",
                principalColumn: "AccountId");
        }
    }
}
