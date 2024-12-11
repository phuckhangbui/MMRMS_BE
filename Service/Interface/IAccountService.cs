using DTOs;
using DTOs.Account;
using DTOs.MachineTask;

namespace Service.Interface
{
    public interface IAccountService
    {
        Task<IEnumerable<CustomerAccountDto>> GetCustomerAccounts();
        Task<IEnumerable<EmployeeAccountDto>> GetEmployeeAccounts();
        Task<IEnumerable<EmployeeAccountDto>> GetStaffAccounts();
        Task<int> CreateEmployeeAccount(NewEmployeeAccountDto newEmployeeAccountDto);
        Task<AccountDto> CreateCustomerAccount(NewCustomerAccountDto newCustomerAccountDto);
        Task<bool> ChangeAccountStatus(int accountId, string status);
        Task<CustomerAccountDetailDto> GetCustomerAccountDetail(int accountId);
        Task<EmployeeAccountDto> GetEmployeeAccountDetail(int accountId);
        Task<IEnumerable<StaffAccountDto>> GetActiveStaffAccounts();
        Task<int> UpdateEmployeeAccount(int accountId, EmployeeAccountUpdateDto employeeAccountUpdateDto);
        Task<int> UpdateEmployeeProfile(int accountId, EmployeeProfileUpdateDto employeeProfileUpdateDto);
        Task<int> UpdateCustomerAccount(int accountId, CustomerAccountUpdateDto customerAccountUpdateDto);
        Task<IEnumerable<TaskAndDeliveryScheduleDto>> GetStaffSchedule(int staffId, DateOnly dateStart, DateOnly dateEnd);
        Task<IEnumerable<TaskAndDeliveryScheduleDto>> GetStaffSchedule(DateOnly dateStart, DateOnly dateEnd);
        Task<IEnumerable<StaffScheduleCounterDto>> GetStaffScheduleFromADate(DateOnly date);
        Task ApproveCustomerAccount(int accountId);
        Task DisapproveCustomerAccount(int accountId, NoteDto note);
    }
}
