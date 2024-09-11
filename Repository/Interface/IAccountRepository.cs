using DAO.Enum;
using DTOs.Account;

namespace Repository.Interface
{
    public interface IAccountRepository
    {
        Task<IEnumerable<AccountBaseDto>> GetAccountsByRole(int? role);
        Task<IEnumerable<StaffAndManagerAccountDto>> GetManagerAndStaffAccountsByRole();
        Task CreateAccount(NewCustomerAccountDto? newCustomerAccountDto, NewStaffAndManagerAccountDto? newStaffAndManagerAccountDto);
        Task<bool> IsAccountExistWithEmail(string email);
        Task ChangeAccountStatus(int acocuntId, int status);
        Task<AccountBaseDto> GetAccountById(int accountId);
        Task<CustomerAccountDto> GetCustomerAccountById(int accountId);
        Task<StaffAndManagerAccountDto> GetStaffAndManagerAccountById(int accountId);
        Task<bool> IsAccountExistWithUsername(string username);
    }
}
