﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    Type = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "ComponentProduct",
                columns: table => new
                {
                    ComponentID = table.Column<int>(type: "int", nullable: false),
                    ComponentName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentProduct", x => x.ComponentID);
                });

            migrationBuilder.CreateTable(
                name: "DiscountType",
                columns: table => new
                {
                    DiscountTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountTypeName = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountType", x => x.DiscountTypeId);
                });

            migrationBuilder.CreateTable(
                name: "PromotionType",
                columns: table => new
                {
                    PromotionTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromotionTypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionType", x => x.PromotionTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Component",
                columns: table => new
                {
                    ComponentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComponentName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Component", x => x.ComponentId);
                    table.ForeignKey(
                        name: "FK_Component_CategoryID",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ProductImg = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Origin = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
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
                name: "Promotion",
                columns: table => new
                {
                    PromotionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromotionPack = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ActionPromotion = table.Column<int>(type: "int", nullable: true),
                    DiscountPercentage = table.Column<double>(type: "float", nullable: true),
                    PromotionTypeId = table.Column<int>(type: "int", nullable: true),
                    DiscountTypeId = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotion", x => x.PromotionId);
                    table.ForeignKey(
                        name: "FK_Promotion_DiscountTypeID",
                        column: x => x.DiscountTypeId,
                        principalTable: "DiscountType",
                        principalColumn: "DiscountTypeId");
                    table.ForeignKey(
                        name: "FK_Promotion_PromotionTypeID",
                        column: x => x.PromotionTypeId,
                        principalTable: "PromotionType",
                        principalColumn: "PromotionTypeId");
                });

            migrationBuilder.CreateTable(
                name: "Attribute",
                columns: table => new
                {
                    ProductAttributeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    AttributeName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Specifications = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attribute", x => x.ProductAttributeId);
                    table.ForeignKey(
                        name: "FK_Attribute_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "ProductDetail",
                columns: table => new
                {
                    ProductDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    ComponentId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetail", x => x.ProductDetailId);
                    table.ForeignKey(
                        name: "FK_ProductDetail_Component",
                        column: x => x.ComponentId,
                        principalTable: "ComponentProduct",
                        principalColumn: "ComponentID");
                    table.ForeignKey(
                        name: "FK_ProductDetail_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "ProductNumber",
                columns: table => new
                {
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductNumber", x => x.SerialNumber);
                    table.ForeignKey(
                        name: "FK_ProductNumber_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvatarImg = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CitizenCard = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DateBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(100)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(100)", nullable: true),
                    OtpNumber = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    TokenRefresh = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    TokenDateExpire = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActionPromotion = table.Column<int>(type: "int", nullable: true),
                    PromotionId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateExpire = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    BusinessType = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Account_Promotion",
                        column: x => x.PromotionId,
                        principalTable: "Promotion",
                        principalColumn: "PromotionId");
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
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductComponentStatus", x => x.ProductComponentStatusId);
                    table.ForeignKey(
                        name: "FK_ProductComponentStatus_ComponentID",
                        column: x => x.ComponentId,
                        principalTable: "ComponentProduct",
                        principalColumn: "ComponentID");
                    table.ForeignKey(
                        name: "FK_ProductComponentStatus_SerialNumber",
                        column: x => x.SerialNumber,
                        principalTable: "ProductNumber",
                        principalColumn: "SerialNumber");
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
                    Position = table.Column<string>(type: "nvarchar(100)", nullable: true)
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
                    PromotionAccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromotionId = table.Column<int>(type: "int", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    DateReceive = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPromotion", x => x.PromotionAccountId);
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
                    Address1 = table.Column<string>(type: "nvarchar(100)", nullable: true),
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
                name: "Content",
                columns: table => new
                {
                    ContentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Content1 = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    AccountCreateId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Content", x => x.ContentId);
                    table.ForeignKey(
                        name: "FK_Content_Account",
                        column: x => x.AccountCreateId,
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
                    NotificationType = table.Column<int>(type: "int", nullable: true),
                    MessageNotification = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    LinkForward = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", nullable: true)
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
                name: "Contract",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    AccountSignId = table.Column<int>(type: "int", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    ShippingPrice = table.Column<double>(type: "float", nullable: true),
                    DiscountPrice = table.Column<double>(type: "float", nullable: true),
                    FinalPrice = table.Column<double>(type: "float", nullable: true),
                    Method = table.Column<int>(type: "int", nullable: true),
                    DateSign = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", nullable: true)
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
                        name: "FK_Contract_Address",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "AddressId");
                });

            migrationBuilder.CreateTable(
                name: "HiringRequest",
                columns: table => new
                {
                    HiringRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountOrderId = table.Column<int>(type: "int", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HiringRequest", x => x.HiringRequestId);
                    table.ForeignKey(
                        name: "FK_HiringRequest_Account",
                        column: x => x.AccountOrderId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_HiringRequest_Address",
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
                    Action = table.Column<string>(type: "nvarchar(100)", nullable: true),
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
                name: "ContractPayment",
                columns: table => new
                {
                    ContractPaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    CustomerPaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SystemPaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", nullable: true),
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
                name: "ContractTerm",
                columns: table => new
                {
                    ContractTermId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(100)", nullable: true),
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
                    ContractId = table.Column<int>(type: "int", nullable: true),
                    DateShip = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", nullable: true)
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
                name: "Feedback",
                columns: table => new
                {
                    FeedbackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountFeedbackId = table.Column<int>(type: "int", nullable: true),
                    FeedbackImg = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ContractId = table.Column<int>(type: "int", nullable: true),
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
                name: "Invoices",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceCode = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ContractId = table.Column<int>(type: "int", nullable: true),
                    Method = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_Invoices_Contract",
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
                    ContractId = table.Column<int>(type: "int", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", nullable: true),
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
                        name: "FK_MaintenanceRequest_ProductNumber",
                        column: x => x.SerialNumber,
                        principalTable: "ProductNumber",
                        principalColumn: "SerialNumber");
                });

            migrationBuilder.CreateTable(
                name: "SerialMechanicalMachinery",
                columns: table => new
                {
                    ContractOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<int>(type: "int", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerialMechanicalMachinery", x => x.ContractOrderId);
                    table.ForeignKey(
                        name: "FK_SerialMechanicalMachinery_ContractID",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId");
                    table.ForeignKey(
                        name: "FK_SerialMechanicalMachinery_SerialNumber",
                        column: x => x.SerialNumber,
                        principalTable: "ProductNumber",
                        principalColumn: "SerialNumber");
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    EmployeeTaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskTitle = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ContractId = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    AssigneeId = table.Column<int>(type: "int", nullable: true),
                    ReporterId = table.Column<int>(type: "int", nullable: true),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    TaskType = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.EmployeeTaskId);
                    table.ForeignKey(
                        name: "FK_Task_Assignee",
                        column: x => x.AssigneeId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_Task_Contract",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId");
                    table.ForeignKey(
                        name: "FK_Task_Reporter",
                        column: x => x.ReporterId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "HiringRequestProductDetail",
                columns: table => new
                {
                    HiringRequestProductDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HiringRequestId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HiringRequestProductDetail", x => x.HiringRequestProductDetailId);
                    table.ForeignKey(
                        name: "FK_HiringRequestProductDetail_HiringRequest",
                        column: x => x.HiringRequestId,
                        principalTable: "HiringRequest",
                        principalColumn: "HiringRequestId");
                    table.ForeignKey(
                        name: "FK_HiringRequestProductDetail_Product",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId");
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
                    Status = table.Column<string>(type: "nvarchar(100)", nullable: true)
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
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    EmployeeTaskId = table.Column<int>(type: "int", nullable: true),
                    ComponentId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    DateRepair = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Maintain__76F8D53F2FA1A432", x => x.MaintainingTicketId);
                    table.ForeignKey(
                        name: "FK_MaintainingTicket_ComponentID",
                        column: x => x.ComponentId,
                        principalTable: "Component",
                        principalColumn: "ComponentId");
                    table.ForeignKey(
                        name: "FK_MaintainingTicket_TaskID",
                        column: x => x.EmployeeTaskId,
                        principalTable: "Task",
                        principalColumn: "EmployeeTaskId");
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeTaskId = table.Column<int>(type: "int", nullable: true),
                    ReportContent = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Report_TaskID",
                        column: x => x.EmployeeTaskId,
                        principalTable: "Task",
                        principalColumn: "EmployeeTaskId");
                });

            migrationBuilder.CreateTable(
                name: "TaskLog",
                columns: table => new
                {
                    TaskLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeTaskId = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskLog", x => x.TaskLogId);
                    table.ForeignKey(
                        name: "FK_TaskLog_TaskID",
                        column: x => x.EmployeeTaskId,
                        principalTable: "Task",
                        principalColumn: "EmployeeTaskId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_PromotionId",
                table: "Account",
                column: "PromotionId");

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
                name: "IX_Attribute_ProductId",
                table: "Attribute",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Component_CategoryId",
                table: "Component",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Content_AccountCreateId",
                table: "Content",
                column: "AccountCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_AccountSignId",
                table: "Contract",
                column: "AccountSignId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_AddressId",
                table: "Contract",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractPayment_ContractId",
                table: "ContractPayment",
                column: "ContractId");

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
                name: "IX_Feedback_AccountFeedbackId",
                table: "Feedback",
                column: "AccountFeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_ContractId",
                table: "Feedback",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_HiringRequest_AccountOrderId",
                table: "HiringRequest",
                column: "AccountOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_HiringRequest_AddressId",
                table: "HiringRequest",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_HiringRequestProductDetail_HiringRequestId",
                table: "HiringRequestProductDetail",
                column: "HiringRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_HiringRequestProductDetail_ProductId",
                table: "HiringRequestProductDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ContractId",
                table: "Invoices",
                column: "ContractId");

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
                name: "IX_MaintainingTicket_EmployeeTaskId",
                table: "MaintainingTicket",
                column: "EmployeeTaskId");

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
                name: "IX_ProductComponentStatus_ComponentId",
                table: "ProductComponentStatus",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductComponentStatus_SerialNumber",
                table: "ProductComponentStatus",
                column: "SerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetail_ComponentId",
                table: "ProductDetail",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetail_ProductId",
                table: "ProductDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductNumber_ProductId",
                table: "ProductNumber",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_DiscountTypeId",
                table: "Promotion",
                column: "DiscountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_PromotionTypeId",
                table: "Promotion",
                column: "PromotionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_EmployeeTaskId",
                table: "Report",
                column: "EmployeeTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestDateResponse_RequestId",
                table: "RequestDateResponse",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_SerialMechanicalMachinery_ContractId",
                table: "SerialMechanicalMachinery",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_SerialMechanicalMachinery_SerialNumber",
                table: "SerialMechanicalMachinery",
                column: "SerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Task_AssigneeId",
                table: "Task",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_ContractId",
                table: "Task",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_ReporterId",
                table: "Task",
                column: "ReporterId");

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
                name: "Attribute");

            migrationBuilder.DropTable(
                name: "Content");

            migrationBuilder.DropTable(
                name: "ContractPayment");

            migrationBuilder.DropTable(
                name: "ContractTerm");

            migrationBuilder.DropTable(
                name: "Delivery");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "HiringRequestProductDetail");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "LogDetail");

            migrationBuilder.DropTable(
                name: "MaintainingTicket");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "ProductComponentStatus");

            migrationBuilder.DropTable(
                name: "ProductDetail");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "RequestDateResponse");

            migrationBuilder.DropTable(
                name: "SerialMechanicalMachinery");

            migrationBuilder.DropTable(
                name: "TaskLog");

            migrationBuilder.DropTable(
                name: "HiringRequest");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "Component");

            migrationBuilder.DropTable(
                name: "ComponentProduct");

            migrationBuilder.DropTable(
                name: "MaintenanceRequest");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "ProductNumber");

            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Promotion");

            migrationBuilder.DropTable(
                name: "DiscountType");

            migrationBuilder.DropTable(
                name: "PromotionType");
        }
    }
}
