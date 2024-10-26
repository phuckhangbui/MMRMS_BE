﻿using DTOs.DeliveryTask;

namespace Repository.Interface
{
    public interface IDeliveryTaskRepository
    {
        Task<DeliveryTaskDto> CreateDeliveryTaskToStaff(int managerId, CreateDeliveryTaskDto createDeliveryTaskDto);
        Task<IEnumerable<DeliveryTaskDto>> GetDeliveries();
        Task<IEnumerable<DeliveryTaskDto>> GetDeliveriesForStaff(int staffId);
        Task<IEnumerable<DeliveryTaskDto>> GetDeliveriesOfStaffInADay(int staffId, DateTime dateShip);
        Task<DeliveryTaskDto> GetDeliveryTask(int DeliveryTaskId);
        Task UpdateDeliveryTaskStatus(int DeliveryTaskId, string status, int accountId);
    }
}
