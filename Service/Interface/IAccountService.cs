using DTOs.Account;

namespace Service.Interface
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountBaseDto>> GetAccountsByRole(int? role);
        Task<IEnumerable<CustomerAccountDto>> GetCustomerAccounts();
        Task<IEnumerable<EmployeeAccountDto>> GetEmployeeAccounts();
        Task<IEnumerable<EmployeeAccountDto>> GetStaffAccounts();
        Task<int> CreateEmployeeAccount(NewEmployeeAccountDto newEmployeeAccountDto);
        Task CreateCustomerAccount(NewCustomerAccountDto newCustomerAccountDto);
        Task ChangeAccountStatus(int accountId, string status);
        Task<CustomerAccountDto> GetCustomerAccountById(int accountId);
        Task<EmployeeAccountDto> GetEmployeeAccountById(int accountId);
        Task<AccountDto> GetAccount(int accountId);
    }
}
