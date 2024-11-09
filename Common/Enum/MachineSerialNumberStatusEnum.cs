namespace Common.Enum
{
    public enum MachineSerialNumberStatusEnum
    {
        [Translation("Sẵn sàng")]
        Available,

        [Translation("Đang thuê")]
        Renting,

        [Translation("Đang sửa chữa")]
        Maintenance,

        [Translation("Đã khóa")]
        Locked,

        [Translation("Đã giữ chỗ")]
        Reserved,
    }
}
