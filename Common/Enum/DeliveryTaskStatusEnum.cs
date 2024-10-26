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
        Fail
    }

    public enum DeliveryTaskTypeEnum
    {
        Delivery,
        GetMachinery
    }
}
