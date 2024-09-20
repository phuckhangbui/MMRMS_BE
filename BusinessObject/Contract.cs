﻿namespace BusinessObject;

public partial class Contract
{
    public string ContractId { get; set; } = null!;

    public string? ContractName { get; set; }

    public int? AccountSignId { get; set; }

    public int? AddressId { get; set; }

    public string? HiringRequestId { get; set; }

    public double? TotalRentPrice { get; set; }

    public double? ShippingPrice { get; set; }

    public double? TotalDepositPrice { get; set; }

    public double? DiscountPrice { get; set; }

    public double? FinalAmount { get; set; }

    public string? Content { get; set; }

    public DateTime? DateCreate { get; set; }

    public DateTime? DateSign { get; set; }

    public DateTime? DateStart { get; set; }

    public DateTime? DateEnd { get; set; }

    public string? Status { get; set; }

    public virtual Account? AccountSign { get; set; }

    public virtual Address? Address { get; set; }

    public virtual HiringRequest? HiringRequest { get; set; }

    public virtual ICollection<ContractPayment> ContractPayments { get; set; } = new List<ContractPayment>();

    public virtual ICollection<ContractTerm> ContractTerms { get; set; } = new List<ContractTerm>();

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();

    public virtual ICollection<ContractSerialNumberProduct> ContractSerialNumberProducts { get; set; } = new List<ContractSerialNumberProduct>();

    public virtual ICollection<EmployeeTask> EmployeeTasks { get; set; } = new List<EmployeeTask>();
}
