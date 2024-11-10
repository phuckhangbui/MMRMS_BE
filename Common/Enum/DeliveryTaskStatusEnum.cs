namespace Common.Enum
{
    public enum DeliveryTaskStatusEnum
    {
        [Translation("Mới tạo")]
        Created,

        [Translation("Đang giao")]
        Delivering,

        [Translation("Hoàn thành")]
        Completed,

        [Translation("Thất bại")]
        Fail,

        [Translation("Đã xử lý lại")]
        ProcessedAfterFailure
    }


    public enum DeliveryTaskTypeEnum
    {
        Delivery,
        GetMachinery
    }
}
