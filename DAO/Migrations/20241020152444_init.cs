using System;
using Microsoft.EntityFrameworkCore.Migrations;

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
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    ComponentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Component", x => x.ComponentId);
                });

            migrationBuilder.CreateTable(
                name: "MembershipRank",
                columns: table => new
                {
                    MembershipRankId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MembershipRankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoneySpent = table.Column<double>(type: "float", nullable: true),
                    DiscountPercentage = table.Column<double>(type: "float", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    RentingServiceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    IsOptional = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentingService", x => x.RentingServiceId);
                });

            migrationBuilder.CreateTable(
                name: "Term",
                columns: table => new
                {
                    TermId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Term", x => x.TermId);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RentPrice = table.Column<double>(type: "float", nullable: true),
                    ProductPrice = table.Column<double>(type: "float", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Product_Category",
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
                    AvatarImg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    OtpNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                });

            migrationBuilder.CreateTable(
                name: "ComponentProduct",
                columns: table => new
                {
                    ComponentProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    ComponentId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRequiredMoney = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentProduct", x => x.ComponentProductId);
                    table.ForeignKey(
                        name: "FK_ComponentProduct_ComponentID",
                        column: x => x.ComponentId,
                        principalTable: "Component",
                        principalColumn: "ComponentId");
                    table.ForeignKey(
                        name: "FK_ComponentProduct_ProductID",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "ProductAttribute",
                columns: table => new
                {
                    ProductAttributeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    AttributeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Specifications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttribute", x => x.ProductAttributeId);
                    table.ForeignKey(
                        name: "FK_Attribute_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "ProductImage",
                columns: table => new
                {
                    ProductImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    ProductImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsThumbnail = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImage", x => x.ProductImageId);
                    table.ForeignKey(
                        name: "FK_ProductImage_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTerm",
                columns: table => new
                {
                    ProductTermId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTerm", x => x.ProductTermId);
                    table.ForeignKey(
                        name: "FK_Term_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "SerialNumberProduct",
                columns: table => new
                {
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    ActualRentPrice = table.Column<double>(type: "float", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RentTimeCounter = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerialNumberProduct", x => x.SerialNumber);
                    table.ForeignKey(
                        name: "FK_ProductNumber_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "AccountBusiness",
                columns: table => new
                {
                    AccountBusinessId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    AddressBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true),
                    Coordinates = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "Invoices",
                columns: table => new
                {
                    InvoiceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccountPaidId = table.Column<int>(type: "int", nullable: true),
                    MaintainTicketId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DigitalTransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DatePaid = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayOsOrderId = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    NotificationType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotificationTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageNotification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkForward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateRead = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    RentingRequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccountOrderId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalRentPrice = table.Column<double>(type: "float", nullable: true),
                    TotalDepositPrice = table.Column<double>(type: "float", nullable: true),
                    TotalServicePrice = table.Column<double>(type: "float", nullable: true),
                    ShippingPrice = table.Column<double>(type: "float", nullable: true),
                    DiscountPrice = table.Column<double>(type: "float", nullable: true),
                    NumberOfMonth = table.Column<int>(type: "int", nullable: true),
                    TotalAmount = table.Column<double>(type: "float", nullable: true),
                    IsOnetimePayment = table.Column<bool>(type: "bit", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "ProductComponentStatus",
                columns: table => new
                {
                    ProductComponentStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ComponentId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductComponentStatus", x => x.ProductComponentStatusId);
                    table.ForeignKey(
                        name: "FK_ProductComponentStatus_ComponentID",
                        column: x => x.ComponentId,
                        principalTable: "ComponentProduct",
                        principalColumn: "ComponentProductId");
                    table.ForeignKey(
                        name: "FK_ProductComponentStatus_SerialNumberProduct",
                        column: x => x.SerialNumber,
                        principalTable: "SerialNumberProduct",
                        principalColumn: "SerialNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SerialNumberProductLog",
                columns: table => new
                {
                    SerialNumberProductLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AccountTriggerId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerialNumberProductLog", x => x.SerialNumberProductLogId);
                    table.ForeignKey(
                        name: "FK_SerialNumberProductLog_AccountID",
                        column: x => x.AccountTriggerId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_SerialNumberProduct_Log",
                        column: x => x.SerialNumber,
                        principalTable: "SerialNumberProduct",
                        principalColumn: "SerialNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DigitalTransactions",
                columns: table => new
                {
                    DigitalTransactionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InvoiceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PayOsOrderId = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContractName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountSignId = table.Column<int>(type: "int", nullable: true),
                    RentingRequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RentPrice = table.Column<double>(type: "float", nullable: true),
                    DepositPrice = table.Column<double>(type: "float", nullable: true),
                    NumberOfMonth = table.Column<int>(type: "int", nullable: true),
                    TotalRentPrice = table.Column<double>(type: "float", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateSign = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                        name: "FK_Contract_SerialNumberProduct",
                        column: x => x.SerialNumber,
                        principalTable: "SerialNumberProduct",
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

            migrationBuilder.CreateTable(
                name: "RentingRequestProductDetail",
                columns: table => new
                {
                    RentingRequestProductDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentingRequestId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentingRequestProductDetail", x => x.RentingRequestProductDetailId);
                    table.ForeignKey(
                        name: "FK_RentingRequestProductDetail_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId");
                    table.ForeignKey(
                        name: "FK_RentingRequestProductDetail_RentingRequest",
                        column: x => x.RentingRequestId,
                        principalTable: "RentingRequest",
                        principalColumn: "RentingRequestId");
                });

            migrationBuilder.CreateTable(
                name: "ServiceRentingRequest",
                columns: table => new
                {
                    ServiceRentingRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentingServiceId = table.Column<int>(type: "int", nullable: true),
                    RentingRequestId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                name: "ContractPayment",
                columns: table => new
                {
                    ContractPaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InvoiceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: true),
                    CustomerPaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SystemPaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                name: "DeliveryTask",
                columns: table => new
                {
                    DeliveryTaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffId = table.Column<int>(type: "int", nullable: true),
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DateShip = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCompleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryTask", x => x.DeliveryTaskId);
                    table.ForeignKey(
                        name: "FK_DeliveryTask_ContractID",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId");
                    table.ForeignKey(
                        name: "FK_DeliveryTask_StaffID",
                        column: x => x.StaffId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    FeedbackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountFeedbackId = table.Column<int>(type: "int", nullable: true),
                    FeedbackImg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.FeedbackId);
                    table.ForeignKey(
                        name: "FK_Feedback_Account",
                        column: x => x.AccountFeedbackId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_Feedback_Contract",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId");
                });

            migrationBuilder.CreateTable(
                name: "MachineCheckRequest",
                columns: table => new
                {
                    MachineCheckRequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "DeliveryTaskLog",
                columns: table => new
                {
                    DeliveryTaskLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryTaskId = table.Column<int>(type: "int", nullable: true),
                    AccountTriggerId = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "RequestResponse",
                columns: table => new
                {
                    RequestResponseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineCheckRequestId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MachineTaskId = table.Column<int>(type: "int", nullable: true),
                    DateResponse = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestResponse", x => x.RequestResponseId);
                    table.ForeignKey(
                        name: "FK_RequestResponse_MachineCheckRequest",
                        column: x => x.MachineCheckRequestId,
                        principalTable: "MachineCheckRequest",
                        principalColumn: "MachineCheckRequestId");
                });

            migrationBuilder.CreateTable(
                name: "ComponentReplacementTicket",
                columns: table => new
                {
                    ComponentReplacementTicketId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployeeCreateId = table.Column<int>(type: "int", nullable: true),
                    MachineTaskCreateId = table.Column<int>(type: "int", nullable: true),
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                        name: "FK_ComponentReplacementTicket_SerialNumberProduct",
                        column: x => x.ProductSerialNumber,
                        principalTable: "SerialNumberProduct",
                        principalColumn: "SerialNumber");
                    table.ForeignKey(
                        name: "FK_Invoice_MaintainTicket",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId");
                });

            migrationBuilder.CreateTable(
                name: "ComponentReplacementTicketLog",
                columns: table => new
                {
                    ComponentReplacementTicketLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComponentReplacementTicketId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AccountTriggerId = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "MachineTask",
                columns: table => new
                {
                    MachineTaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestResponseId = table.Column<int>(type: "int", nullable: false),
                    ComponentReplacementTicketId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PreviousTaskId = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StaffId = table.Column<int>(type: "int", nullable: true),
                    ManagerId = table.Column<int>(type: "int", nullable: true),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCompleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineTask", x => x.MachineTaskId);
                    table.ForeignKey(
                        name: "FK_ComponentReplacementTicketId_Task",
                        column: x => x.ComponentReplacementTicketId,
                        principalTable: "ComponentReplacementTicket",
                        principalColumn: "ComponentReplacementTicketId");
                    table.ForeignKey(
                        name: "FK_Task_Contract",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId");
                    table.ForeignKey(
                        name: "FK_Task_Manager",
                        column: x => x.ManagerId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_Task_PreviousTask",
                        column: x => x.PreviousTaskId,
                        principalTable: "MachineTask",
                        principalColumn: "MachineTaskId");
                    table.ForeignKey(
                        name: "FK_Task_Response",
                        column: x => x.RequestResponseId,
                        principalTable: "RequestResponse",
                        principalColumn: "RequestResponseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Task_Staff",
                        column: x => x.StaffId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
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
                name: "Report",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineTaskId = table.Column<int>(type: "int", nullable: true),
                    ReportContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Report_TaskID",
                        column: x => x.MachineTaskId,
                        principalTable: "MachineTask",
                        principalColumn: "MachineTaskId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_MembershipRankId",
                table: "Account",
                column: "MembershipRankId");

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
                name: "IX_ComponentProduct_ComponentId",
                table: "ComponentProduct",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentProduct_ProductId",
                table: "ComponentProduct",
                column: "ProductId");

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
                name: "IX_ComponentReplacementTicket_MachineTaskCreateId",
                table: "ComponentReplacementTicket",
                column: "MachineTaskCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentReplacementTicket_ProductSerialNumber",
                table: "ComponentReplacementTicket",
                column: "ProductSerialNumber");

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
                name: "IX_Contract_RentingRequestId",
                table: "Contract",
                column: "RentingRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_SerialNumber",
                table: "Contract",
                column: "SerialNumber");

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
                name: "IX_DeliveryTask_ContractId",
                table: "DeliveryTask",
                column: "ContractId");

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
                name: "IX_Feedback_AccountFeedbackId",
                table: "Feedback",
                column: "AccountFeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_ContractId",
                table: "Feedback",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_AccountPaidId",
                table: "Invoices",
                column: "AccountPaidId");

            migrationBuilder.CreateIndex(
                name: "IX_LogDetail_AccountId",
                table: "LogDetail",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineCheckRequest_ContractId",
                table: "MachineCheckRequest",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineTask_ComponentReplacementTicketId",
                table: "MachineTask",
                column: "ComponentReplacementTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineTask_ContractId",
                table: "MachineTask",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineTask_ManagerId",
                table: "MachineTask",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineTask_PreviousTaskId",
                table: "MachineTask",
                column: "PreviousTaskId",
                unique: true,
                filter: "[PreviousTaskId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MachineTask_RequestResponseId",
                table: "MachineTask",
                column: "RequestResponseId",
                unique: true);

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
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttribute_ProductId",
                table: "ProductAttribute",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductComponentStatus_ComponentId",
                table: "ProductComponentStatus",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductComponentStatus_SerialNumber",
                table: "ProductComponentStatus",
                column: "SerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ProductId",
                table: "ProductImage",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTerm_ProductId",
                table: "ProductTerm",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RentingRequest_AccountOrderId",
                table: "RentingRequest",
                column: "AccountOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RentingRequestProductDetail_ProductId",
                table: "RentingRequestProductDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RentingRequestProductDetail_RentingRequestId",
                table: "RentingRequestProductDetail",
                column: "RentingRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_MachineTaskId",
                table: "Report",
                column: "MachineTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestResponse_MachineCheckRequestId",
                table: "RequestResponse",
                column: "MachineCheckRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_SerialNumberProduct_ProductId",
                table: "SerialNumberProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SerialNumberProductLog_AccountTriggerId",
                table: "SerialNumberProductLog",
                column: "AccountTriggerId");

            migrationBuilder.CreateIndex(
                name: "IX_SerialNumberProductLog_SerialNumber",
                table: "SerialNumberProductLog",
                column: "SerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRentingRequest_RentingRequestId",
                table: "ServiceRentingRequest",
                column: "RentingRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRentingRequest_RentingServiceId",
                table: "ServiceRentingRequest",
                column: "RentingServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentReplacementTicket_MachineTaskCreated",
                table: "ComponentReplacementTicket",
                column: "MachineTaskCreateId",
                principalTable: "MachineTask",
                principalColumn: "MachineTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_MembershipRank",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_ComponentReplacementTicket_Account_EmployeeCreateId",
                table: "ComponentReplacementTicket");

            migrationBuilder.DropForeignKey(
                name: "FK_Contract_Account",
                table: "Contract");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Account",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Manager",
                table: "MachineTask");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Staff",
                table: "MachineTask");

            migrationBuilder.DropForeignKey(
                name: "FK_RentingRequest_Account",
                table: "RentingRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_ComponentReplacementTicket_ComponentID",
                table: "ComponentReplacementTicket");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductNumber_Product",
                table: "SerialNumberProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_ComponentReplacementTicket_ContractID",
                table: "ComponentReplacementTicket");

            migrationBuilder.DropForeignKey(
                name: "FK_MachineCheckRequest_Contract",
                table: "MachineCheckRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Contract",
                table: "MachineTask");

            migrationBuilder.DropForeignKey(
                name: "FK_ComponentReplacementTicket_MachineTaskCreated",
                table: "ComponentReplacementTicket");

            migrationBuilder.DropTable(
                name: "AccountBusiness");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "ComponentReplacementTicketLog");

            migrationBuilder.DropTable(
                name: "Content");

            migrationBuilder.DropTable(
                name: "ContractPayment");

            migrationBuilder.DropTable(
                name: "ContractTerm");

            migrationBuilder.DropTable(
                name: "DeliveryTaskLog");

            migrationBuilder.DropTable(
                name: "DigitalTransactions");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "LogDetail");

            migrationBuilder.DropTable(
                name: "MachineTaskLog");

            migrationBuilder.DropTable(
                name: "MembershipRankLog");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "ProductAttribute");

            migrationBuilder.DropTable(
                name: "ProductComponentStatus");

            migrationBuilder.DropTable(
                name: "ProductImage");

            migrationBuilder.DropTable(
                name: "ProductTerm");

            migrationBuilder.DropTable(
                name: "RentingRequestAddress");

            migrationBuilder.DropTable(
                name: "RentingRequestProductDetail");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "SerialNumberProductLog");

            migrationBuilder.DropTable(
                name: "ServiceRentingRequest");

            migrationBuilder.DropTable(
                name: "Term");

            migrationBuilder.DropTable(
                name: "DeliveryTask");

            migrationBuilder.DropTable(
                name: "ComponentProduct");

            migrationBuilder.DropTable(
                name: "RentingService");

            migrationBuilder.DropTable(
                name: "MembershipRank");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Component");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "RentingRequest");

            migrationBuilder.DropTable(
                name: "MachineTask");

            migrationBuilder.DropTable(
                name: "ComponentReplacementTicket");

            migrationBuilder.DropTable(
                name: "RequestResponse");

            migrationBuilder.DropTable(
                name: "SerialNumberProduct");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "MachineCheckRequest");
        }
    }
}
