namespace DTOs.Delivery
{
    public class DeliveryTaskDetailDto
    {
        public DeliveryTaskDto DeliveryTask { get; set; }

        public IEnumerable<ContractDeliveryDto> ContractDeliveries { get; set; }

        public IEnumerable<DeliveryTaskLogDto> DeliveryTaskLogs { get; set; }
    }
}
