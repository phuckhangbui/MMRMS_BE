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

    public int? AccountBusinessId { get; set; }

    public int? LogId { get; set; }

    public double? MoneySpent { get; set; }

    public DateTime? DateCreate { get; set; }

    public DateTime? DateExpire { get; set; }

    public int? RoleId { get; set; }

    public string? Status { get; set; }

    public bool? IsDelete { get; set; }

    public virtual MembershipRank? MembershipRank { get; set; }

    public virtual AccountBusiness? AccountBusiness { get; set; }

    public virtual Role? Role { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<LogDetail> LogDetails { get; set; } = new List<LogDetail>();

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<DeliveryTask> Deliveries { get; set; } = new List<DeliveryTask>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<RentingRequest> RentingRequests { get; set; } = new List<RentingRequest>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<MachineTask> TaskReceivedList { get; set; } = new List<MachineTask>();

    public virtual ICollection<MachineTask> TaskGaveList { get; set; } = new List<MachineTask>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<MachineTaskLog> MachineTaskLogs { get; set; } = new List<MachineTaskLog>();

    public virtual ICollection<DeliveryTaskLog> DeliveryTaskLogs { get; set; } = new List<DeliveryTaskLog>();

    public virtual ICollection<MachineSerialNumberLog> MachineSerialNumberLogs { get; set; } = new List<MachineSerialNumberLog>();

    public virtual ICollection<MembershipRankLog> MembershipRankLogs { get; set; } = new List<MembershipRankLog>();

    public virtual ICollection<Content> Contents { get; set; } = new List<Content>();
}
