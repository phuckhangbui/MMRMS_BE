using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class addRentingRequestAddressTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentingRequest_Address",
                table: "RentingRequest");

            migrationBuilder.DropIndex(
                name: "IX_RentingRequest_AddressId",
                table: "RentingRequest");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "RentingRequest");

            migrationBuilder.CreateTable(
                name: "RentingRequestAddress",
                columns: table => new
                {
                    RentingRequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AddressBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Coordinates = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentingRequestAddress", x => x.RentingRequestId);
                    table.ForeignKey(
                        name: "FK_RentingRequest_RentingRequestAddress",
                        column: x => x.RentingRequestId,
                        principalTable: "RentingRequest",
                        principalColumn: "RentingRequestId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentingRequestAddress");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "RentingRequest",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentingRequest_AddressId",
                table: "RentingRequest",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentingRequest_Address",
                table: "RentingRequest",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "AddressId");
        }
    }
}
