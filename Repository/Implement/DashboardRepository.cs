using DAO;
using DAO.Enum;
using DTOs.Dashboard;
using Repository.Interface;

namespace Repository.Implement
{
    public class DashboardRepository : IDashboardRepository
    {
        public async Task<DataTotalAdminDto> GetDataTotalAdminDashboard()
        {
            var totalCustomers = await AccountDao.Instance.GetAccountsByRoleAsync((int)AccountRoleEnum.Customer);
            var totalStaffs = await AccountDao.Instance.GetManagerAndStaffAccountsAsync();
            var totalContents = await ContentDao.Instance.GetAllAsync();

            var dataTotal = new DataTotalAdminDto()
            {
                TotalCustomer = totalCustomers.Count(),
                TotalStaff = totalStaffs.Count(),
                TotalContent = totalContents.Count()
            };

            return dataTotal;
        }
    }
}
