using DTOs.Account;

namespace Service.Interface
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountBaseDto>> GetAccountsByRole(int? role);
        Task<IEnumerable<CustomerAccountDto>> GetCustomerAccounts();
        Task<IEnumerable<EmployeeAccountDto>> GetManagerAndStaffAccountsByRole();
        Task<int> CreateEmployeeAccount(NewStaffAndManagerAccountDto newStaffAndManagerAccountDto);
        Task CreateCustomerAccount(NewCustomerAccountDto newCustomerAccountDto);
        Task ChangeAccountStatus(int accountId, string status);
        Task<CustomerAccountDto> GetCustomerAccountById(int accountId);
        Task<EmployeeAccountDto> GetStaffAndManagerAccountById(int accountId);
    }
}
