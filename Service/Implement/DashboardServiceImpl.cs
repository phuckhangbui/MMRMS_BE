using DTOs.Dashboard;
using Repository.Interface;
using Service.Interface;

namespace Service.Implement
{
    public class DashboardServiceImpl : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardServiceImpl(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<DataTotalAdminDto> GetDataTotalAdminDashboard()
        {
            return await _dashboardRepository.GetDataTotalAdminDashboard();
        }
    }
}
