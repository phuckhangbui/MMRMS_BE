using DTOs.Delivery;

namespace Repository.Interface
{
    public interface IDeliveryTaskRepository
    {
        Task CompleteFullyAllDeliveryTask(StaffUpdateDeliveryTaskDto staffUpdateDeliveryTaskDto);
        Task<DeliveryTaskDto> CreateDeliveryTaskToStaff(int managerId, CreateDeliveryTaskDto createDeliveryTaskDto);
        Task FailDeliveryTask(StaffFailDeliveryTaskDto staffFailDeliveryTask);
        Task<IEnumerable<DeliveryTaskDto>> GetDeliveries();
        Task<IEnumerable<DeliveryTaskDto>> GetDeliveriesForStaff(int staffId);
        Task<IEnumerable<DeliveryTaskDto>> GetDeliveriesOfStaffInADay(int staffId, DateTime dateShip);
        Task<DeliveryTaskDto> GetDeliveryTask(int DeliveryTaskId);
        Task<DeliveryTaskDetailDto> GetDeliveryTaskDetail(int deliveryTaskId);
        Task<IEnumerable<DeliveryTaskDto>> GetDeliveryTasksForStaff(int staffId, DateOnly dateStart, DateOnly dateEnd);
        Task<IEnumerable<DeliveryTaskDto>> GetDeliveryTasksInADate(DateOnly date);
        Task MarkDeliveryTaskAsFail(StaffUpdateDeliveryTaskDto staffUpdateDeliveryTaskDto);
        Task UpdateDeliveryTaskStatus(int DeliveryTaskId, string status, int accountId);
    }
}
