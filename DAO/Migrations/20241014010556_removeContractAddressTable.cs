using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class removeContractAddressTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractAddress_Contract",
                table: "Contract");

            migrationBuilder.DropTable(
                name: "ContractAddress");

            migrationBuilder.DropIndex(
                name: "IX_Contract_ContractAddressId",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "ContractAddressId",
                table: "Contract");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContractAddressId",
                table: "Contract",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ContractAddress",
                columns: table => new
                {
                    ContractAddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractAddress", x => x.ContractAddressId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contract_ContractAddressId",
                table: "Contract",
                column: "ContractAddressId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractAddress_Contract",
                table: "Contract",
                column: "ContractAddressId",
                principalTable: "ContractAddress",
                principalColumn: "ContractAddressId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
