﻿namespace Common.Enum
{
    public enum MachineCheckRequestStatusEnum
    {
        [Translation("Mới tạo")]
        New,

        [Translation("Đã giao nhiệm vụ")]
        Assigned,

        [Translation("Đang sử lý")]
        Processing,

        [Translation("Hoàn thành")]
        Completed,

        [Translation("Đã hủy")]
        Canceled,
    }
}
