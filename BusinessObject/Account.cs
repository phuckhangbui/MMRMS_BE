namespace BusinessObject;

public partial class Account
{
    public int AccountId { get; set; }

    public string? AvatarImg { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateTime? DateBirth { get; set; }

    public int? Gender { get; set; }

    public string? Username { get; set; }

    public byte[]? PasswordHash { get; set; }

    public byte[]? PasswordSalt { get; set; }

    public string? OtpNumber { get; set; }

    public string? FirebaseMessageToken { get; set; }

    public string? TokenRefresh { get; set; }

    public DateTime? TokenDateExpire { get; set; }

    public int? MembershipRankId { get; set; }

    public int? LogId { get; set; }

    public double? MoneySpent { get; set; }

    public DateTime? DateCreate { get; set; }

    public DateTime? DateExpire { get; set; }

    public int? RoleId { get; set; }

    public string? Status { get; set; }

    public bool? IsDelete { get; set; }

    public virtual MembershipRank? MembershipRank { get; set; }

    public virtual Log? Log { get; set; }

    public virtual ICollection<AccountBusiness> AccountBusinesses { get; set; } = new List<AccountBusiness>();

    public virtual ICollection<AccountPromotion> AccountPromotions { get; set; } = new List<AccountPromotion>();

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<RentingRequest> RentingRequests { get; set; } = new List<RentingRequest>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<EmployeeTask> TaskReceivedList { get; set; } = new List<EmployeeTask>();

    public virtual ICollection<EmployeeTask> TaskGaveList { get; set; } = new List<EmployeeTask>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Contract> CreateContracts { get; set; } = new List<Contract>();

    public virtual ICollection<TaskLog> TaskLogs { get; set; } = new List<TaskLog>();

    public virtual ICollection<DeliveryLog> DeliveryLogs { get; set; } = new List<DeliveryLog>();

}
