using DTOs.Account;

namespace Repository.Interface
{
    public interface IAccountRepository
    {
        Task<IEnumerable<AccountBaseDto>> GetAccountsByRole(int? role);
        Task<IEnumerable<CustomerAccountDto>> GetCustomerAccounts();
        Task<IEnumerable<StaffAndManagerAccountDto>> GetManagerAndStaffAccountsByRole();
        Task CreateStaffOrManagerAccount(NewStaffAndManagerAccountDto newStaffAndManagerAccountDto);
        Task CreateCustomerAccount(NewCustomerAccountDto newCustomerAccountDto);
        Task<bool> IsAccountExistWithEmail(string email);
        Task ChangeAccountStatus(int acocuntId, string status);
        Task<AccountBaseDto> GetAccountById(int accountId);
        Task<CustomerAccountDto> GetCustomerAccountById(int accountId);
        Task<StaffAndManagerAccountDto> GetStaffAndManagerAccountById(int accountId);
        Task<bool> IsAccountExistWithUsername(string username);
        Task<AccountDto> GetCustomerAccountWithEmail(string email);
        Task<AccountDto> GetStaffAndManagerAccountWithUsername(string username);
        Task<AccountDto> GetAccountDtoById(int accountId);

        Task UpdateAccount(AccountDto accountDto);
        Task ChangeAccountPassword(AccountDto accountDto, string password);
    }
}
