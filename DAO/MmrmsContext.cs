using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAO;

public partial class MmrmsContext : DbContext
{
    public MmrmsContext()
    {
    }

    public MmrmsContext(DbContextOptions<MmrmsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountBusiness> AccountBusinesses { get; set; }

    public virtual DbSet<AccountPromotion> AccountPromotions { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<ProductAttribute> ProductAttributes { get; set; }

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

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductComponentStatus> ProductComponentStatuses { get; set; }

    public virtual DbSet<SerialNumberProduct> ProductNumbers { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<PromotionType> PromotionTypes { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<RequestDateResponse> RequestDateResponses { get; set; }

    public virtual DbSet<ContractSerialNumberProduct> ContractSerialNumberProducts { get; set; }

    public virtual DbSet<EmployeeTask> EmployeeTasks { get; set; }

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
            entity.HasKey(e => e.AccountId);

            entity.ToTable("Account");

            entity.Property(e => e.AccountId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Promotion).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK_Account_Promotion");
        });

        modelBuilder.Entity<AccountBusiness>(entity =>
        {
            entity.HasKey(e => e.AccountBusinessId);

            entity.ToTable("AccountBusiness");

            entity.Property(e => e.AccountBusinessId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Account).WithMany(p => p.AccountBusinesses)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_AccountBusiness_Account");
        });

        modelBuilder.Entity<AccountPromotion>(entity =>
        {
            entity.HasKey(e => e.PromotionAccountId);

            entity.ToTable("AccountPromotion");

            entity.Property(e => e.PromotionAccountId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Account).WithMany(p => p.AccountPromotions)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_AccountPromotion_Account");

            entity.HasOne(d => d.Promotion).WithMany(p => p.AccountPromotions)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK_AccountPromotion_Promotion");
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId);

            entity.ToTable("Address");

            entity.Property(e => e.AddressId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Account).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Address_Account");
        });

        modelBuilder.Entity<ProductAttribute>(entity =>
        {
            entity.HasKey(e => e.ProductAttributeId);

            entity.ToTable("Attribute");

            entity.Property(e => e.ProductAttributeId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Product).WithMany(p => p.ProductAttributes)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Attribute_Product");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId);

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();
        });

        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => e.ComponentId);

            entity.ToTable("Component");

