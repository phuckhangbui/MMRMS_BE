using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class updateRentingRequestAndContractAndRemoveRentingRequestMachineDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentingRequestMachineDetail");

            migrationBuilder.DropColumn(
                name: "DateEnd",
                table: "RentingRequest");

            migrationBuilder.DropColumn(
                name: "DateStart",
                table: "RentingRequest");

            migrationBuilder.DropColumn(
                name: "NumberOfMonth",
                table: "RentingRequest");

            migrationBuilder.DropColumn(
                name: "NumberOfMonth",
                table: "Contract");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateEnd",
                table: "RentingRequest",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateStart",
                table: "RentingRequest",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfMonth",
                table: "RentingRequest",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfMonth",
                table: "Contract",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RentingRequestMachineDetail",
                columns: table => new
                {
                    RentingRequestMachineDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineId = table.Column<int>(type: "int", nullable: true),
                    RentingRequestId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentingRequestMachineDetail", x => x.RentingRequestMachineDetailId);
                    table.ForeignKey(
                        name: "FK_RentingRequestMachineDetail_Machine",
                        column: x => x.MachineId,
                        principalTable: "Machine",
                        principalColumn: "MachineId");
                    table.ForeignKey(
                        name: "FK_RentingRequestMachineDetail_RentingRequest",
                        column: x => x.RentingRequestId,
                        principalTable: "RentingRequest",
                        principalColumn: "RentingRequestId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentingRequestMachineDetail_MachineId",
                table: "RentingRequestMachineDetail",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_RentingRequestMachineDetail_RentingRequestId",
                table: "RentingRequestMachineDetail",
                column: "RentingRequestId");
        }
    }
}
