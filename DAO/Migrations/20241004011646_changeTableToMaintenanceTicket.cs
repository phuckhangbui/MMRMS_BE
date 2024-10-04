using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class changeTableToMaintenanceTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaintainingTicket");

            migrationBuilder.CreateTable(
                name: "MaintenanceTicket",
                columns: table => new
                {
                    MaintenanceTicketId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeTaskId = table.Column<int>(type: "int", nullable: true),
                    EmployeeCreateId = table.Column<int>(type: "int", nullable: true),
                    ComponentId = table.Column<int>(type: "int", nullable: true),
                    InvoiceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProductSerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ComponentPrice = table.Column<double>(type: "float", nullable: true),
                    AdditionalFee = table.Column<double>(type: "float", nullable: true),
                    TotalAmount = table.Column<double>(type: "float", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateRepair = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Maintain__76F8D53F2FA1A432", x => x.MaintenanceTicketId);
                    table.ForeignKey(
                        name: "FK_Invoice_MaintainTicket",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId");
                    table.ForeignKey(
                        name: "FK_MaintenanceTicket_Account_EmployeeCreateId",
                        column: x => x.EmployeeCreateId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_MaintenanceTicket_ComponentID",
                        column: x => x.ComponentId,
                        principalTable: "Component",
                        principalColumn: "ComponentId");
                    table.ForeignKey(
                        name: "FK_MaintenanceTicket_SerialNumberProduct",
                        column: x => x.ProductSerialNumber,
                        principalTable: "SerialNumberProduct",
                        principalColumn: "SerialNumber");
                    table.ForeignKey(
                        name: "FK_MaintenanceTicket_TaskID",
                        column: x => x.EmployeeTaskId,
                        principalTable: "EmployeeTask",
                        principalColumn: "EmployeeTaskId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTicket_ComponentId",
                table: "MaintenanceTicket",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTicket_EmployeeCreateId",
                table: "MaintenanceTicket",
                column: "EmployeeCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTicket_EmployeeTaskId",
                table: "MaintenanceTicket",
                column: "EmployeeTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTicket_InvoiceId",
                table: "MaintenanceTicket",
                column: "InvoiceId",
                unique: true,
                filter: "[InvoiceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTicket_ProductSerialNumber",
                table: "MaintenanceTicket",
                column: "ProductSerialNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaintenanceTicket");

            migrationBuilder.CreateTable(
                name: "MaintainingTicket",
                columns: table => new
                {
                    MaintainingTicketId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComponentId = table.Column<int>(type: "int", nullable: true),
                    EmployeeCreateId = table.Column<int>(type: "int", nullable: true),
                    EmployeeTaskId = table.Column<int>(type: "int", nullable: true),
                    InvoiceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProductSerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AdditionalFee = table.Column<double>(type: "float", nullable: true),
                    ComponentPrice = table.Column<double>(type: "float", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateRepair = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAmount = table.Column<double>(type: "float", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Maintain__76F8D53F2FA1A432", x => x.MaintainingTicketId);
                    table.ForeignKey(
                        name: "FK_Invoice_MaintainTicket",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId");
                    table.ForeignKey(
                        name: "FK_MaintainingTicket_Account_EmployeeCreateId",
                        column: x => x.EmployeeCreateId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_MaintainingTicket_ComponentID",
                        column: x => x.ComponentId,
                        principalTable: "Component",
                        principalColumn: "ComponentId");
                    table.ForeignKey(
                        name: "FK_MaintainingTicket_SerialNumberProduct",
                        column: x => x.ProductSerialNumber,
                        principalTable: "SerialNumberProduct",
                        principalColumn: "SerialNumber");
                    table.ForeignKey(
                        name: "FK_MaintainingTicket_TaskID",
                        column: x => x.EmployeeTaskId,
                        principalTable: "EmployeeTask",
                        principalColumn: "EmployeeTaskId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaintainingTicket_ComponentId",
                table: "MaintainingTicket",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintainingTicket_EmployeeCreateId",
                table: "MaintainingTicket",
                column: "EmployeeCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintainingTicket_EmployeeTaskId",
                table: "MaintainingTicket",
                column: "EmployeeTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintainingTicket_InvoiceId",
                table: "MaintainingTicket",
                column: "InvoiceId",
                unique: true,
                filter: "[InvoiceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MaintainingTicket_ProductSerialNumber",
                table: "MaintainingTicket",
                column: "ProductSerialNumber");
        }
    }
}
