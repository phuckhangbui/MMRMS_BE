using DTOs.Dashboard;

namespace Repository.Interface
{
    public interface IDashboardRepository
    {
        Task<DataTotalAdminDto> GetDataTotalAdminDashboard();
    }
}
