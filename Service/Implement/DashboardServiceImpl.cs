using Common;
using DTOs.Dashboard;
using Repository.Interface;
using Service.Exceptions;
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

        public async Task<DataTotalAdminDto> GetDataTotalAdminDashboard(DateTime? startDate, DateTime? endDate)
        {
            return await _dashboardRepository.GetDataTotalAdminDashboard(startDate, endDate);
        }

        public async Task<DataTotalManagerDto> GetDataTotalManagerDashboard(DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                if (startDate > endDate)
                {
                    throw new ServiceException(MessageConstant.DashBoard.FilterParamNotValid);
                }
            }

            return await _dashboardRepository.GetDataTotalManagerDashboard(startDate, endDate);
        }

        public async Task<List<DataUserAdminDto>> GetMonthlyCustomerDataAsync(DateTime? startDate, DateTime? endDate)
        {
            return await _dashboardRepository.GetMonthlyCustomerDataAsync(startDate, endDate);
        }
    }
}
