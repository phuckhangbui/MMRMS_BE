namespace DTOs.ContractPayment
{
    public class ContractPaymentDto
    {
        public int ContractPaymentId { get; set; }
        public string? ContractId { get; set; }
        public string? InvoiceId { get; set; }
        public string? Title { get; set; }
        public double? Amount { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public DateTime? CustomerPaidDate { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? Period { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? DueDate { get; set; }
        public bool? IsFirstRentalPayment { get; set; }
        //public FirstRentalPaymentDto? FirstRentalPayment { get; set; }
    }

    public class FirstRentalPaymentDto
    {
        public double? TotalServicePrice { get; set; }
        public double? ShippingPrice { get; set; }
        public double? DiscountPrice { get; set; }
    }
}
