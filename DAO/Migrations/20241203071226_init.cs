﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Component",
                columns: table => new
                {
                    ComponentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComponentName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    QuantityOnHold = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Component", x => x.ComponentId);
                });

            migrationBuilder.CreateTable(
                name: "MachineCheckCriteria",
                columns: table => new
                {
                    MachineCheckCriteriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineCheckCriteria", x => x.MachineCheckCriteriaId);
                });

            migrationBuilder.CreateTable(
                name: "MembershipRank",
                columns: table => new
                {
                    MembershipRankId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MembershipRankName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    MoneySpent = table.Column<double>(type: "float", nullable: true),
                    DiscountPercentage = table.Column<double>(type: "float", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipRank", x => x.MembershipRankId);
                });

            migrationBuilder.CreateTable(
                name: "RentingService",
                columns: table => new
                {
                    RentingServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentingServiceName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    IsOptional = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentingService", x => x.RentingServiceId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Term",
                columns: table => new
                {
                    TermId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Term", x => x.TermId);
                });

            migrationBuilder.CreateTable(
                name: "Machine",
                columns: table => new
                {
                    MachineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineName = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    RentPrice = table.Column<double>(type: "float", nullable: true),
                    Weight = table.Column<double>(type: "float", nullable: true),
                    MachinePrice = table.Column<double>(type: "float", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Origin = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machine", x => x.MachineId);
                    table.ForeignKey(
                        name: "FK_Machine_Category",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId");
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvatarImg = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DateBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    OtpNumber = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    FirebaseMessageToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenRefresh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenDateExpire = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MembershipRankId = table.Column<int>(type: "int", nullable: true),
                    AccountBusinessId = table.Column<int>(type: "int", nullable: true),
                    LogId = table.Column<int>(type: "int", nullable: true),
                    MoneySpent = table.Column<double>(type: "float", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateExpire = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Account_MembershipRank",
                        column: x => x.MembershipRankId,
                        principalTable: "MembershipRank",
                        principalColumn: "MembershipRankId");
                    table.ForeignKey(
                        name: "FK_Account_Role",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "MachineAttribute",
                columns: table => new
                {
                    MachineAttributeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineId = table.Column<int>(type: "int", nullable: true),
                    AttributeName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Specifications = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineAttribute", x => x.MachineAttributeId);
                    table.ForeignKey(
                        name: "FK_Attribute_Machine",
                        column: x => x.MachineId,
                        principalTable: "Machine",
                        principalColumn: "MachineId");
                });

            migrationBuilder.CreateTable(
                name: "MachineComponent",
                columns: table => new
                {
                    MachineComponentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineId = table.Column<int>(type: "int", nullable: true),
                    ComponentId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineComponent", x => x.MachineComponentId);
                    table.ForeignKey(
                        name: "FK_MachineComponent_ComponentID",
                        column: x => x.ComponentId,
                        principalTable: "Component",
                        principalColumn: "ComponentId");
                    table.ForeignKey(
                        name: "FK_MachineComponent_MachineID",
                        column: x => x.MachineId,
                        principalTable: "Machine",
                        principalColumn: "MachineId");
                });

            migrationBuilder.CreateTable(
                name: "MachineImage",
                columns: table => new
                {
                    MachineImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineId = table.Column<int>(type: "int", nullable: true),
                    MachineImageUrl = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    IsThumbnail = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineImage", x => x.MachineImageId);
                    table.ForeignKey(
                        name: "FK_MachineImage_Machine",
                        column: x => x.MachineId,
                        principalTable: "Machine",
                        principalColumn: "MachineId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MachineSerialNumber",
                columns: table => new
                {
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    MachineId = table.Column<int>(type: "int", nullable: true),
                    ActualRentPrice = table.Column<double>(type: "float", nullable: true),
                    MachineConditionPercent = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpectedAvailableDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RentDaysCounter = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineSerialNumber", x => x.SerialNumber);
                    table.ForeignKey(
                        name: "FK_MachineNumber_Machine",
                        column: x => x.MachineId,
                        principalTable: "Machine",
                        principalColumn: "MachineId");
                });

            migrationBuilder.CreateTable(
                name: "MachineTerm",
                columns: table => new
                {
                    MachineTermId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineTerm", x => x.MachineTermId);
                    table.ForeignKey(
                        name: "FK_Term_Machine",
                        column: x => x.MachineId,
                        principalTable: "Machine",
                        principalColumn: "MachineId");
                });

            migrationBuilder.CreateTable(
                name: "AccountBusiness",
                columns: table => new
                {
                    AccountBusinessId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    Company = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    TaxNumber = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountBusiness", x => x.AccountBusinessId);
                    table.ForeignKey(
                        name: "FK_AccountBusiness_Account",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    AddressBody = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Coordinates = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_Address_Account",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "Content",
                columns: table => new
                {
                    ContentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Content", x => x.ContentId);
                    table.ForeignKey(
                        name: "FK_Content_Account",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "DeliveryTask",
                columns: table => new
                {
                    DeliveryTaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffId = table.Column<int>(type: "int", nullable: true),
                    ManagerId = table.Column<int>(type: "int", nullable: true),
                    DateShip = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCompleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfirmationPictureUrl = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    ReceiverName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DeliveryVehicleCounter = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryTask", x => x.DeliveryTaskId);
                    table.ForeignKey(
                        name: "FK_DeliveryTask_ManagerID",
                        column: x => x.ManagerId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_DeliveryTask_StaffID",
                        column: x => x.StaffId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    AccountPaidId = table.Column<int>(type: "int", nullable: true),
                    ComponentReplacementTicketId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DigitalTransactionId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DatePaid = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayOsOrderId = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_Invoices_Account",
                        column: x => x.AccountPaidId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "LogDetail",
                columns: table => new
                {
                    LogDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogDetail", x => x.LogDetailId);
                    table.ForeignKey(
                        name: "FK_LogDetail_AccountID",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "MembershipRankLog",
                columns: table => new
                {
                    MembershipRankLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MembershipRankId = table.Column<int>(type: "int", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipRankLog", x => x.MembershipRankLogId);
                    table.ForeignKey(
                        name: "FK_MembershipRank_membershiplog",
                        column: x => x.MembershipRankId,
                        principalTable: "MembershipRank",
                        principalColumn: "MembershipRankId");
                    table.ForeignKey(
                        name: "FK_account_membershiplog",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountReceiveId = table.Column<int>(type: "int", nullable: true),
                    NotificationType = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    NotificationTitle = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    MessageNotification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateRead = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DetailId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DetailIdName = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notification_Account",
                        column: x => x.AccountReceiveId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "RentingRequest",
                columns: table => new
                {
                    RentingRequestId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    AccountOrderId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalRentPrice = table.Column<double>(type: "float", nullable: true),
                    TotalDepositPrice = table.Column<double>(type: "float", nullable: true),
                    TotalServicePrice = table.Column<double>(type: "float", nullable: true),
                    ShippingPrice = table.Column<double>(type: "float", nullable: true),
                    DiscountPrice = table.Column<double>(type: "float", nullable: true),
                    TotalAmount = table.Column<double>(type: "float", nullable: true),
                    IsOnetimePayment = table.Column<bool>(type: "bit", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    BeneficiaryBank = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    BeneficiaryName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ShippingDistance = table.Column<double>(type: "float", nullable: true),
                    ShippingPricePerKm = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentingRequest", x => x.RentingRequestId);
                    table.ForeignKey(
                        name: "FK_RentingRequest_Account",
                        column: x => x.AccountOrderId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "MachineSerialNumberComponent",
                columns: table => new
                {
                    MachineSerialNumberComponentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    MachineComponentId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineSerialNumberComponent", x => x.MachineSerialNumberComponentId);
                    table.ForeignKey(
                        name: "FK_MachineSerialNumberComponent_ComponentID",
                        column: x => x.MachineComponentId,
                        principalTable: "MachineComponent",
                        principalColumn: "MachineComponentId");
                    table.ForeignKey(
                        name: "FK_MachineSerialNumberComponent_MachineSerialNumber",
                        column: x => x.SerialNumber,
                        principalTable: "MachineSerialNumber",
                        principalColumn: "SerialNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryTaskLog",
                columns: table => new
                {
                    DeliveryTaskLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryTaskId = table.Column<int>(type: "int", nullable: true),
                    AccountTriggerId = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryTaskLog", x => x.DeliveryTaskLogId);
                    table.ForeignKey(
                        name: "FK_DeliveryTaskLog_AccountID",
                        column: x => x.AccountTriggerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryTaskLog_DeliveryTaskID",
                        column: x => x.DeliveryTaskId,
                        principalTable: "DeliveryTask",
                        principalColumn: "DeliveryTaskId");
                });

            migrationBuilder.CreateTable(
                name: "DigitalTransactions",
                columns: table => new
                {
                    DigitalTransactionId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    InvoiceId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    AccountName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    BankCode = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PayOsOrderId = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigitalTransactions", x => x.DigitalTransactionId);
                    table.ForeignKey(
                        name: "FK_DigitalTransaction_Invoice",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId");
                });

            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    ContractId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ContractName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    AccountSignId = table.Column<int>(type: "int", nullable: true),
                    RentingRequestId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    RentPrice = table.Column<double>(type: "float", nullable: true),
                    DepositPrice = table.Column<double>(type: "float", nullable: true),
                    RentPeriod = table.Column<int>(type: "int", nullable: true),
                    TotalRentPrice = table.Column<double>(type: "float", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefundShippingPrice = table.Column<double>(type: "float", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateSign = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    BaseContractId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    IsExtended = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.ContractId);
                    table.ForeignKey(
                        name: "FK_Contract_Account",
                        column: x => x.AccountSignId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_Contract_BaseContract",
                        column: x => x.BaseContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId");
                    table.ForeignKey(
                        name: "FK_Contract_MachineSerialNumber",
                        column: x => x.SerialNumber,
                        principalTable: "MachineSerialNumber",
                        principalColumn: "SerialNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentingRequest_Contract",
                        column: x => x.RentingRequestId,
                        principalTable: "RentingRequest",
                        principalColumn: "RentingRequestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RentingRequestAddress",
                columns: table => new
                {
                    RentingRequestId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    AddressBody = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Coordinates = table.Column<string>(type: "nvarchar(100)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "ServiceRentingRequest",
                columns: table => new
                {
                    ServiceRentingRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentingServiceId = table.Column<int>(type: "int", nullable: true),
                    RentingRequestId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ServicePrice = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRentingRequest", x => x.ServiceRentingRequestId);
                    table.ForeignKey(
                        name: "FK_rentingservice_servicerequest",
                        column: x => x.RentingServiceId,
                        principalTable: "RentingService",
                        principalColumn: "RentingServiceId");
                    table.ForeignKey(
                        name: "FK_servicerequest_rentingrequest",
                        column: x => x.RentingRequestId,
                        principalTable: "RentingRequest",
                        principalColumn: "RentingRequestId");
                });

            migrationBuilder.CreateTable(
                name: "MachineSerialNumberLog",
                columns: table => new
                {
                    MachineSerialNumberLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    MachineSerialNumberComponentId = table.Column<int>(type: "int", nullable: true),
                    AccountTriggerId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineSerialNumberLog", x => x.MachineSerialNumberLogId);
                    table.ForeignKey(
                        name: "FK_MachineSerialNumberComponent_Log",
                        column: x => x.MachineSerialNumberComponentId,
                        principalTable: "MachineSerialNumberComponent",
                        principalColumn: "MachineSerialNumberComponentId");
                    table.ForeignKey(
                        name: "FK_MachineSerialNumberLog_AccountID",
                        column: x => x.AccountTriggerId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_MachineSerialNumber_Log",
                        column: x => x.SerialNumber,
                        principalTable: "MachineSerialNumber",
                        principalColumn: "SerialNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractDelivery",
                columns: table => new
                {
                    ContractDeliveryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DeliveryTaskId = table.Column<int>(type: "int", nullable: true),
                    PictureUrl = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractDelivery", x => x.ContractDeliveryId);
                    table.ForeignKey(
                        name: "FK_Contract_ContractDelivery",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId");
                    table.ForeignKey(
                        name: "FK_DeliveryTask_ContractDelivery",
                        column: x => x.DeliveryTaskId,
                        principalTable: "DeliveryTask",
                        principalColumn: "DeliveryTaskId");
                });

            migrationBuilder.CreateTable(
                name: "ContractPayment",
                columns: table => new
                {
                    ContractPaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    InvoiceId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: true),
                    CustomerPaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Period = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsFirstRentalPayment = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractPayment", x => x.ContractPaymentId);
                    table.ForeignKey(
                        name: "FK_ContractPayment_ContractID",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId");
                    table.ForeignKey(
                        name: "FK_Invoice_ContractPayment",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId");
                });

            migrationBuilder.CreateTable(
                name: "ContractTerm",
                columns: table => new
                {
                    ContractTermId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractTerm", x => x.ContractTermId);
                    table.ForeignKey(
                        name: "FK_ContractTerm_ContractID",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId");
                });

            migrationBuilder.CreateTable(
                name: "MachineCheckRequest",
                columns: table => new
                {
                    MachineCheckRequestId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ContractId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    MachineTaskId = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineCheckRequest", x => x.MachineCheckRequestId);
                    table.ForeignKey(
                        name: "FK_MachineCheckRequest_Contract",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId");
                });

            migrationBuilder.CreateTable(
                name: "MachineCheckRequestCriteria",
                columns: table => new
                {
                    MachineCheckRequestCriteriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineCheckRequestId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CriteriaName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CustomerNote = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineCheckRequestCriteria", x => x.MachineCheckRequestCriteriaId);
                    table.ForeignKey(
                        name: "FK_MachineCheckRequestCriteria_MachineCheckRequest",
                        column: x => x.MachineCheckRequestId,
                        principalTable: "MachineCheckRequest",
                        principalColumn: "MachineCheckRequestId");
                });

            migrationBuilder.CreateTable(
                name: "MachineTask",
                columns: table => new
                {
                    MachineTaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskTitle = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ContractId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    MachineCheckRequestId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StaffId = table.Column<int>(type: "int", nullable: true),
                    ManagerId = table.Column<int>(type: "int", nullable: true),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCompleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ConfirmationPictureUrl = table.Column<string>(type: "nvarchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineTask", x => x.MachineTaskId);
                    table.ForeignKey(
                        name: "FK_Task_Contract",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId");
                    table.ForeignKey(
                        name: "FK_Task_MachineCheckRequest",
                        column: x => x.MachineCheckRequestId,
                        principalTable: "MachineCheckRequest",
                        principalColumn: "MachineCheckRequestId");
                    table.ForeignKey(
                        name: "FK_Task_Manager",
                        column: x => x.ManagerId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_Task_Staff",
                        column: x => x.StaffId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "ComponentReplacementTicket",
                columns: table => new
                {
                    ComponentReplacementTicketId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    EmployeeCreateId = table.Column<int>(type: "int", nullable: true),
                    MachineTaskCreateId = table.Column<int>(type: "int", nullable: true),
                    ContractId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ComponentId = table.Column<int>(type: "int", nullable: true),
                    InvoiceId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    MachineSerialNumberComponentId = table.Column<int>(type: "int", nullable: true),
                    ComponentPrice = table.Column<double>(type: "float", nullable: true),
                    AdditionalFee = table.Column<double>(type: "float", nullable: true),
                    TotalAmount = table.Column<double>(type: "float", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateRepair = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentReplacementTicket", x => x.ComponentReplacementTicketId);
                    table.ForeignKey(
                        name: "FK_ComponentReplacementTicket_Account_EmployeeCreateId",
                        column: x => x.EmployeeCreateId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_ComponentReplacementTicket_ComponentID",
                        column: x => x.ComponentId,
                        principalTable: "Component",
                        principalColumn: "ComponentId");
                    table.ForeignKey(
                        name: "FK_ComponentReplacementTicket_ContractID",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId");
                    table.ForeignKey(
                        name: "FK_ComponentReplacementTicket_MachineSerialNumberComponent",
                        column: x => x.MachineSerialNumberComponentId,
                        principalTable: "MachineSerialNumberComponent",
                        principalColumn: "MachineSerialNumberComponentId");
                    table.ForeignKey(
                        name: "FK_ComponentReplacementTicket_MachineTaskCreated",
                        column: x => x.MachineTaskCreateId,
                        principalTable: "MachineTask",
                        principalColumn: "MachineTaskId");
                    table.ForeignKey(
                        name: "FK_Invoice_MaintainTicket",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId");
                });

            migrationBuilder.CreateTable(
                name: "MachineTaskLog",
                columns: table => new
                {
                    MachineTaskLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineTaskId = table.Column<int>(type: "int", nullable: true),
                    AccountTriggerId = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineTaskLog", x => x.MachineTaskLogId);
                    table.ForeignKey(
                        name: "FK_MachineTaskLog_AccountID",
                        column: x => x.AccountTriggerId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_MachineTaskLog_TaskID",
                        column: x => x.MachineTaskId,
                        principalTable: "MachineTask",
                        principalColumn: "MachineTaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComponentReplacementTicketLog",
                columns: table => new
                {
                    ComponentReplacementTicketLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComponentReplacementTicketId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    AccountTriggerId = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentReplacementTicketLog", x => x.ComponentReplacementTicketLogId);
                    table.ForeignKey(
                        name: "FK_ComponentReplacementTicketLog_Account_AccountTriggerId",
                        column: x => x.AccountTriggerId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_ComponentReplacementTicket_Log",
                        column: x => x.ComponentReplacementTicketId,
                        principalTable: "ComponentReplacementTicket",
                        principalColumn: "ComponentReplacementTicketId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_MembershipRankId",
                table: "Account",
                column: "MembershipRankId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_RoleId",
                table: "Account",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountBusiness_AccountId",
                table: "AccountBusiness",
                column: "AccountId",
                unique: true,
                filter: "[AccountId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Address_AccountId",
                table: "Address",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentReplacementTicket_ComponentId",
                table: "ComponentReplacementTicket",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentReplacementTicket_ContractId",
                table: "ComponentReplacementTicket",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentReplacementTicket_EmployeeCreateId",
                table: "ComponentReplacementTicket",
                column: "EmployeeCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentReplacementTicket_InvoiceId",
                table: "ComponentReplacementTicket",
                column: "InvoiceId",
                unique: true,
                filter: "[InvoiceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentReplacementTicket_MachineSerialNumberComponentId",
                table: "ComponentReplacementTicket",
                column: "MachineSerialNumberComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentReplacementTicket_MachineTaskCreateId",
                table: "ComponentReplacementTicket",
                column: "MachineTaskCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentReplacementTicketLog_AccountTriggerId",
                table: "ComponentReplacementTicketLog",
                column: "AccountTriggerId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentReplacementTicketLog_ComponentReplacementTicketId",
                table: "ComponentReplacementTicketLog",
                column: "ComponentReplacementTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Content_AccountId",
                table: "Content",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_AccountSignId",
                table: "Contract",
                column: "AccountSignId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_BaseContractId",
                table: "Contract",
                column: "BaseContractId",
                unique: true,
                filter: "[BaseContractId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_RentingRequestId",
                table: "Contract",
                column: "RentingRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_SerialNumber",
                table: "Contract",
                column: "SerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ContractDelivery_ContractId",
                table: "ContractDelivery",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractDelivery_DeliveryTaskId",
                table: "ContractDelivery",
                column: "DeliveryTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractPayment_ContractId",
                table: "ContractPayment",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractPayment_InvoiceId",
                table: "ContractPayment",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractTerm_ContractId",
                table: "ContractTerm",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTask_ManagerId",
                table: "DeliveryTask",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTask_StaffId",
                table: "DeliveryTask",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTaskLog_AccountTriggerId",
                table: "DeliveryTaskLog",
                column: "AccountTriggerId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTaskLog_DeliveryTaskId",
                table: "DeliveryTaskLog",
                column: "DeliveryTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_DigitalTransactions_InvoiceId",
                table: "DigitalTransactions",
                column: "InvoiceId",
                unique: true,
                filter: "[InvoiceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_AccountPaidId",
                table: "Invoices",
                column: "AccountPaidId");

            migrationBuilder.CreateIndex(
                name: "IX_LogDetail_AccountId",
                table: "LogDetail",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Machine_CategoryId",
                table: "Machine",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineAttribute_MachineId",
                table: "MachineAttribute",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineCheckRequest_ContractId",
                table: "MachineCheckRequest",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineCheckRequestCriteria_MachineCheckRequestId",
                table: "MachineCheckRequestCriteria",
                column: "MachineCheckRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineComponent_ComponentId",
                table: "MachineComponent",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineComponent_MachineId",
                table: "MachineComponent",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineImage_MachineId",
                table: "MachineImage",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineSerialNumber_MachineId",
                table: "MachineSerialNumber",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineSerialNumberComponent_MachineComponentId",
                table: "MachineSerialNumberComponent",
                column: "MachineComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineSerialNumberComponent_SerialNumber",
                table: "MachineSerialNumberComponent",
                column: "SerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_MachineSerialNumberLog_AccountTriggerId",
                table: "MachineSerialNumberLog",
                column: "AccountTriggerId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineSerialNumberLog_MachineSerialNumberComponentId",
                table: "MachineSerialNumberLog",
                column: "MachineSerialNumberComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineSerialNumberLog_SerialNumber",
                table: "MachineSerialNumberLog",
                column: "SerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_MachineTask_ContractId",
                table: "MachineTask",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineTask_MachineCheckRequestId",
                table: "MachineTask",
                column: "MachineCheckRequestId",
                unique: true,
                filter: "[MachineCheckRequestId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MachineTask_ManagerId",
                table: "MachineTask",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineTask_StaffId",
                table: "MachineTask",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineTaskLog_AccountTriggerId",
                table: "MachineTaskLog",
                column: "AccountTriggerId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineTaskLog_MachineTaskId",
                table: "MachineTaskLog",
                column: "MachineTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineTerm_MachineId",
                table: "MachineTerm",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipRankLog_AccountId",
                table: "MembershipRankLog",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipRankLog_MembershipRankId",
                table: "MembershipRankLog",
                column: "MembershipRankId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_AccountReceiveId",
                table: "Notification",
                column: "AccountReceiveId");

            migrationBuilder.CreateIndex(
                name: "IX_RentingRequest_AccountOrderId",
                table: "RentingRequest",
                column: "AccountOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRentingRequest_RentingRequestId",
                table: "ServiceRentingRequest",
                column: "RentingRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRentingRequest_RentingServiceId",
                table: "ServiceRentingRequest",
                column: "RentingServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountBusiness");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "ComponentReplacementTicketLog");

            migrationBuilder.DropTable(
                name: "Content");

            migrationBuilder.DropTable(
                name: "ContractDelivery");

            migrationBuilder.DropTable(
                name: "ContractPayment");

            migrationBuilder.DropTable(
                name: "ContractTerm");

            migrationBuilder.DropTable(
                name: "DeliveryTaskLog");

            migrationBuilder.DropTable(
                name: "DigitalTransactions");

            migrationBuilder.DropTable(
                name: "LogDetail");

            migrationBuilder.DropTable(
                name: "MachineAttribute");

            migrationBuilder.DropTable(
                name: "MachineCheckCriteria");

            migrationBuilder.DropTable(
                name: "MachineCheckRequestCriteria");

            migrationBuilder.DropTable(
                name: "MachineImage");

            migrationBuilder.DropTable(
                name: "MachineSerialNumberLog");

            migrationBuilder.DropTable(
                name: "MachineTaskLog");

            migrationBuilder.DropTable(
                name: "MachineTerm");

            migrationBuilder.DropTable(
                name: "MembershipRankLog");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "RentingRequestAddress");

            migrationBuilder.DropTable(
                name: "ServiceRentingRequest");

            migrationBuilder.DropTable(
                name: "Term");

            migrationBuilder.DropTable(
                name: "ComponentReplacementTicket");

            migrationBuilder.DropTable(
                name: "DeliveryTask");

            migrationBuilder.DropTable(
                name: "RentingService");

            migrationBuilder.DropTable(
                name: "MachineSerialNumberComponent");

            migrationBuilder.DropTable(
                name: "MachineTask");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "MachineComponent");

            migrationBuilder.DropTable(
                name: "MachineCheckRequest");

            migrationBuilder.DropTable(
                name: "Component");

            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "MachineSerialNumber");

            migrationBuilder.DropTable(
                name: "RentingRequest");

            migrationBuilder.DropTable(
                name: "Machine");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "MembershipRank");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
