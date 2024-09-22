using DTOs.Dashboard;

namespace Service.Interface
{
    public interface IDashboardService
    {
        Task<DataTotalAdminDto> GetDataTotalAdminDashboard(DateTime? startDate, DateTime? endDate);
        Task<List<DataUserAdminDto>> GetMonthlyCustomerDataAsync(DateTime? startDate, DateTime? endDate);
    }
}
