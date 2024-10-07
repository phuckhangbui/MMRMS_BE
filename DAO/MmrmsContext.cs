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

    public virtual DbSet<ProductTerm> ProductTerms { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Component> Components { get; set; }

    public virtual DbSet<ComponentProduct> ComponentProducts { get; set; }

    public virtual DbSet<Content> Contents { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<ContractAddress> ContractAddresses { get; set; }

    public virtual DbSet<ContractPayment> ContractPayments { get; set; }

    public virtual DbSet<ContractTerm> ContractTerms { get; set; }

    public virtual DbSet<Delivery> Deliveries { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<RentingRequest> RentingRequests { get; set; }

    public virtual DbSet<RentingRequestProductDetail> RentingRequestProductDetails { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<LogDetail> LogDetails { get; set; }

    public virtual DbSet<MaintenanceTicket> MaintenanceTickets { get; set; }

    public virtual DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductComponentStatus> ProductComponentStatuses { get; set; }

    public virtual DbSet<SerialNumberProduct> SerialNumberProducts { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<MembershipRank> MembershipRanks { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<RequestResponse> RequestResponses { get; set; }

    //public virtual DbSet<ContractSerialNumberProduct> ContractSerialNumberProducts { get; set; }

    public virtual DbSet<EmployeeTask> EmployeeTasks { get; set; }

    public virtual DbSet<TaskLog> TaskLogs { get; set; }

    public virtual DbSet<ProductComponentStatusLog> ProductComponentStatusLogs { get; set; }

    public virtual DbSet<RentingService> RentingServices { get; set; }

    public virtual DbSet<ServiceRentingRequest> ServiceRentingRequests { get; set; }

    public virtual DbSet<DeliveryLog> DeliveryLogs { get; set; }


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

            entity.HasOne(d => d.MembershipRank).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.MembershipRankId)
                .HasConstraintName("FK_Account_MembershipRank");

            entity.HasOne(d => d.AccountBusiness)
                .WithOne(p => p.Account)
                .HasForeignKey<AccountBusiness>(d => d.AccountBusinessId)
                .HasConstraintName("FK_Account_AccountBusiness");

        });




        modelBuilder.Entity<AccountBusiness>(entity =>
        {
            entity.HasKey(e => e.AccountBusinessId);

            entity.ToTable("AccountBusiness");

            entity.Property(e => e.AccountBusinessId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Account)
                .WithOne(p => p.AccountBusiness)
                .HasForeignKey<AccountBusiness>(d => d.AccountId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_AccountBusiness_Account");
        });

        modelBuilder.Entity<AccountPromotion>(entity =>
        {
            entity.HasKey(e => e.AccountPromotionId);

            entity.ToTable("AccountPromotion");

            entity.Property(e => e.AccountPromotionId)
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

            entity.ToTable("ProductAttribute");

            entity.Property(e => e.ProductAttributeId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Product).WithMany(p => p.ProductAttributes)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Attribute_Product");
        });

        modelBuilder.Entity<ProductTerm>(entity =>
        {
            entity.HasKey(e => e.ProductTermId);

            entity.ToTable("ProductTerm");

            entity.Property(e => e.ProductTermId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Product).WithMany(p => p.ProductTerms)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Term_Product");
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
        });

        modelBuilder.Entity<ContractAddress>(entity =>
        {
            entity.HasKey(e => e.ContractAddressId);

            entity.ToTable("ContractAddress");

            entity.HasOne(d => d.Contract)
               .WithOne(p => p.ContractAddress)
               .HasForeignKey<Contract>(p => p.ContractAddressId)
               .OnDelete(DeleteBehavior.Restrict) // Prevent cascading delete
               .HasConstraintName("FK_ContractAddress_Contract");
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.ContractId);

            entity.ToTable("Contract");

            entity.HasOne(d => d.AccountSign).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.AccountSignId)
                .HasConstraintName("FK_Contract_Account");

            entity.HasOne(d => d.AccountCreate).WithMany(p => p.CreateContracts)
                .HasForeignKey(d => d.AccountCreateId)
                .HasConstraintName("FK_Contract_Account_Create");

            entity.HasOne(d => d.ContractAddress).WithOne(p => p.Contract)
                .HasForeignKey<Contract>(p => p.ContractAddressId)
                .IsRequired()
                .HasConstraintName("FK_ContractAddress_Contract");

            //entity.HasOne(d => d.RentingRequest).WithOne(d => d.Contract)
            //    .HasConstraintName("FK_Contract_RentingRequest");

            entity.HasOne(d => d.RentingRequest)
                .WithMany(p => p.Contracts)
                .HasForeignKey(d => d.RentingRequestId)
                .IsRequired() // Contract requires RentingRequestId
                .HasConstraintName("FK_Contract_RentingRequest");

            entity.HasOne(d => d.ContractSerialNumberProduct)
                .WithMany(p => p.Contracts) // Assuming SerialNumberProduct has 'Contracts' collection
                .HasForeignKey(d => d.SerialNumber)
                .HasConstraintName("FK_Contract_SerialNumberProduct")
                .IsRequired();
        });

        modelBuilder.Entity<RentingRequest>(entity =>
        {
            entity.HasKey(e => e.RentingRequestId);

            entity.ToTable("RentingRequest");

            entity.HasOne(d => d.AccountOrder).WithMany(p => p.RentingRequests)
                .HasForeignKey(d => d.AccountOrderId)
                .HasConstraintName("FK_RentingRequest_Account");

            entity.HasOne(d => d.Address).WithMany(p => p.RentingRequests)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK_RentingRequest_Address");

            entity.HasMany(d => d.Contracts)
                .WithOne(p => p.RentingRequest)
                .HasForeignKey(p => p.RentingRequestId)
                .OnDelete(DeleteBehavior.Restrict) // Prevent cascading delete
                .HasConstraintName("FK_RentingRequest_Contract");
        });

        modelBuilder.Entity<ContractPayment>(entity =>
        {
            entity.HasKey(e => e.ContractPaymentId);

            entity.ToTable("ContractPayment");

            entity.Property(e => e.ContractPaymentId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            // Foreign key for Contract
            entity.HasOne(d => d.Contract)
                .WithMany(p => p.ContractPayments)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_ContractPayment_ContractID");

            // One-to-one with Invoice, using ContractPaymentId as foreign key in Invoice
            entity.HasOne(d => d.Invoice)
                .WithOne(p => p.ContractPayment)
                .HasForeignKey<Invoice>(d => d.ContractPaymentId)  // Use ContractPaymentId as the foreign key
                .HasConstraintName("FK_Invoice_ContractPayment");
        });


        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId);

            // One-to-one relationship with ContractPayment
            entity.HasOne(d => d.ContractPayment)
                .WithOne(p => p.Invoice)
                .HasForeignKey<Invoice>(d => d.ContractPaymentId)  // Use ContractPaymentId
                .HasConstraintName("FK_Invoice_ContractPayment");

            // One-to-many relationship with AccountPaid
            entity.HasOne(d => d.AccountPaid)
                .WithMany(p => p.Invoices)
                .HasForeignKey(d => d.AccountPaidId)
                .HasConstraintName("FK_Invoices_Account");

            // One-to-one relationship with MaintainTicket
            entity.HasOne(d => d.MaintenanceTicket)
                .WithOne(p => p.Invoice)
                .HasForeignKey<Invoice>(d => d.MaintainTicketId)
                .HasConstraintName("FK_Invoice_MaintainTicket");
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



        modelBuilder.Entity<RentingRequestProductDetail>(entity =>
        {
            entity.HasKey(e => e.RentingRequestProductDetailId);

            entity.ToTable("RentingRequestProductDetail");

            entity.Property(e => e.RentingRequestProductDetailId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.RentingRequest).WithMany(p => p.RentingRequestProductDetails)
                .HasForeignKey(d => d.RentingRequestId)
                .HasConstraintName("FK_RentingRequestProductDetail_RentingRequest");

            entity.HasOne(d => d.Product).WithMany(p => p.RentingRequestProductDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_RentingRequestProductDetail_Product");
        });




        modelBuilder.Entity<LogDetail>(entity =>
        {
            entity.HasKey(e => e.LogDetailId);

            entity.ToTable("LogDetail");

            entity.Property(e => e.LogDetailId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Account).WithMany(p => p.LogDetails)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_LogDetail_AccountID");
        });

        modelBuilder.Entity<MaintenanceTicket>(entity =>
        {
            entity.HasKey(e => e.MaintenanceTicketId).HasName("PK__Maintain__76F8D53F2FA1A432");

            entity.ToTable("MaintenanceTicket");

            entity.Property(e => e.MaintenanceTicketId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Component).WithMany(p => p.MaintenanceTickets)
                .HasForeignKey(d => d.ComponentId)
                .HasConstraintName("FK_MaintenanceTicket_ComponentID");

            entity.HasOne(d => d.EmployeeTask).WithMany(p => p.MaintenanceTickets)
                .HasForeignKey(d => d.EmployeeTaskId)
                .HasConstraintName("FK_MaintenanceTicket_TaskID");

            entity.HasOne(d => d.SerialNumberProduct).WithMany(p => p.MaintenanceTickets)
                .HasForeignKey(d => d.ProductSerialNumber)
                .HasConstraintName("FK_MaintenanceTicket_SerialNumberProduct");

            entity.HasOne(d => d.Contract).WithMany(p => p.MaintenanceTickets)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_MaintenanceTicket_ContractID");

            entity.HasOne(d => d.Invoice)
                .WithOne(p => p.MaintenanceTicket)
                .HasForeignKey<MaintenanceTicket>(d => d.InvoiceId)
                .HasConstraintName("FK_Invoice_MaintainTicket");

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
                .OnDelete(DeleteBehavior.Cascade)
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
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ProductComponentStatus_SerialNumberProduct");
        });

        modelBuilder.Entity<ProductComponentStatusLog>(entity =>
        {
            entity.HasKey(e => e.ProductComponentStatusLogId);

            entity.ToTable("ProductComponentStatusLog");

            entity.Property(e => e.ProductComponentStatusLogId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.ProductComponentStatus).WithMany(p => p.ProductComponentStatusLogs)
                .HasForeignKey(d => d.ProductComponentStatusId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ProductComponentStatus_Log");
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

        });

        modelBuilder.Entity<MembershipRank>(entity =>
        {
            entity.HasKey(e => e.MembershipRankId);

            entity.ToTable("MembershipRank");

            entity.Property(e => e.MembershipRankId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

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

        modelBuilder.Entity<RequestResponse>(entity =>
        {
            entity.HasKey(e => e.RequestResponseId);

            entity.ToTable("RequestResponse");

            entity.Property(e => e.RequestResponseId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Request).WithMany(p => p.RequestResponses)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("FK_RequestResponse_MaintenanceRequest");

            entity.HasOne(d => d.EmployeeTask)
                  .WithOne(t => t.RequestResponse)
                  .HasForeignKey<EmployeeTask>(t => t.RequestResponseId)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("FK_Response_Task");
        });

        //modelBuilder.Entity<ContractSerialNumberProduct>(entity =>
        //{
        //    entity.HasKey(e => e.ContractSerialNumberProductId);

        //    entity.ToTable("ContractSerialNumberProduct");

        //    entity.Property(e => e.ContractSerialNumberProductId)
        //        .ValueGeneratedOnAdd()
        //        .UseIdentityColumn();

        //    entity.HasOne(d => d.Contract).WithMany(p => p.ContractSerialNumberProducts)
        //        .HasForeignKey(d => d.ContractId)
        //        .HasConstraintName("FK_SerialMechanicalMachinery_ContractID");

        //    entity.HasOne(d => d.SerialNumberProduct).WithMany(p => p.ContractSerialNumberProducts)
        //        .HasForeignKey(d => d.SerialNumber)
        //        .HasConstraintName("FK_SerialMechanicalMachinery_SerialNumber");
        //});

        modelBuilder.Entity<EmployeeTask>(entity =>
        {
            entity.HasKey(e => e.EmployeeTaskId);

            entity.ToTable("EmployeeTask");

            entity.Property(e => e.EmployeeTaskId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Staff).WithMany(p => p.TaskReceivedList)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_Task_Staff");

            entity.HasOne(d => d.Contract).WithMany(p => p.EmployeeTasks)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_Task_Contract");

            entity.HasOne(d => d.Manager).WithMany(p => p.TaskGaveList)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK_Task_Manager");

            entity.HasOne(d => d.RequestResponse)
                  .WithOne(t => t.EmployeeTask)
                  .HasForeignKey<EmployeeTask>(t => t.RequestResponseId)
                  .HasConstraintName("FK_Task_Response");
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
                  .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TaskLog_TaskID");

            entity.HasOne(d => d.AccountTrigger).WithMany(p => p.TaskLogs)
                .HasForeignKey(d => d.AccountTriggerId)

                .HasConstraintName("FK_TaskLog_AccountID");
        });

        modelBuilder.Entity<DeliveryLog>(entity =>
        {
            entity.HasKey(e => e.DeliveryLogId);

            entity.ToTable("DeliveryLog");

            entity.Property(e => e.DeliveryLogId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Delivery).WithMany(p => p.DeliveryLogs)
                .HasForeignKey(d => d.DeliveryId)
                .HasConstraintName("FK_DeliveryLog_DeliveryID");

            entity.HasOne(d => d.AccountTrigger).WithMany(p => p.DeliveryLogs)
                .HasForeignKey(d => d.AccountTriggerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DeliveryLog_AccountID");
        });

        modelBuilder.Entity<RentingService>(entity =>
        {
            entity.HasKey(e => e.RentingServiceId);

            entity.ToTable("RentingService");

            entity.Property(e => e.RentingServiceId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();
        });

        modelBuilder.Entity<ServiceRentingRequest>(entity =>
        {
            entity.HasKey(e => e.ServiceRentingRequestId);

            entity.ToTable("ServiceRentingRequest");

            entity.Property(e => e.ServiceRentingRequestId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.RentingService).WithMany(p => p.ServiceRentingRequests)
               .HasForeignKey(d => d.RentingServiceId)
               .HasConstraintName("FK_rentingservice_servicerequest");

            entity.HasOne(d => d.RentingRequest).WithMany(p => p.ServiceRentingRequests)
               .HasForeignKey(d => d.RentingRequestId)
               .HasConstraintName("FK_servicerequest_rentingrequest");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
