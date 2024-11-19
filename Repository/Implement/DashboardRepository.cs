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
        public async Task<List<DataContractManagerDto>> GetDataContractManagerDashboard(string? startMonth, string? endMonth)
        {
            DateTime startDate = string.IsNullOrEmpty(startMonth)
                ? new DateTime(DateTime.Now.Year, 1, 1)
                : DateTime.ParseExact(startMonth, "yyyy-MM", null);

            DateTime endDate = string.IsNullOrEmpty(endMonth)
                ? new DateTime(DateTime.Now.Year, 12, 31)
                : DateTime.ParseExact(endMonth, "yyyy-MM", null).AddMonths(1).AddDays(-1);

            var monthlyData = new List<DataContractManagerDto>();
            for (var date = startDate; date <= endDate; date = date.AddMonths(1))
            {
                var monthStart = new DateTime(date.Year, date.Month, 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);

                var totalContracts = await ContractDao.Instance.GetContractsInRangeAsync(monthStart, monthEnd);

                monthlyData.Add(new DataContractManagerDto
                {
                    Time = monthStart.ToString("MMMM"),
                    Contract = totalContracts
                });
            }

            return monthlyData;
        }

        public async Task<List<DataMachineCheckRequestManagerDto>> GetDataMachineCheckRequestManagerDashboard(string? startMonth, string? endMonth)
        {
            DateTime startDate = string.IsNullOrEmpty(startMonth)
                ? new DateTime(DateTime.Now.Year, 1, 1)
                : DateTime.ParseExact(startMonth, "yyyy-MM", null);

            DateTime endDate = string.IsNullOrEmpty(endMonth)
                ? new DateTime(DateTime.Now.Year, 12, 31)
                : DateTime.ParseExact(endMonth, "yyyy-MM", null).AddMonths(1).AddDays(-1);

            var monthlyData = new List<DataMachineCheckRequestManagerDto>();

            for (var date = startDate; date <= endDate; date = date.AddMonths(1))
            {
                var monthStart = new DateTime(date.Year, date.Month, 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);

                var totalRequest = await MachineCheckRequestDao.Instance.GetMachineCheckRequestsInRangeAsync(monthStart, monthEnd);

                monthlyData.Add(new DataMachineCheckRequestManagerDto
                {
                    Time = monthStart.ToString("MMMM"),
                    MachineCheckRequest = totalRequest
                });
            }

            return monthlyData;
        }

        public async Task<List<DataMoneyManagerDto>> GetDataMoneyManagerDashboard(string? startMonth, string? endMonth)
        {
            DateTime startDate = string.IsNullOrEmpty(startMonth)
                ? new DateTime(DateTime.Now.Year, 1, 1)
                : DateTime.ParseExact(startMonth, "yyyy-MM", null);

            DateTime endDate = string.IsNullOrEmpty(endMonth)
                ? new DateTime(DateTime.Now.Year, 12, 31)
                : DateTime.ParseExact(endMonth, "yyyy-MM", null).AddMonths(1).AddDays(-1);

            var monthlyData = new List<DataMoneyManagerDto>();

            for (var date = startDate; date <= endDate; date = date.AddMonths(1))
            {
                var monthStart = new DateTime(date.Year, date.Month, 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);

                var totalMoney = await InvoiceDao.Instance.GetTotalMoneyInRangeAsync(monthStart, monthEnd);

                monthlyData.Add(new DataMoneyManagerDto
                {
                    Time = monthStart.ToString("MMMM"),
                    Money = totalMoney
                });
            }

            return monthlyData;
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
