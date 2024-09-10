using DTOs.Account;

namespace Service.Interface
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountBaseDto>> GetAccountsByRole(int? role);
        Task<IEnumerable<StaffAndManagerAccountDto>> GetManagerAndStaffAccountsByRole();
        Task CreateAccount(NewCustomerAccountDto? newCustomerAccountDto, NewStaffAndManagerAccountDto? newStaffAndManagerAccountDto);
        Task ChangeAccountStatus(int accountId, int status);
    }
}
