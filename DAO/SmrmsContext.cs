using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAO;

public partial class SmrmsContext : DbContext
{
    public SmrmsContext()
    {
    }

    public SmrmsContext(DbContextOptions<SmrmsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountBusiness> AccountBusinesses { get; set; }

    public virtual DbSet<AccountPromotion> AccountPromotions { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<BusinessObject.Attribute> Attributes { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Component> Components { get; set; }

    public virtual DbSet<ComponentProduct> ComponentProducts { get; set; }

    public virtual DbSet<Content> Contents { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<ContractPayment> ContractPayments { get; set; }

    public virtual DbSet<ContractTerm> ContractTerms { get; set; }

    public virtual DbSet<Delivery> Deliveries { get; set; }

    public virtual DbSet<DiscountType> DiscountTypes { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<HiringRequest> HiringRequests { get; set; }

    public virtual DbSet<HiringRequestProductDetail> HiringRequestProductDetails { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<LogDetail> LogDetails { get; set; }

    public virtual DbSet<MaintainingTicket> MaintainingTickets { get; set; }

    public virtual DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductComponentStatus> ProductComponentStatuses { get; set; }

    public virtual DbSet<ProductDetail> ProductDetails { get; set; }

    public virtual DbSet<ProductNumber> ProductNumbers { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<PromotionType> PromotionTypes { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<RequestDateResponse> RequestDateResponses { get; set; }

    public virtual DbSet<SerialMechanicalMachinery> SerialMechanicalMachineries { get; set; }

    public virtual DbSet<BusinessObject.Task> Tasks { get; set; }

    public virtual DbSet<TaskLog> TaskLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(GetConnectionString());
        }
    }

    private static string GetConnectionString()
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();

        var strConn = config["ConnectionStrings:SqlCloud"];

        return strConn;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA586CC701C2D");

            entity.ToTable("Account");

            entity.Property(e => e.AccountId)
                .ValueGeneratedNever()
                .HasColumnName("AccountID");
            entity.Property(e => e.CitizenCard).HasMaxLength(50);
            entity.Property(e => e.DateBirth).HasColumnType("datetime");
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.DateExpire).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Otpnumber)
                .HasMaxLength(20)
                .HasColumnName("OTPNumber");
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.PromotionId).HasColumnName("PromotionID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.TokenDateExpire).HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Promotion).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK_Account_Promotion");
        });

        modelBuilder.Entity<AccountBusiness>(entity =>
        {
            entity.HasKey(e => e.AccountBusinessId).HasName("PK__AccountB__293C04D827DBAFDD");

            entity.ToTable("AccountBusiness");

            entity.Property(e => e.AccountBusinessId)
                .ValueGeneratedNever()
                .HasColumnName("AccountBusinessID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Position).HasMaxLength(100);

            entity.HasOne(d => d.Account).WithMany(p => p.AccountBusinesses)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_AccountBusiness_Account");
        });

        modelBuilder.Entity<AccountPromotion>(entity =>
        {
            entity.HasKey(e => e.PromotionAccountId).HasName("PK__AccountP__1828094A4D794EBF");

            entity.ToTable("AccountPromotion");

            entity.Property(e => e.PromotionAccountId)
                .ValueGeneratedNever()
                .HasColumnName("PromotionAccountID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.DateReceive).HasColumnType("datetime");
            entity.Property(e => e.PromotionId).HasColumnName("PromotionID");

            entity.HasOne(d => d.Account).WithMany(p => p.AccountPromotions)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_AccountPromotion_Account");

            entity.HasOne(d => d.Promotion).WithMany(p => p.AccountPromotions)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK_AccountPromotion_Promotion");
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Address__091C2A1B0D8C61F2");

            entity.ToTable("Address");

            entity.Property(e => e.AddressId)
                .ValueGeneratedNever()
                .HasColumnName("AddressID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Address1).HasColumnName("Address");

            entity.HasOne(d => d.Account).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Address_Account");
        });

        modelBuilder.Entity<Attribute>(entity =>
        {
            entity.HasKey(e => e.AttributeId).HasName("PK__Attribut__C189298AD73AE2A1");

            entity.ToTable("Attribute");

            entity.Property(e => e.AttributeId)
                .ValueGeneratedNever()
                .HasColumnName("AttributeID");
            entity.Property(e => e.AttributeName).HasMaxLength(100);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany(p => p.Attributes)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Attribute_Product");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A2BA1876522");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedNever()
                .HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => e.ComponentId).HasName("PK__Componen__D79CF02ECAE98BCF");

            entity.ToTable("Component");

            entity.Property(e => e.ComponentId)
                .ValueGeneratedNever()
                .HasColumnName("ComponentID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ComponentName).HasMaxLength(100);
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Category).WithMany(p => p.Components)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Component_CategoryID");
        });

        modelBuilder.Entity<ComponentProduct>(entity =>
        {
            entity.HasKey(e => e.ComponentId).HasName("PK__Componen__D79CF02EA8404F11");

            entity.ToTable("ComponentProduct");

            entity.Property(e => e.ComponentId)
                .ValueGeneratedNever()
                .HasColumnName("ComponentID");
            entity.Property(e => e.ComponentName).HasMaxLength(100);
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<Content>(entity =>
        {
            entity.HasKey(e => e.ContentId).HasName("PK__Content__2907A87ECB0F8C31");

            entity.ToTable("Content");

            entity.Property(e => e.ContentId)
                .ValueGeneratedNever()
                .HasColumnName("ContentID");
            entity.Property(e => e.AccountCreateId).HasColumnName("AccountCreateID");
            entity.Property(e => e.Content1).HasColumnName("Content");
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.AccountCreate).WithMany(p => p.Contents)
                .HasForeignKey(d => d.AccountCreateId)
                .HasConstraintName("FK_Content_Account");
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.ContractId).HasName("PK__Contract__C90D340958851950");

            entity.ToTable("Contract");

            entity.Property(e => e.ContractId)
                .ValueGeneratedNever()
                .HasColumnName("ContractID");
            entity.Property(e => e.AccountSignId).HasColumnName("AccountSignID");
            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.ContractName).HasMaxLength(100);
            entity.Property(e => e.DateEnd).HasColumnType("datetime");
            entity.Property(e => e.DateSign).HasColumnType("datetime");
            entity.Property(e => e.DateStart).HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.AccountSign).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.AccountSignId)
                .HasConstraintName("FK_Contract_Account");

            entity.HasOne(d => d.Address).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK_Contract_Address");
        });

        modelBuilder.Entity<ContractPayment>(entity =>
        {
            entity.HasKey(e => e.ContractPaymentId).HasName("PK__Contract__DAE23998B7192799");

            entity.ToTable("ContractPayment");

            entity.Property(e => e.ContractPaymentId)
                .ValueGeneratedNever()
                .HasColumnName("ContractPaymentID");
            entity.Property(e => e.ContractId).HasColumnName("ContractID");
            entity.Property(e => e.CustomerPaidDate).HasColumnType("datetime");
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.SystemPaidDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Contract).WithMany(p => p.ContractPayments)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_ContractPayment_ContractID");
        });

        modelBuilder.Entity<ContractTerm>(entity =>
        {
            entity.HasKey(e => e.ContractTermId).HasName("PK__Contract__0DE4188D4B9C085F");

            entity.ToTable("ContractTerm");

            entity.Property(e => e.ContractTermId)
                .ValueGeneratedNever()
                .HasColumnName("ContractTermID");
            entity.Property(e => e.ContractId).HasColumnName("ContractID");
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Contract).WithMany(p => p.ContractTerms)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_ContractTerm_ContractID");
        });

        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.HasKey(e => e.DeliveryId).HasName("PK__Delivery__626D8FEE9C6A20D2");

            entity.ToTable("Delivery");

            entity.Property(e => e.DeliveryId)
                .ValueGeneratedNever()
                .HasColumnName("DeliveryID");
            entity.Property(e => e.ContractId).HasColumnName("ContractID");
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.DateShip).HasColumnType("datetime");
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Contract).WithMany(p => p.Deliveries)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_Delivery_ContractID");

            entity.HasOne(d => d.Staff).WithMany(p => p.Deliveries)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_Delivery_StaffID");
        });

        modelBuilder.Entity<DiscountType>(entity =>
        {
            entity.HasKey(e => e.DiscountTypeId).HasName("PK__Discount__6CCE1DD67A3D868B");

            entity.ToTable("DiscountType");

            entity.Property(e => e.DiscountTypeId)
                .ValueGeneratedNever()
                .HasColumnName("DiscountTypeID");
            entity.Property(e => e.DiscountTypeName).HasMaxLength(100);
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__6A4BEDF65BE7DFDC");

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId)
                .ValueGeneratedNever()
                .HasColumnName("FeedbackID");
            entity.Property(e => e.AccountFeedbackId).HasColumnName("AccountFeedbackID");
            entity.Property(e => e.ContractId).HasColumnName("ContractID");
            entity.Property(e => e.DateCreate).HasColumnType("datetime");

            entity.HasOne(d => d.AccountFeedback).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.AccountFeedbackId)
                .HasConstraintName("FK_Feedback_Account");

            entity.HasOne(d => d.Contract).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_Feedback_Contract");
        });

        modelBuilder.Entity<HiringRequest>(entity =>
        {
            entity.HasKey(e => e.HiringRequestId).HasName("PK__HiringRe__6D744848F51C473B");

            entity.ToTable("HiringRequest");

            entity.Property(e => e.HiringRequestId)
                .ValueGeneratedNever()
                .HasColumnName("HiringRequestID");
            entity.Property(e => e.AccountOrderId).HasColumnName("AccountOrderID");
            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.AccountOrder).WithMany(p => p.HiringRequests)
                .HasForeignKey(d => d.AccountOrderId)
                .HasConstraintName("FK_HiringRequest_Account");

            entity.HasOne(d => d.Address).WithMany(p => p.HiringRequests)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK_HiringRequest_Address");
        });

        modelBuilder.Entity<HiringRequestProductDetail>(entity =>
        {
            entity.HasKey(e => e.HiringRequestProductDetailId).HasName("PK__HiringRe__9AE9E39584A15500");

            entity.ToTable("HiringRequestProductDetail");

            entity.Property(e => e.HiringRequestProductDetailId)
                .ValueGeneratedNever()
                .HasColumnName("HiringRequestProductDetailID");
            entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequestID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.HiringRequest).WithMany(p => p.HiringRequestProductDetails)
                .HasForeignKey(d => d.HiringRequestId)
                .HasConstraintName("FK_HiringRequestProductDetail_HiringRequest");

            entity.HasOne(d => d.Product).WithMany(p => p.HiringRequestProductDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_HiringRequestProductDetail_Product");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__Invoices__D796AAD5010636E8");

            entity.Property(e => e.InvoiceId)
                .ValueGeneratedNever()
                .HasColumnName("InvoiceID");
            entity.Property(e => e.ContractId).HasColumnName("ContractID");
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceCode).HasMaxLength(50);

            entity.HasOne(d => d.Contract).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_Invoices_Contract");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Log__5E5499A8AB7E12BB");

            entity.ToTable("Log");

            entity.Property(e => e.LogId)
                .ValueGeneratedNever()
                .HasColumnName("LogID");
            entity.Property(e => e.AccountLogId).HasColumnName("AccountLogID");
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.DateUpdate).HasColumnType("datetime");

            entity.HasOne(d => d.AccountLog).WithMany(p => p.Logs)
                .HasForeignKey(d => d.AccountLogId)
                .HasConstraintName("FK_Log_AccountLogID");
        });

        modelBuilder.Entity<LogDetail>(entity =>
        {
            entity.HasKey(e => e.LogDetailId).HasName("PK__LogDetai__D7AFD39630BDA15F");

            entity.ToTable("LogDetail");

            entity.Property(e => e.LogDetailId)
                .ValueGeneratedNever()
                .HasColumnName("LogDetailID");
            entity.Property(e => e.Action).HasMaxLength(100);
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.LogId).HasColumnName("LogID");

            entity.HasOne(d => d.Log).WithMany(p => p.LogDetails)
                .HasForeignKey(d => d.LogId)
                .HasConstraintName("FK_LogDetail_LogID");
        });

        modelBuilder.Entity<MaintainingTicket>(entity =>
        {
            entity.HasKey(e => e.MaintainingTicketId).HasName("PK__Maintain__76F8D53F2FA1A432");

            entity.ToTable("MaintainingTicket");

            entity.Property(e => e.MaintainingTicketId)
                .ValueGeneratedNever()
                .HasColumnName("MaintainingTicketID");
            entity.Property(e => e.ComponentId).HasColumnName("ComponentID");
            entity.Property(e => e.DateRepair).HasColumnType("datetime");
            entity.Property(e => e.SerialNumber).HasMaxLength(50);
            entity.Property(e => e.TaskId).HasColumnName("TaskID");

            entity.HasOne(d => d.Component).WithMany(p => p.MaintainingTickets)
                .HasForeignKey(d => d.ComponentId)
                .HasConstraintName("FK_MaintainingTicket_ComponentID");

            entity.HasOne(d => d.Task).WithMany(p => p.MaintainingTickets)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK_MaintainingTicket_TaskID");
        });

        modelBuilder.Entity<MaintenanceRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Maintena__33A8519AC913D9FF");

            entity.ToTable("MaintenanceRequest");

            entity.Property(e => e.RequestId)
                .ValueGeneratedNever()
                .HasColumnName("RequestID");
            entity.Property(e => e.ContractId).HasColumnName("ContractID");
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.SerialNumber).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Contract).WithMany(p => p.MaintenanceRequests)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_MaintenanceRequest_Contract");

            entity.HasOne(d => d.SerialNumberNavigation).WithMany(p => p.MaintenanceRequests)
                .HasForeignKey(d => d.SerialNumber)
                .HasConstraintName("FK_MaintenanceRequest_ProductNumber");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E32D5957DE2");

            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId)
                .ValueGeneratedNever()
                .HasColumnName("NotificationID");
            entity.Property(e => e.AccountReceiveId).HasColumnName("AccountReceiveID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.AccountReceive).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.AccountReceiveId)
                .HasConstraintName("FK_Notification_Account");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6EDC6EF03F3");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.Model).HasMaxLength(50);
            entity.Property(e => e.Origin).HasMaxLength(50);
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Product_Category");
        });

        modelBuilder.Entity<ProductComponentStatus>(entity =>
        {
            entity.HasKey(e => e.ProductComponentStatusId).HasName("PK__ProductC__D0CF1C0B55B7B53C");

            entity.ToTable("ProductComponentStatus");

            entity.Property(e => e.ProductComponentStatusId)
                .ValueGeneratedNever()
                .HasColumnName("ProductComponentStatusID");
            entity.Property(e => e.ComponentId).HasColumnName("ComponentID");
            entity.Property(e => e.SerialNumber).HasMaxLength(50);

            entity.HasOne(d => d.Component).WithMany(p => p.ProductComponentStatuses)
                .HasForeignKey(d => d.ComponentId)
                .HasConstraintName("FK_ProductComponentStatus_ComponentID");

            entity.HasOne(d => d.SerialNumberNavigation).WithMany(p => p.ProductComponentStatuses)
                .HasForeignKey(d => d.SerialNumber)
                .HasConstraintName("FK_ProductComponentStatus_SerialNumber");
        });

        modelBuilder.Entity<ProductDetail>(entity =>
        {
            entity.HasKey(e => e.ProductDetailId).HasName("PK__ProductD__3C8DD6943FF3401F");

            entity.ToTable("ProductDetail");

            entity.Property(e => e.ProductDetailId)
                .ValueGeneratedNever()
                .HasColumnName("ProductDetailID");
            entity.Property(e => e.ComponentId).HasColumnName("ComponentID");
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Component).WithMany(p => p.ProductDetails)
                .HasForeignKey(d => d.ComponentId)
                .HasConstraintName("FK_ProductDetail_Component");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductDetail_Product");
        });

        modelBuilder.Entity<ProductNumber>(entity =>
        {
            entity.HasKey(e => e.SerialNumber).HasName("PK__ProductN__048A00094C0B4787");

            entity.ToTable("ProductNumber");

            entity.Property(e => e.SerialNumber).HasMaxLength(50);
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductNumbers)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductNumber_Product");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.PromotionId).HasName("PK__Promotio__52C42F2F20B9C661");

            entity.ToTable("Promotion");

            entity.Property(e => e.PromotionId)
                .ValueGeneratedNever()
                .HasColumnName("PromotionID");
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.DateEnd).HasColumnType("datetime");
            entity.Property(e => e.DateStart).HasColumnType("datetime");
            entity.Property(e => e.DiscountTypeId).HasColumnName("DiscountTypeID");
            entity.Property(e => e.PromotionTypeId).HasColumnName("PromotionTypeID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.DiscountType).WithMany(p => p.Promotions)
                .HasForeignKey(d => d.DiscountTypeId)
                .HasConstraintName("FK_Promotion_DiscountTypeID");

            entity.HasOne(d => d.PromotionType).WithMany(p => p.Promotions)
                .HasForeignKey(d => d.PromotionTypeId)
                .HasConstraintName("FK_Promotion_PromotionTypeID");
        });

        modelBuilder.Entity<PromotionType>(entity =>
        {
            entity.HasKey(e => e.PromotionTypeId).HasName("PK__Promotio__01055C88A6EC5679");

            entity.ToTable("PromotionType");

            entity.Property(e => e.PromotionTypeId)
                .ValueGeneratedNever()
                .HasColumnName("PromotionTypeID");
            entity.Property(e => e.PromotionTypeName).HasMaxLength(100);
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Report__D5BD48E5695291A2");

            entity.ToTable("Report");

            entity.Property(e => e.ReportId)
                .ValueGeneratedNever()
                .HasColumnName("ReportID");
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.TaskId).HasColumnName("TaskID");

            entity.HasOne(d => d.Task).WithMany(p => p.Reports)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK_Report_TaskID");
        });

        modelBuilder.Entity<RequestDateResponse>(entity =>
        {
            entity.HasKey(e => e.ResponseDateId).HasName("PK__RequestD__48F98BA5F14FBF58");

            entity.ToTable("RequestDateResponse");

            entity.Property(e => e.ResponseDateId)
                .ValueGeneratedNever()
                .HasColumnName("ResponseDateID");
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.DateResponse).HasColumnType("datetime");
            entity.Property(e => e.RequestId).HasColumnName("RequestID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Request).WithMany(p => p.RequestDateResponses)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("FK_RequestDateResponse_MaintenanceRequest");
        });

        modelBuilder.Entity<SerialMechanicalMachinery>(entity =>
        {
            entity.HasKey(e => e.ContractOrderId).HasName("PK__SerialMe__392293D3B4D0CDA7");

            entity.ToTable("SerialMechanicalMachinery");

            entity.Property(e => e.ContractOrderId)
                .ValueGeneratedNever()
                .HasColumnName("ContractOrderID");
            entity.Property(e => e.ContractId).HasColumnName("ContractID");
            entity.Property(e => e.SerialNumber).HasMaxLength(50);

            entity.HasOne(d => d.Contract).WithMany(p => p.SerialMechanicalMachineries)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_SerialMechanicalMachinery_ContractID");

            entity.HasOne(d => d.SerialNumberNavigation).WithMany(p => p.SerialMechanicalMachineries)
                .HasForeignKey(d => d.SerialNumber)
                .HasConstraintName("FK_SerialMechanicalMachinery_SerialNumber");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Task__7C6949D10BD82AF2");

            entity.ToTable("Task");

            entity.Property(e => e.TaskId)
                .ValueGeneratedNever()
                .HasColumnName("TaskID");
            entity.Property(e => e.AssigneeId).HasColumnName("AssigneeID");
            entity.Property(e => e.ContractId).HasColumnName("ContractID");
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.Deadline).HasColumnType("datetime");
            entity.Property(e => e.ReporterId).HasColumnName("ReporterID");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TaskTitle).HasMaxLength(100);

            entity.HasOne(d => d.Assignee).WithMany(p => p.TaskAssignees)
                .HasForeignKey(d => d.AssigneeId)
                .HasConstraintName("FK_Task_Assignee");

            entity.HasOne(d => d.Contract).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_Task_Contract");

            entity.HasOne(d => d.Reporter).WithMany(p => p.TaskReporters)
                .HasForeignKey(d => d.ReporterId)
                .HasConstraintName("FK_Task_Reporter");
        });

        modelBuilder.Entity<TaskLog>(entity =>
        {
            entity.HasKey(e => e.TaskLogId).HasName("PK__TaskLog__36DB6FA9464E938C");

            entity.ToTable("TaskLog");

            entity.Property(e => e.TaskLogId)
                .ValueGeneratedNever()
                .HasColumnName("TaskLogID");
            entity.Property(e => e.Action).HasMaxLength(100);
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.TaskId).HasColumnName("TaskID");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskLogs)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK_TaskLog_TaskID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
