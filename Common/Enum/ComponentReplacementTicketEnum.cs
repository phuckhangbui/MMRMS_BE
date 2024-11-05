namespace Common.Enum
{
    public enum ComponentReplacementTicketStatusEnum
    {
        [Translation("Chưa thanh toán")]
        Unpaid,

        [Translation("Đã thanh toán")]
        Paid,

        [Translation("Đã hoàn thành")]
        Completed,

        [Translation("Đã hủy")]
        Canceled
    }

    public enum ComponentReplacementTicketTypeEnum
    {
        RentingTicket,
        ContractTerminationTicket,
    }
}
