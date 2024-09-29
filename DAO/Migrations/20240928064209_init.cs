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
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Content", x => x.ContentId);
                });

            migrationBuilder.CreateTable(
                name: "ContractAddress",
                columns: table => new
                {
                    ContractAddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractAddress", x => x.ContractAddressId);
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
                name: "Promotion",
                columns: table => new
                {
                    PromotionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountPercentage = table.Column<double>(type: "float", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotion", x => x.PromotionId);
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
                    Quantity = table.Column<int>(type: "int", nullable: true),
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
                    AccountPromotionId = table.Column<int>(type: "int", nullable: true),
                    MembershipRankId = table.Column<int>(type: "int", nullable: true),
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
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "AccountPromotion",
                columns: table => new
                {
                    AccountPromotionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromotionId = table.Column<int>(type: "int", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    DateReceive = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPromotion", x => x.AccountPromotionId);
                    table.ForeignKey(
                        name: "FK_AccountPromotion_Account",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_AccountPromotion_Promotion",
                        column: x => x.PromotionId,
                        principalTable: "Promotion",
                        principalColumn: "PromotionId");
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
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
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
                name: "Log",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountLogId = table.Column<int>(type: "int", nullable: true),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_Log_AccountLogID",
                        column: x => x.AccountLogId,
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
                        principalColumn: "SerialNumber");
                });

            migrationBuilder.CreateTable(
                name: "RentingRequest",
                columns: table => new
                {
                    RentingRequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccountOrderId = table.Column<int>(type: "int", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: true),
                    ContractId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumberOfMonth = table.Column<int>(type: "int", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_RentingRequest_Address",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "AddressId");
                });

            migrationBuilder.CreateTable(
                name: "LogDetail",
                columns: table => new
                {
                    LogDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogId = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogDetail", x => x.LogDetailId);
                    table.ForeignKey(
                        name: "FK_LogDetail_LogID",
                        column: x => x.LogId,
                        principalTable: "Log",
                        principalColumn: "LogId");
                });

            migrationBuilder.CreateTable(
                name: "ProductComponentStatusLog",
                columns: table => new
                {
                    ProductComponentStatusLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductComponentStatusId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductComponentStatusLog", x => x.ProductComponentStatusLogId);
                    table.ForeignKey(
                        name: "FK_ProductComponentStatus_Log",
                        column: x => x.ProductComponentStatusId,
                        principalTable: "ProductComponentStatus",
                        principalColumn: "ProductComponentStatusId");
                });

            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContractName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountSignId = table.Column<int>(type: "int", nullable: true),
                    ContractAddressId = table.Column<int>(type: "int", nullable: false),
                    RentingRequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalRentPrice = table.Column<double>(type: "float", nullable: true),
                    ShippingPrice = table.Column<double>(type: "float", nullable: true),
                    TotalDepositPrice = table.Column<double>(type: "float", nullable: true),
                    DiscountPrice = table.Column<double>(type: "float", nullable: true),
                    FinalAmount = table.Column<double>(type: "float", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateSign = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.ContractId);
                    table.ForeignKey(
                        name: "FK_ContractAddress_Contract",
                        column: x => x.ContractAddressId,
                        principalTable: "ContractAddress",
                        principalColumn: "ContractAddressId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contract_Account",
                        column: x => x.AccountSignId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_RentingRequest_Contract",
                        column: x => x.RentingRequestId,
                        principalTable: "RentingRequest",
                        principalColumn: "RentingRequestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RentingRequestProductDetail",
                columns: table => new
                {
                    RentingRequestProductDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentingRequestId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    RentPrice = table.Column<double>(type: "float", nullable: true),
                    DepositPrice = table.Column<double>(type: "float", nullable: true)
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
                name: "ContractPayment",
                columns: table => new
                {
                    ContractPaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InvoiceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    CustomerPaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SystemPaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractPayment", x => x.ContractPaymentId);
                    table.ForeignKey(
                        name: "FK_ContractPayment_ContractID",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId");
                });

            migrationBuilder.CreateTable(
                name: "ContractSerialNumberProduct",
                columns: table => new
                {
                    ContractSerialNumberProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DepositPrice = table.Column<double>(type: "float", nullable: true),
                    DiscountPrice = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractSerialNumberProduct", x => x.ContractSerialNumberProductId);
                    table.ForeignKey(
                        name: "FK_SerialMechanicalMachinery_ContractID",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId");
                    table.ForeignKey(
                        name: "FK_SerialMechanicalMachinery_SerialNumber",
                        column: x => x.SerialNumber,
                        principalTable: "SerialNumberProduct",
                        principalColumn: "SerialNumber");
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
                name: "Delivery",
                columns: table => new
                {
                    DeliveryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffId = table.Column<int>(type: "int", nullable: true),
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DateShip = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delivery", x => x.DeliveryId);
                    table.ForeignKey(
                        name: "FK_Delivery_ContractID",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId");
                    table.ForeignKey(
                        name: "FK_Delivery_StaffID",
                        column: x => x.StaffId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTask",
                columns: table => new
                {
                    EmployeeTaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StaffId = table.Column<int>(type: "int", nullable: true),
                    ManagerId = table.Column<int>(type: "int", nullable: true),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTask", x => x.EmployeeTaskId);
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
                        name: "FK_Task_Staff",
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
                name: "MaintenanceRequest",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceRequest", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_MaintenanceRequest_Contract",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId");
                    table.ForeignKey(
                        name: "FK_MaintenanceRequest_SerialNumberProduct",
                        column: x => x.SerialNumber,
                        principalTable: "SerialNumberProduct",
                        principalColumn: "SerialNumber");
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InvoiceCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountPaidId = table.Column<int>(type: "int", nullable: true),
                    ContractPaymentId = table.Column<int>(type: "int", nullable: true),
                    MaintainTicketId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_Invoice_ContractPayment",
                        column: x => x.ContractPaymentId,
                        principalTable: "ContractPayment",
                        principalColumn: "ContractPaymentId");
                    table.ForeignKey(
                        name: "FK_Invoices_Account",
                        column: x => x.AccountPaidId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeTaskId = table.Column<int>(type: "int", nullable: true),
                    ReportContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Report_TaskID",
                        column: x => x.EmployeeTaskId,
                        principalTable: "EmployeeTask",
                        principalColumn: "EmployeeTaskId");
                });

            migrationBuilder.CreateTable(
                name: "TaskLog",
                columns: table => new
                {
                    TaskLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeTaskId = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskLog", x => x.TaskLogId);
                    table.ForeignKey(
                        name: "FK_TaskLog_TaskID",
                        column: x => x.EmployeeTaskId,
                        principalTable: "EmployeeTask",
                        principalColumn: "EmployeeTaskId");
                });

            migrationBuilder.CreateTable(
                name: "RequestDateResponse",
                columns: table => new
                {
                    ResponseDateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: true),
                    DateResponse = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestDateResponse", x => x.ResponseDateId);
                    table.ForeignKey(
                        name: "FK_RequestDateResponse_MaintenanceRequest",
                        column: x => x.RequestId,
                        principalTable: "MaintenanceRequest",
                        principalColumn: "RequestId");
                });

            migrationBuilder.CreateTable(
                name: "MaintainingTicket",
                columns: table => new
                {
                    MaintainingTicketId = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_Account_MembershipRankId",
                table: "Account",
                column: "MembershipRankId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountBusiness_AccountId",
                table: "AccountBusiness",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountPromotion_AccountId",
                table: "AccountPromotion",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountPromotion_PromotionId",
                table: "AccountPromotion",
                column: "PromotionId");

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
                name: "IX_Contract_AccountSignId",
                table: "Contract",
                column: "AccountSignId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_ContractAddressId",
                table: "Contract",
                column: "ContractAddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contract_RentingRequestId",
                table: "Contract",
                column: "RentingRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractPayment_ContractId",
                table: "ContractPayment",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSerialNumberProduct_ContractId",
                table: "ContractSerialNumberProduct",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSerialNumberProduct_SerialNumber",
                table: "ContractSerialNumberProduct",
                column: "SerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ContractTerm_ContractId",
                table: "ContractTerm",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_ContractId",
                table: "Delivery",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_StaffId",
                table: "Delivery",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTask_ContractId",
                table: "EmployeeTask",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTask_ManagerId",
                table: "EmployeeTask",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTask_StaffId",
                table: "EmployeeTask",
                column: "StaffId");

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
                name: "IX_Invoices_ContractPaymentId",
                table: "Invoices",
                column: "ContractPaymentId",
                unique: true,
                filter: "[ContractPaymentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Log_AccountLogId",
                table: "Log",
                column: "AccountLogId");

            migrationBuilder.CreateIndex(
                name: "IX_LogDetail_LogId",
                table: "LogDetail",
                column: "LogId");

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

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequest_ContractId",
                table: "MaintenanceRequest",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequest_SerialNumber",
                table: "MaintenanceRequest",
                column: "SerialNumber");

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
                name: "IX_ProductComponentStatusLog_ProductComponentStatusId",
                table: "ProductComponentStatusLog",
                column: "ProductComponentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ProductId",
                table: "ProductImage",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RentingRequest_AccountOrderId",
                table: "RentingRequest",
                column: "AccountOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RentingRequest_AddressId",
                table: "RentingRequest",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_RentingRequestProductDetail_ProductId",
                table: "RentingRequestProductDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RentingRequestProductDetail_RentingRequestId",
                table: "RentingRequestProductDetail",
                column: "RentingRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_EmployeeTaskId",
                table: "Report",
                column: "EmployeeTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestDateResponse_RequestId",
                table: "RequestDateResponse",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_SerialNumberProduct_ProductId",
                table: "SerialNumberProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskLog_EmployeeTaskId",
                table: "TaskLog",
                column: "EmployeeTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountBusiness");

            migrationBuilder.DropTable(
                name: "AccountPromotion");

            migrationBuilder.DropTable(
                name: "Content");

            migrationBuilder.DropTable(
                name: "ContractSerialNumberProduct");

            migrationBuilder.DropTable(
                name: "ContractTerm");

            migrationBuilder.DropTable(
                name: "Delivery");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "LogDetail");

            migrationBuilder.DropTable(
                name: "MaintainingTicket");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "ProductAttribute");

            migrationBuilder.DropTable(
                name: "ProductComponentStatusLog");

            migrationBuilder.DropTable(
                name: "ProductImage");

            migrationBuilder.DropTable(
                name: "RentingRequestProductDetail");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "RequestDateResponse");

            migrationBuilder.DropTable(
                name: "TaskLog");

            migrationBuilder.DropTable(
                name: "Promotion");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "ProductComponentStatus");

            migrationBuilder.DropTable(
                name: "MaintenanceRequest");

            migrationBuilder.DropTable(
                name: "EmployeeTask");

            migrationBuilder.DropTable(
                name: "ContractPayment");

            migrationBuilder.DropTable(
                name: "ComponentProduct");

            migrationBuilder.DropTable(
                name: "SerialNumberProduct");

            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "Component");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "ContractAddress");

            migrationBuilder.DropTable(
                name: "RentingRequest");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "MembershipRank");
        }
    }
}
