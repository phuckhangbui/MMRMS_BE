using DTOs.Dashboard;

namespace Repository.Interface
{
    public interface IDashboardRepository
    {
        Task<DataTotalAdminDto> GetDataTotalAdminDashboard(DateTime? startDate, DateTime? endDate);
        Task<List<DataUserAdminDto>> GetMonthlyCustomerDataAsync(DateTime? startDate, DateTime? endDate);
        Task<DataTotalManagerDto> GetDataTotalManagerDashboard(DateTime? startDate, DateTime? endDate);
    }
}
