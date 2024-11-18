using DTOs.Dashboard;

namespace Service.Interface
{
    public interface IDashboardService
    {
        Task<DataTotalAdminDto> GetDataTotalAdminDashboard(DateTime? startDate, DateTime? endDate);
        Task<List<DataUserAdminDto>> GetMonthlyCustomerDataAsync(DateTime? startDate, DateTime? endDate);
        Task<DataTotalManagerDto> GetDataTotalManagerDashboard(DateTime? startDate, DateTime? endDate);
        Task<DataContractManagerDto> GetDataContractManagerDashboard(DateTime? startDate, DateTime? endDate);
        Task<DataMoneyManagerDto> GetDataMoneyManagerDashboard(DateTime? startDate, DateTime? endDate);
        Task<DataMachineCheckRequestManagerDto> GetDataMachineCheckRequestManagerDashboard(DateTime? startDate, DateTime? endDate);
    }
}
