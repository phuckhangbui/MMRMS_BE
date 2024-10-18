using DTOs.Account;

namespace Repository.Interface
{
    public interface IAccountRepository
    {
        Task<IEnumerable<AccountBaseDto>> GetAccountsByRole(int? role);
        Task<IEnumerable<CustomerAccountDto>> GetCustomerAccounts();
        Task<IEnumerable<EmployeeAccountDto>> GetEmployeeAccounts();
        Task<IEnumerable<EmployeeAccountDto>> GetStaffAccounts();
        Task<EmployeeAccountDto> CreateEmployeeAccount(NewEmployeeAccountDto newStaffAndManagerAccountDto);
        Task<AccountDto> CreateCustomerAccount(NewCustomerAccountDto newCustomerAccountDto);
        Task<bool> IsAccountExistWithEmail(string email);
        Task ChangeAccountStatus(int acocuntId, string status);
        Task<AccountBaseDto> GetAccountBaseById(int accountId);
        Task<AccountDto> GetAccounById(int accountId);
        Task<IEnumerable<AccountDto>> GetManagerAccounts();
        Task<CustomerAccountDetailDto> GetCustomerAccountById(int accountId);
        Task<EmployeeAccountDto> GetEmployeeAccountById(int accountId);
        Task<bool> IsAccountExistWithUsername(string username);
        Task<AccountDto> GetCustomerAccountWithEmail(string email);
        Task<AccountDto> GetStaffAndManagerAccountWithUsername(string username);
        Task<AccountDto> GetAccountDtoById(int accountId);
        Task UpdateAccount(AccountDto accountDto);
        Task ChangeAccountPassword(AccountDto accountDto, string password);
        Task<AccountDto> FirebaseTokenExisted(string firebaseToken);
        Task<IEnumerable<StaffAccountDto>> GetActiveStaffAccounts();
        Task<bool> IsEmployeeAccountValidToUpdate(int accountId, EmployeeAccountUpdateDto employeeAccountUpdateDto);
        Task<int> UpdateEmployeeAccount(int accountId, EmployeeAccountUpdateDto employeeAccountUpdateDto);
    }
}
