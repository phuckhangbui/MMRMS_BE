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

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<AccountBusiness> AccountBusinesses { get; set; }

    //public virtual DbSet<AccountPromotion> AccountPromotions { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<MachineAttribute> MachineAttributes { get; set; }

    public virtual DbSet<MachineTerm> MachineTerms { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Component> Components { get; set; }

    public virtual DbSet<MachineComponent> MachineComponents { get; set; }

    public virtual DbSet<Content> Contents { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<ContractPayment> ContractPayments { get; set; }

    public virtual DbSet<ContractTerm> ContractTerms { get; set; }

    public virtual DbSet<DeliveryTask> Deliveries { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<RentingRequest> RentingRequests { get; set; }

    public virtual DbSet<RentingRequestMachineDetail> RentingRequestMachineDetails { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<LogDetail> LogDetails { get; set; }

    public virtual DbSet<ComponentReplacementTicket> ComponentReplacementTickets { get; set; }

    public virtual DbSet<ComponentReplacementTicketLog> ComponentReplacementTicketLogs { get; set; }

    public virtual DbSet<MachineCheckRequest> MachineCheckRequests { get; set; }

    public virtual DbSet<MachineCheckCriteria> MachineCheckCriterias { get; set; }

    public virtual DbSet<MachineCheckRequestCriteria> MachineCheckRequestCriterias { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Machine> Machines { get; set; }

    public virtual DbSet<MachineImage> MachineImages { get; set; }

    public virtual DbSet<MachineComponentStatus> MachineComponentStatuses { get; set; }

    public virtual DbSet<MachineSerialNumber> MachineSerialNumbers { get; set; }

    //public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<MembershipRank> MembershipRanks { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<RequestResponse> RequestResponses { get; set; }

    public virtual DbSet<RentingRequestAddress> RentingRequestAddresses { get; set; }

    public virtual DbSet<MachineTask> MachineTasks { get; set; }

    public virtual DbSet<MachineTaskLog> MachineTaskLogs { get; set; }

    public virtual DbSet<MachineSerialNumberLog> MachineSerialNumberLogs { get; set; }

    public virtual DbSet<RentingService> RentingServices { get; set; }

    public virtual DbSet<ServiceRentingRequest> ServiceRentingRequests { get; set; }

    public virtual DbSet<DeliveryTaskLog> DeliveryTaskLogs { get; set; }

    public virtual DbSet<Term> Terms { get; set; }

    public virtual DbSet<DigitalTransaction> DigitalTransactions { get; set; }

    public virtual DbSet<MembershipRankLog> MembershipRankLogs { get; set; }


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

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
               .HasForeignKey(d => d.RoleId)
               .HasConstraintName("FK_Account_Role");

            entity.HasOne(d => d.AccountBusiness)
                .WithOne(p => p.Account)
                .HasForeignKey<AccountBusiness>(d => d.AccountBusinessId)
                .HasConstraintName("FK_Account_AccountBusiness");

        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("Role");
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

        //modelBuilder.Entity<AccountPromotion>(entity =>
        //{
        //    entity.HasKey(e => e.AccountPromotionId);

        //    entity.ToTable("AccountPromotion");

        //    entity.Property(e => e.AccountPromotionId)
        //        .ValueGeneratedOnAdd()
        //        .UseIdentityColumn();

        //    entity.HasOne(d => d.Account).WithMany(p => p.AccountPromotions)
        //        .HasForeignKey(d => d.AccountId)
        //        .HasConstraintName("FK_AccountPromotion_Account");

        //    entity.HasOne(d => d.Promotion).WithMany(p => p.AccountPromotions)
        //        .HasForeignKey(d => d.PromotionId)
        //        .HasConstraintName("FK_AccountPromotion_Promotion");
        //});

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

        modelBuilder.Entity<MachineAttribute>(entity =>
        {
            entity.HasKey(e => e.MachineAttributeId);

            entity.ToTable("MachineAttribute");

            entity.Property(e => e.MachineAttributeId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Machine).WithMany(p => p.MachineAttributes)
                .HasForeignKey(d => d.MachineId)
                .HasConstraintName("FK_Attribute_Machine");
        });

        modelBuilder.Entity<MachineTerm>(entity =>
        {
            entity.HasKey(e => e.MachineTermId);

            entity.ToTable("MachineTerm");

            entity.Property(e => e.MachineTermId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Machine).WithMany(p => p.MachineTerms)
                .HasForeignKey(d => d.MachineId)
                .HasConstraintName("FK_Term_Machine");
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

        modelBuilder.Entity<MachineComponent>(entity =>
        {
            entity.HasKey(e => e.MachineComponentId);

            entity.ToTable("MachineComponent");

            entity.Property(e => e.MachineComponentId)
                 .ValueGeneratedOnAdd()
                 .UseIdentityColumn();

            entity.HasOne(d => d.Component).WithMany(p => p.MachineComponents)
                .HasForeignKey(d => d.ComponentId)
                .HasConstraintName("FK_MachineComponent_ComponentID");

            entity.HasOne(d => d.Machine).WithMany(p => p.MachineComponents)
              .HasForeignKey(d => d.MachineId)
              .HasConstraintName("FK_MachineComponent_MachineID");
        });

        modelBuilder.Entity<Content>(entity =>
        {
            entity.HasKey(e => e.ContentId);

            entity.ToTable("Content");

            entity.Property(e => e.ContentId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Account)
                .WithMany(p => p.Contents)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Content_Account");
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.ContractId);

            entity.ToTable("Contract");

            entity.HasOne(d => d.AccountSign).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.AccountSignId)
                .HasConstraintName("FK_Contract_Account");

            entity.HasOne(d => d.RentingRequest)
                .WithMany(p => p.Contracts)
                .HasForeignKey(d => d.RentingRequestId)
                .IsRequired() // Contract requires RentingRequestId
                .HasConstraintName("FK_Contract_RentingRequest");

            entity.HasOne(d => d.ContractMachineSerialNumber)
                .WithMany(p => p.Contracts) // Assuming MachineSerialNumber has 'Contracts' collection
                .HasForeignKey(d => d.SerialNumber)
                .HasConstraintName("FK_Contract_MachineSerialNumber")
                .IsRequired();
        });

        modelBuilder.Entity<RentingRequest>(entity =>
        {
            entity.HasKey(e => e.RentingRequestId);

            entity.ToTable("RentingRequest");

            entity.HasOne(d => d.AccountOrder).WithMany(p => p.RentingRequests)
                .HasForeignKey(d => d.AccountOrderId)
                .HasConstraintName("FK_RentingRequest_Account");

            //entity.HasOne(d => d.Address).WithMany(p => p.RentingRequests)
            //    .HasForeignKey(d => d.AddressId)
            //    .HasConstraintName("FK_RentingRequest_Address");
            entity.HasOne(d => d.RentingRequestAddress)
                .WithOne(p => p.RentingRequest)
                .HasForeignKey<RentingRequestAddress>(d => d.RentingRequestId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_RentingRequest_RentingRequestAddress");

            entity.HasMany(d => d.Contracts)
                .WithOne(p => p.RentingRequest)
                .HasForeignKey(p => p.RentingRequestId)
                .OnDelete(DeleteBehavior.Restrict) // Prevent cascading delete
                .HasConstraintName("FK_RentingRequest_Contract");
        });

        modelBuilder.Entity<RentingRequestAddress>(entity =>
        {
            entity.HasKey(e => e.RentingRequestId);

            entity.ToTable("RentingRequestAddress");

            entity.HasOne(d => d.RentingRequest)
                .WithOne(p => p.RentingRequestAddress)
                .HasForeignKey<RentingRequestAddress>(d => d.RentingRequestId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_RentingRequest_RentingRequestAddress");
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

            entity.HasOne(d => d.Invoice)
                .WithMany(p => p.ContractPayments)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK_Invoice_ContractPayment");
        });


        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId);

            // One-to-one relationship with ContractPayment
            entity.HasMany(d => d.ContractPayments)
                .WithOne(p => p.Invoice)
                .HasForeignKey(d => d.InvoiceId)  // Use ContractPaymentId
                .HasConstraintName("FK_Invoice_ContractPayment");

            // One-to-many relationship with AccountPaid
            entity.HasOne(d => d.AccountPaid)
                .WithMany(p => p.Invoices)
                .HasForeignKey(d => d.AccountPaidId)
                .HasConstraintName("FK_Invoices_Account");

            // One-to-one relationship with MaintainTicket
            entity.HasOne(d => d.ComponentReplacementTicket)
                .WithOne(p => p.Invoice)
                .HasForeignKey<Invoice>(d => d.MaintainTicketId)
                .HasConstraintName("FK_Invoice_MaintainTicket");

            entity.HasOne(d => d.DigitalTransaction)
               .WithOne(p => p.Invoice)
               .HasForeignKey<DigitalTransaction>(d => d.InvoiceId)
               .HasConstraintName("FK_Invoice_DigitalTransaction");
        });

        modelBuilder.Entity<DigitalTransaction>(entity =>
        {
            entity.HasKey(e => e.DigitalTransactionId);

            entity.HasOne(d => d.Invoice)
                .WithOne(p => p.DigitalTransaction)
                .HasForeignKey<DigitalTransaction>(d => d.InvoiceId)
                .HasConstraintName("FK_DigitalTransaction_Invoice");
        });


        modelBuilder.Entity<ContractTerm>(entity =>
        {
            entity.HasKey(e => e.ContractTermId);

            entity.ToTable("ContractTerm");

            entity.HasOne(d => d.Contract).WithMany(p => p.ContractTerms)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_ContractTerm_ContractID");
        });

        modelBuilder.Entity<DeliveryTask>(entity =>
        {
            entity.HasKey(e => e.DeliveryTaskId);

            entity.ToTable("DeliveryTask");

            entity.Property(e => e.DeliveryTaskId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Contract).WithMany(p => p.Deliveries)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_DeliveryTask_ContractID");

            entity.HasOne(d => d.Staff).WithMany(p => p.Deliveries)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_DeliveryTask_StaffID");
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



        modelBuilder.Entity<RentingRequestMachineDetail>(entity =>
        {
            entity.HasKey(e => e.RentingRequestMachineDetailId);

            entity.ToTable("RentingRequestMachineDetail");

            entity.Property(e => e.RentingRequestMachineDetailId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.RentingRequest).WithMany(p => p.RentingRequestMachineDetails)
                .HasForeignKey(d => d.RentingRequestId)
                .HasConstraintName("FK_RentingRequestMachineDetail_RentingRequest");

            entity.HasOne(d => d.Machine).WithMany(p => p.RentingRequestMachineDetails)
                .HasForeignKey(d => d.MachineId)
                .HasConstraintName("FK_RentingRequestMachineDetail_Machine");
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

        modelBuilder.Entity<ComponentReplacementTicket>(entity =>
        {
            entity.HasKey(e => e.ComponentReplacementTicketId);

            entity.ToTable("ComponentReplacementTicket");

            entity.HasOne(d => d.Component).WithMany(p => p.ComponentReplacementTickets)
                .HasForeignKey(d => d.ComponentId)
                .HasConstraintName("FK_ComponentReplacementTicket_ComponentID");


            entity.HasOne(d => d.MachineSerialNumber).WithMany(p => p.ComponentReplacementTickets)
                .HasForeignKey(d => d.MachineSerialNumber)
                .HasConstraintName("FK_ComponentReplacementTicket_MachineSerialNumber");

            entity.HasOne(d => d.Contract).WithMany(p => p.ComponentReplacementTickets)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_ComponentReplacementTicket_ContractID");

            entity.HasOne(d => d.Invoice)
                .WithOne(p => p.ComponentReplacementTicket)
                .HasForeignKey<ComponentReplacementTicket>(d => d.InvoiceId)
                .HasConstraintName("FK_Invoice_MaintainTicket");

            entity.HasOne(d => d.MachineTaskCreate).WithMany(p => p.ComponentReplacementTicketsCreateFromTask)
                .HasForeignKey(d => d.MachineTaskCreateId)
                .HasConstraintName("FK_ComponentReplacementTicket_MachineTaskCreated");

        });

        modelBuilder.Entity<ComponentReplacementTicketLog>(entity =>
        {
            entity.HasKey(e => e.ComponentReplacementTicketLogId);

            entity.ToTable("ComponentReplacementTicketLog");

            entity.Property(e => e.ComponentReplacementTicketLogId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.ComponentReplacementTicket).WithMany(p => p.ComponentReplacementTicketLogs)
                .HasForeignKey(d => d.ComponentReplacementTicketId)
                .HasConstraintName("FK_ComponentReplacementTicket_Log");

        });

        modelBuilder.Entity<MachineCheckRequest>(entity =>
        {
            entity.HasKey(e => e.MachineCheckRequestId);

            entity.ToTable("MachineCheckRequest");

            entity.HasOne(d => d.Contract).WithMany(p => p.MachineCheckRequests)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_MachineCheckRequest_Contract");
        });

        modelBuilder.Entity<MachineCheckCriteria>(entity =>
        {
            entity.HasKey(e => e.MachineCheckCriteriaId);

            entity.ToTable("MachineCheckCriteria");

            entity.Property(e => e.MachineCheckCriteriaId)
               .ValueGeneratedOnAdd()
               .UseIdentityColumn();

        });

        modelBuilder.Entity<MachineCheckRequestCriteria>(entity =>
        {
            entity.HasKey(e => e.MachineCheckRequestCriteriaId);

            entity.ToTable("MachineCheckRequestCriteria");

            entity.HasOne(d => d.MachineCheckRequest).WithMany(p => p.MachineCheckRequestCriterias)
                .HasForeignKey(d => d.MachineCheckRequestId)
                .HasConstraintName("FK_MachineCheckRequestCriteria_MachineCheckRequest");

            entity.HasOne(d => d.MachineCheckCriteria).WithMany(p => p.MachineCheckRequestCriterias)
                .HasForeignKey(d => d.MachineCheckCriteriaId)
                .HasConstraintName("FK_MachineCheckRequestCriteria_MachineCheckCriteria");
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

        modelBuilder.Entity<Machine>(entity =>
        {
            entity.HasKey(e => e.MachineId);

            entity.ToTable("Machine");

            entity.Property(e => e.MachineId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Category).WithMany(p => p.Machines)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Machine_Category");
        });

        modelBuilder.Entity<MachineImage>(entity =>
        {
            entity.HasKey(e => e.MachineImageId);

            entity.ToTable("MachineImage");

            entity.Property(e => e.MachineImageId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Machine).WithMany(p => p.MachineImages)
                .HasForeignKey(d => d.MachineId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_MachineImage_Machine");
        });

        modelBuilder.Entity<MachineComponentStatus>(entity =>
        {
            entity.HasKey(e => e.MachineComponentStatusId);

            entity.ToTable("MachineComponentStatus");

            entity.Property(e => e.MachineComponentStatusId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Component).WithMany(p => p.MachineComponentStatuses)
                .HasForeignKey(d => d.ComponentId)
                .HasConstraintName("FK_MachineComponentStatus_ComponentID");

            entity.HasOne(d => d.MachineSerialNumber).WithMany(p => p.MachineComponentStatuses)
                .HasForeignKey(d => d.SerialNumber)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_MachineComponentStatus_MachineSerialNumber");
        });

        modelBuilder.Entity<MachineSerialNumberLog>(entity =>
        {
            entity.HasKey(e => e.MachineSerialNumberLogId);

            entity.ToTable("MachineSerialNumberLog");

            entity.Property(e => e.MachineSerialNumberLogId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.MachineSerialNumber).WithMany(p => p.MachineSerialNumberLogs)
                .HasForeignKey(d => d.SerialNumber)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_MachineSerialNumber_Log");

            entity.HasOne(d => d.AccountTrigger).WithMany(p => p.MachineSerialNumberLogs)
                .HasForeignKey(d => d.AccountTriggerId)
                .HasConstraintName("FK_MachineSerialNumberLog_AccountID");
        });

        modelBuilder.Entity<MachineSerialNumber>(entity =>
        {
            entity.HasKey(e => e.SerialNumber);

            entity.ToTable("MachineSerialNumber");

            entity.HasOne(d => d.Machine).WithMany(p => p.MachineSerialNumbers)
                .HasForeignKey(d => d.MachineId)
                .HasConstraintName("FK_MachineNumber_Machine");
        });

        //modelBuilder.Entity<Promotion>(entity =>
        //{
        //    entity.HasKey(e => e.PromotionId);

        //    entity.ToTable("Promotion");

        //    entity.Property(e => e.PromotionId)
        //        .ValueGeneratedOnAdd()
        //        .UseIdentityColumn();

        //});

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

            entity.HasOne(d => d.MachineTask).WithMany(p => p.Reports)
                .HasForeignKey(d => d.MachineTaskId)
                .HasConstraintName("FK_Report_TaskID");
        });

        modelBuilder.Entity<RequestResponse>(entity =>
        {
            entity.HasKey(e => e.RequestResponseId);

            entity.ToTable("RequestResponse");

            entity.Property(e => e.RequestResponseId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.MachineCheckRequest).WithMany(p => p.RequestResponses)
                .HasForeignKey(d => d.MachineCheckRequestId)
                .HasConstraintName("FK_RequestResponse_MachineCheckRequest");



            entity.HasOne(d => d.MachineTask)
                  .WithOne(t => t.RequestResponse)
                  .HasForeignKey<MachineTask>(t => t.RequestResponseId)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("FK_Response_Task");
        });

        modelBuilder.Entity<MachineTask>(entity =>
        {
            entity.HasKey(e => e.MachineTaskId);

            entity.ToTable("MachineTask");

            entity.Property(e => e.MachineTaskId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Staff).WithMany(p => p.TaskReceivedList)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_Task_Staff");

            entity.HasOne(d => d.Contract).WithMany(p => p.MachineTasks)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_Task_Contract");

            entity.HasOne(d => d.Manager).WithMany(p => p.TaskGaveList)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK_Task_Manager");

            entity.HasOne(d => d.ComponentReplacementTicket).WithMany(p => p.MachineTasks)
               .HasForeignKey(d => d.ComponentReplacementTicketId)
               .HasConstraintName("FK_ComponentReplacementTicketId_Task");

            entity.HasOne(d => d.RequestResponse)
                  .WithOne(t => t.MachineTask)
                  .HasForeignKey<MachineTask>(t => t.RequestResponseId)
                  .HasConstraintName("FK_Task_Response");

            entity.HasOne(d => d.PreviousTask)
                .WithOne()
                .HasForeignKey<MachineTask>(d => d.PreviousTaskId)
                .HasConstraintName("FK_Task_PreviousTask");
        });

        modelBuilder.Entity<MachineTaskLog>(entity =>
        {
            entity.HasKey(e => e.MachineTaskLogId);

            entity.ToTable("MachineTaskLog");

            entity.Property(e => e.MachineTaskLogId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.MachineTask).WithMany(p => p.MachineTaskLogs)
                .HasForeignKey(d => d.MachineTaskId)
                  .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_MachineTaskLog_TaskID");

            entity.HasOne(d => d.AccountTrigger).WithMany(p => p.MachineTaskLogs)
                .HasForeignKey(d => d.AccountTriggerId)
                .HasConstraintName("FK_MachineTaskLog_AccountID");
        });

        modelBuilder.Entity<DeliveryTaskLog>(entity =>
        {
            entity.HasKey(e => e.DeliveryTaskLogId);

            entity.ToTable("DeliveryTaskLog");

            entity.Property(e => e.DeliveryTaskLogId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.DeliveryTask).WithMany(p => p.DeliveryTaskLogs)
                .HasForeignKey(d => d.DeliveryTaskId)
                .HasConstraintName("FK_DeliveryTaskLog_DeliveryTaskID");

            entity.HasOne(d => d.AccountTrigger).WithMany(p => p.DeliveryTaskLogs)
                .HasForeignKey(d => d.AccountTriggerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DeliveryTaskLog_AccountID");
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

        modelBuilder.Entity<Term>(entity =>
        {
            entity.HasKey(e => e.TermId);

            entity.ToTable("Term");

            entity.Property(e => e.TermId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();
        });

        modelBuilder.Entity<MembershipRankLog>(entity =>
        {
            entity.HasKey(e => e.MembershipRankLogId);

            entity.ToTable("MembershipRankLog");

            entity.Property(e => e.MembershipRankLogId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            entity.HasOne(d => d.Account).WithMany(p => p.MembershipRankLogs)
              .HasForeignKey(d => d.AccountId)
              .HasConstraintName("FK_account_membershiplog");

            entity.HasOne(d => d.MembershipRank).WithMany(p => p.MembershipRankLogs)
              .HasForeignKey(d => d.MembershipRankId)
              .HasConstraintName("FK_MembershipRank_membershiplog");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
