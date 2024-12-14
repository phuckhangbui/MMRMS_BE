namespace Common
{
    public static class GlobalConstant
    {
        public const string DefaultPassword = "12345";
        public const string DefaultAvatarUrl = "https://res.cloudinary.com/dfdwupiah/image/upload/v1728103743/MMRMS/qbjsfiyperlviohymsmv.png";
        public const string ContractIdPrefixPattern = "CON";
        public const string RentingRequestIdPrefixPattern = "REH";
        public const string InvoiceIdPrefixPattern = "INV";
        public const string TaskIdPrefixPattern = "TK";
        public const string DateTimeFormatPattern = "yyyyMMdd";
        public const string MachineCheckRequestIdPrefixPattern = "REQ";
        public const string ComponentReplacementTicketIdPrefixPattern = "CRT";
        public const string DateOnlyFormat = "dd/MM/yyyy";
        public const double DepositValue = 0.3;
        public const int MaxTaskLimitADay = 3;
        public const string ContractName = "Hợp đồng thuê máy ";
        public const string DepositContractPaymentTitle = "Thanh toán tiền đặt cọc cho hợp đồng ";
        public const string RentalContractPaymentTitle = "Thanh toán tiền thuê cho hợp đồng ";
        public const string RefundContractPaymentTitle = "Hoàn trả tiền đặt cọc cho hợp đồng ";
        public const string DamagePenaltyContractPaymentTitle = "Bồi thường tiền cho hợp đồng ";
        public const string ExtendContractPaymentTitle = "Thanh toán tiền thuê gia hạn cho hợp đồng ";
        public const string FineContractPaymentTitle = "Tiền phạt do đóng tiền thuê trễ hạn cho hợp đồng ";
        public const string RefundInvoiceNote = "Hóa đơn hoàn trả tiền đặt cọc cho hợp đồng ";
        public const string DamagePenaltyInvoiceNote = "Hóa đơn tiền bồi thường cho hợp đồng ";
        public const string SequenceSeparator = "NO";
        public const string MembershipRankLogPaymentMadeAction = "Tổng số tiền đã thanh toán là ";
        public const string MembershipRankLogRankUpgradedAction = "Hạng thành viên đã được nâng cấp lên ";
        public const int MinimumRentPeriodInDay = 90;
        public const int MaximumRentPeriodInDay = 365;
        public const int MaxStartDateOffsetInDays = 30;
        public const int DueDateContractPayment = 15;
        public const double FineValue = 500000;
        public const int ExpectedAvailabilityOffsetDays = 5;
    }
}
