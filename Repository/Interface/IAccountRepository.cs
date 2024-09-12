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
        Task ChangeAccountStatus(int acocuntId, int status);
        Task<AccountBaseDto> GetAccountById(int accountId);
        Task<CustomerAccountDto> GetCustomerAccountById(int accountId);
        Task<StaffAndManagerAccountDto> GetStaffAndManagerAccountById(int accountId);
        Task<bool> IsAccountExistWithUsername(string username);
    }
}
