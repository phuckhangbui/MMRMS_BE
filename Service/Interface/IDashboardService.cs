using DTOs.Dashboard;

namespace Service.Interface
{
    public interface IDashboardService
    {
        Task<DataTotalAdminDto> GetDataTotalAdminDashboard();
    }
}
