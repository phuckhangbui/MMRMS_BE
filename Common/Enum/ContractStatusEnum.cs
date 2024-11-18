namespace Common.Enum
{
    public enum ContractStatusEnum
    {
        NotSigned,
        Signed,
        Shipping,
        ShipFail,
        Renting,
        InspectionPending,
        InspectionInProgress,
        AwaitingShippingAfterCheck,
        AwaitingRefundInvoice,
        Completed,
        Canceled
    }
}
