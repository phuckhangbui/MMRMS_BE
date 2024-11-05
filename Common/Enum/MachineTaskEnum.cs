namespace Common.Enum
{
    public enum MachineTaskEnum
    {
        [Translation("Mới tạo")]
        Created,

        [Translation("Hoàn thành")]
        Completed,

        [Translation("Đang sửa")]
        Reparing,

        [Translation("Đã hủy")]
        Canceled
    }

    public enum MachineTaskTypeEnum
    {
        MachineryCheckRequest,
        ContractTerminationCheck
    }
}
