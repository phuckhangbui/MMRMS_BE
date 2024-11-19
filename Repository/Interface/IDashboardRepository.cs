using DTOs.Dashboard;

namespace Repository.Interface
{
    public interface IDashboardRepository
    {
        Task<DataTotalAdminDto> GetDataTotalAdminDashboard(DateTime? startDate, DateTime? endDate);
        Task<List<DataUserAdminDto>> GetMonthlyCustomerDataAsync(DateTime? startDate, DateTime? endDate);
        Task<DataTotalManagerDto> GetDataTotalManagerDashboard(DateTime? startDate, DateTime? endDate);
        Task<List<DataContractManagerDto>> GetDataContractManagerDashboard(string? startMonth, string? endMonth);
        Task<List<DataMoneyManagerDto>> GetDataMoneyManagerDashboard(string? startMonth, string? endMonth);
        Task<List<DataMachineCheckRequestManagerDto>> GetDataMachineCheckRequestManagerDashboard(string? startMonth, string? endMonth);
    }
}
