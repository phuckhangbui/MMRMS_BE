using Common;
using DTOs.Dashboard;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using System.Globalization;

namespace Service.Implement
{
    public class DashboardServiceImpl : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardServiceImpl(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<List<DataContractManagerDto>> GetDataContractManagerDashboard(string? startMonth, string? endMonth)
        {
            if (!string.IsNullOrEmpty(startMonth) && !string.IsNullOrEmpty(endMonth))
            {
                ValidateDateStringFormat(startMonth);
                ValidateDateStringFormat(endMonth);
            }

            return await _dashboardRepository.GetDataContractManagerDashboard(startMonth, endMonth);
        }

        public async Task<List<DataMachineCheckRequestManagerDto>> GetDataMachineCheckRequestManagerDashboard(string? startMonth, string? endMonth)
        {
            if (!string.IsNullOrEmpty(startMonth) && !string.IsNullOrEmpty(endMonth))
            {
                ValidateDateStringFormat(startMonth);
                ValidateDateStringFormat(endMonth);
            }

            return await _dashboardRepository.GetDataMachineCheckRequestManagerDashboard(startMonth, endMonth);
        }

        public async Task<List<DataMoneyManagerDto>> GetDataMoneyManagerDashboard(string? startMonth, string? endMonth)
        {
            if (!string.IsNullOrEmpty(startMonth) && !string.IsNullOrEmpty(endMonth))
            {
                ValidateDateStringFormat(startMonth);
                ValidateDateStringFormat(endMonth);
            }

            return await _dashboardRepository.GetDataMoneyManagerDashboard(startMonth, endMonth);
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

        private void ValidateDateStringFormat(string date)
        {
            if (!DateTime.TryParseExact(date, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                throw new ServiceException("Format ngày không đúng, xin hãy dùng 'yyyy-MM'.");
            }
        }
    }
}
