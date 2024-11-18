using BusinessObject;
using Common.Enum;
using DAO;
using DTOs.Dashboard;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System.Globalization;

namespace Repository.Implement
{
    public class DashboardRepository : IDashboardRepository
    {
        public async Task<DataContractManagerDto> GetDataContractManagerDashboard(DateTime? startDate, DateTime? endDate)
        {
            startDate ??= new DateTime(DateTime.Now.Year, 1, 1);
            endDate ??= new DateTime(DateTime.Now.Year, 12, 31);

            var totalContracts = await ContractDao.Instance.GetContractsInRangeAsync(startDate, endDate);

            return new DataContractManagerDto { TotalContract = totalContracts };
        }

        public async Task<DataMachineCheckRequestManagerDto> GetDataMachineCheckRequestManagerDashboard(DateTime? startDate, DateTime? endDate)
        {
            startDate ??= new DateTime(DateTime.Now.Year, 1, 1);
            endDate ??= new DateTime(DateTime.Now.Year, 12, 31);

            var totalMachineCheckRequest = await MachineCheckRequestDao.Instance.GetMachineCheckRequestsInRangeAsync(startDate, endDate);

            return new DataMachineCheckRequestManagerDto { TotalMachineCheckRequest = totalMachineCheckRequest };
        }

        public async Task<DataMoneyManagerDto> GetDataMoneyManagerDashboard(DateTime? startDate, DateTime? endDate)
        {
            startDate ??= new DateTime(DateTime.Now.Year, 1, 1);
            endDate ??= new DateTime(DateTime.Now.Year, 12, 31);

            var totalMoney = await InvoiceDao.Instance.GetTotalMoneyInRangeAsync(startDate, endDate);

            return new DataMoneyManagerDto { TotalMoney = totalMoney };
        }

        public async Task<DataTotalAdminDto> GetDataTotalAdminDashboard(DateTime? startDate, DateTime? endDate)
        {
            IEnumerable<Account> totalCustomers;
            IEnumerable<Account> totalStaffs;
            IEnumerable<Content> totalContents;

            if (startDate.HasValue && endDate.HasValue)
            {
                totalCustomers = await AccountDao.Instance.GetAccountsByRoleInRangeAsync((int)AccountRoleEnum.Customer, startDate, endDate);
                totalStaffs = await AccountDao.Instance.GetEmployeeAccountsInRangeAsync(startDate, endDate);
                totalContents = await ContentDao.Instance.GetAllInRangeAsync(startDate, endDate);
            }
            else
            {
                totalCustomers = await AccountDao.Instance.GetAccountsByRoleAsync((int)AccountRoleEnum.Customer);
                totalStaffs = await AccountDao.Instance.GetEmployeeAccountsAsync();
                totalContents = await ContentDao.Instance.GetAllAsync();
            }

            return new DataTotalAdminDto
            {
                TotalCustomer = totalCustomers.Count(),
                TotalStaff = totalStaffs.Count(),
                TotalContent = totalContents.Count()
            };
        }

        public async Task<DataTotalManagerDto> GetDataTotalManagerDashboard(DateTime? startDate, DateTime? endDate)
        {
            var totalMachines = await MachineDao.Instance.GetMachinesInRangeAsync(startDate, endDate);
            var totalRentingRequests = await RentingRequestDao.Instance.GetRentingRequestsInRangeAsync(startDate, endDate);
            var totalContracts = await ContractDao.Instance.GetContractsInRangeAsync(startDate, endDate);
            var totalMoney = await InvoiceDao.Instance.GetTotalMoneyInRangeAsync(startDate, endDate);

            return new DataTotalManagerDto
            {
                TotalMachine = totalMachines,
                TotalRentingRequest = totalRentingRequests,
                TotalContract = totalContracts,
                TotalMoney = totalMoney
            };
        }

        public async Task<List<DataUserAdminDto>> GetMonthlyCustomerDataAsync(DateTime? startDate, DateTime? endDate)
        {
            using (var context = new MmrmsContext())
            {
                var query = context.Accounts
                    .Where(a => a.RoleId == (int)AccountRoleEnum.Customer);

                if (startDate.HasValue)
                {
                    query = query.Where(a => a.DateCreate >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    query = query.Where(a => a.DateCreate <= endDate.Value);
                }

                var monthlyData = await query
                    .GroupBy(a => new { a.DateCreate!.Value.Year, a.DateCreate.Value.Month })
                    .Select(g => new DataUserAdminDto
                    {
                        Time = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month) + " / " + g.Key.Year,
                        Customer = g.Count()
                    })
                    .ToListAsync();

                return monthlyData;
            }
        }
    }
}
