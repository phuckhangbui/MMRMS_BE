namespace BusinessObject;

public partial class Account
{
    public int AccountId { get; set; }

    public string? AvatarImg { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? CitizenCard { get; set; }

    public string? Phone { get; set; }

    public DateTime? DateBirth { get; set; }

    public int? Gender { get; set; }

    public string? Address { get; set; }

    public string? Username { get; set; }

    public byte[]? PasswordHash { get; set; }

    public byte[]? PasswordSalt { get; set; }

    public string? OtpNumber { get; set; }

    public string? TokenRefresh { get; set; }

    public DateTime? TokenDateExpire { get; set; }

    public int? ActionPromotion { get; set; }

    public int? PromotionId { get; set; }

    public DateTime? DateCreate { get; set; }

    public DateTime? DateExpire { get; set; }

    public int? RoleId { get; set; }

    public int? BusinessType { get; set; }

    public int? Status { get; set; }

    public bool? IsDelete { get; set; }

    public virtual ICollection<AccountBusiness> AccountBusinesses { get; set; } = new List<AccountBusiness>();

    public virtual ICollection<AccountPromotion> AccountPromotions { get; set; } = new List<AccountPromotion>();

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Content> Contents { get; set; } = new List<Content>();

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<HiringRequest> HiringRequests { get; set; } = new List<HiringRequest>();

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual Promotion? Promotion { get; set; }

    public virtual ICollection<EmployeeTask> TaskAssignees { get; set; } = new List<EmployeeTask>();

    public virtual ICollection<EmployeeTask> TaskReporters { get; set; } = new List<EmployeeTask>();
}