            entity.Property(e => e.ComponentId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

        });

        modelBuilder.Entity<ComponentProduct>(entity =>
        {
            entity.HasKey(e => e.ComponentProductId);

            entity.ToTable("ComponentProduct");

            entity.Property(e => e.ComponentProductId)
                 .ValueGeneratedOnAdd()
                 .UseIdentityColumn();

            entity.HasOne(d => d.Component).WithMany(p => p.ComponentProducts)
                .HasForeignKey(d => d.ComponentId)
                .HasConstraintName("FK_ComponentProduct_ComponentID");

            entity.HasOne(d => d.Product).WithMany(p => p.ComponentProducts)
              .HasForeignKey(d => d.ProductId)
              .HasConstraintName("FK_ComponentProduct_ProductID");
        });

        modelBuilder.Entity<Content>(entity =>
        {
            entity.HasKey(e => e.ContentId);

            entity.ToTable("Content");

            entity.Property(e => e.ContentId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.AccountCreate).WithMany(p => p.Contents)
                .HasForeignKey(d => d.AccountCreateId)
                .HasConstraintName("FK_Content_Account");
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.ContractId);

            entity.ToTable("Contract");

            entity.Property(e => e.ContractId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.AccountSign).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.AccountSignId)
                .HasConstraintName("FK_Contract_Account");

            entity.HasOne(d => d.Address).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK_Contract_Address");
        });

        modelBuilder.Entity<ContractPayment>(entity =>
        {
            entity.HasKey(e => e.ContractPaymentId);

            entity.ToTable("ContractPayment");

            entity.Property(e => e.ContractPaymentId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Contract).WithMany(p => p.ContractPayments)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_ContractPayment_ContractID");
        });

        modelBuilder.Entity<ContractTerm>(entity =>
        {
            entity.HasKey(e => e.ContractTermId);

            entity.ToTable("ContractTerm");

            entity.HasOne(d => d.Contract).WithMany(p => p.ContractTerms)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_ContractTerm_ContractID");
        });

        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.HasKey(e => e.DeliveryId);

            entity.ToTable("Delivery");

            entity.Property(e => e.DeliveryId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Contract).WithMany(p => p.Deliveries)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_Delivery_ContractID");

            entity.HasOne(d => d.Staff).WithMany(p => p.Deliveries)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_Delivery_StaffID");
        });

        modelBuilder.Entity<DiscountType>(entity =>
        {
            entity.HasKey(e => e.DiscountTypeId);

            entity.ToTable("DiscountType");

            entity.Property(e => e.DiscountTypeId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId);

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.AccountFeedback).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.AccountFeedbackId)
                .HasConstraintName("FK_Feedback_Account");

            entity.HasOne(d => d.Contract).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_Feedback_Contract");
        });

        modelBuilder.Entity<HiringRequest>(entity =>
        {
            entity.HasKey(e => e.HiringRequestId);

            entity.ToTable("HiringRequest");

            entity.Property(e => e.HiringRequestId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.AccountOrder).WithMany(p => p.HiringRequests)
                .HasForeignKey(d => d.AccountOrderId)
                .HasConstraintName("FK_HiringRequest_Account");

            entity.HasOne(d => d.Address).WithMany(p => p.HiringRequests)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK_HiringRequest_Address");
        });

        modelBuilder.Entity<HiringRequestProductDetail>(entity =>
        {
            entity.HasKey(e => e.HiringRequestProductDetailId);

            entity.ToTable("HiringRequestProductDetail");

            entity.Property(e => e.HiringRequestProductDetailId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.HiringRequest).WithMany(p => p.HiringRequestProductDetails)
                .HasForeignKey(d => d.HiringRequestId)
                .HasConstraintName("FK_HiringRequestProductDetail_HiringRequest");

            entity.HasOne(d => d.Product).WithMany(p => p.HiringRequestProductDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_HiringRequestProductDetail_Product");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId);

            entity.Property(e => e.InvoiceId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Contract).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_Invoices_Contract");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.LogId);

            entity.ToTable("Log");

            entity.Property(e => e.LogId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.AccountLog).WithMany(p => p.Logs)
                .HasForeignKey(d => d.AccountLogId)
                .HasConstraintName("FK_Log_AccountLogID");
        });

        modelBuilder.Entity<LogDetail>(entity =>
        {
            entity.HasKey(e => e.LogDetailId);

            entity.ToTable("LogDetail");

            entity.Property(e => e.LogDetailId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Log).WithMany(p => p.LogDetails)
                .HasForeignKey(d => d.LogId)
                .HasConstraintName("FK_LogDetail_LogID");
        });

        modelBuilder.Entity<MaintainingTicket>(entity =>
        {
            entity.HasKey(e => e.MaintainingTicketId).HasName("PK__Maintain__76F8D53F2FA1A432");

            entity.ToTable("MaintainingTicket");

            entity.Property(e => e.MaintainingTicketId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Component).WithMany(p => p.MaintainingTickets)
                .HasForeignKey(d => d.ComponentId)
                .HasConstraintName("FK_MaintainingTicket_ComponentID");

            entity.HasOne(d => d.EmployeeTask).WithMany(p => p.MaintainingTickets)
                .HasForeignKey(d => d.EmployeeTaskId)
                .HasConstraintName("FK_MaintainingTicket_TaskID");
        });

        modelBuilder.Entity<MaintenanceRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId);

            entity.ToTable("MaintenanceRequest");

            entity.Property(e => e.RequestId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Contract).WithMany(p => p.MaintenanceRequests)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_MaintenanceRequest_Contract");

            entity.HasOne(d => d.SerialNumberProduct).WithMany(p => p.MaintenanceRequests)
                .HasForeignKey(d => d.SerialNumber)
                .HasConstraintName("FK_MaintenanceRequest_SerialNumberProduct");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId);

            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.AccountReceive).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.AccountReceiveId)
                .HasConstraintName("FK_Notification_Account");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId);

            entity.ToTable("Product");

            entity.Property(e => e.ProductId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Product_Category");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.ProductImageId);

            entity.ToTable("ProductImage");

            entity.Property(e => e.ProductImageId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductImage_Product");
        });

        modelBuilder.Entity<ProductComponentStatus>(entity =>
        {
            entity.HasKey(e => e.ProductComponentStatusId);

            entity.ToTable("ProductComponentStatus");

            entity.Property(e => e.ProductComponentStatusId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Component).WithMany(p => p.ProductComponentStatuses)
                .HasForeignKey(d => d.ComponentId)
                .HasConstraintName("FK_ProductComponentStatus_ComponentID");

            entity.HasOne(d => d.SerialNumberProduct).WithMany(p => p.ProductComponentStatuses)
                .HasForeignKey(d => d.SerialNumber)
                .HasConstraintName("FK_ProductComponentStatus_SerialNumberProduct");
        });

        modelBuilder.Entity<SerialNumberProduct>(entity =>
        {
            entity.HasKey(e => e.SerialNumber);

            entity.ToTable("SerialNumberProduct");

            entity.HasOne(d => d.Product).WithMany(p => p.SerialNumberProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductNumber_Product");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.PromotionId);

            entity.ToTable("Promotion");

            entity.Property(e => e.PromotionId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.DiscountType).WithMany(p => p.Promotions)
                .HasForeignKey(d => d.DiscountTypeId)
                .HasConstraintName("FK_Promotion_DiscountTypeID");

            entity.HasOne(d => d.PromotionType).WithMany(p => p.Promotions)
                .HasForeignKey(d => d.PromotionTypeId)
                .HasConstraintName("FK_Promotion_PromotionTypeID");
        });

        modelBuilder.Entity<PromotionType>(entity =>
        {
            entity.HasKey(e => e.PromotionTypeId);

            entity.ToTable("PromotionType");

            entity.Property(e => e.PromotionTypeId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();
            entity.Property(e => e.PromotionTypeName).HasMaxLength(100);
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId);

            entity.ToTable("Report");

            entity.Property(e => e.ReportId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.EmployeeTask).WithMany(p => p.Reports)
                .HasForeignKey(d => d.EmployeeTaskId)
                .HasConstraintName("FK_Report_TaskID");
        });

        modelBuilder.Entity<RequestDateResponse>(entity =>
        {
            entity.HasKey(e => e.ResponseDateId);

            entity.ToTable("RequestDateResponse");

            entity.Property(e => e.ResponseDateId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Request).WithMany(p => p.RequestDateResponses)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("FK_RequestDateResponse_MaintenanceRequest");
        });

        modelBuilder.Entity<ContractSerialNumberProduct>(entity =>
        {
            entity.HasKey(e => e.ContractSerialNumberProductId);

            entity.ToTable("ContractSerialNumberProduct");

            entity.Property(e => e.ContractSerialNumberProductId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Contract).WithMany(p => p.ContractSerialNumberProducts)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_SerialMechanicalMachinery_ContractID");

            entity.HasOne(d => d.SerialNumberProduct).WithMany(p => p.ContractSerialNumberProducts)
                .HasForeignKey(d => d.SerialNumber)
                .HasConstraintName("FK_SerialMechanicalMachinery_SerialNumber");
        });

        modelBuilder.Entity<EmployeeTask>(entity =>
        {
            entity.HasKey(e => e.EmployeeTaskId);

            entity.ToTable("Task");

            entity.Property(e => e.EmployeeTaskId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Assignee).WithMany(p => p.TaskAssignees)
                .HasForeignKey(d => d.AssigneeId)
                .HasConstraintName("FK_Task_Assignee");

            entity.HasOne(d => d.Contract).WithMany(p => p.EmployeeTasks)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_Task_Contract");

            entity.HasOne(d => d.Reporter).WithMany(p => p.TaskReporters)
                .HasForeignKey(d => d.ReporterId)
                .HasConstraintName("FK_Task_Reporter");
        });

        modelBuilder.Entity<TaskLog>(entity =>
        {
            entity.HasKey(e => e.TaskLogId);

            entity.ToTable("TaskLog");

            entity.Property(e => e.TaskLogId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.EmployeeTask).WithMany(p => p.TaskLogs)
                .HasForeignKey(d => d.EmployeeTaskId)
                .HasConstraintName("FK_TaskLog_TaskID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
