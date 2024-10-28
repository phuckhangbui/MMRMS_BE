﻿using DTOs.Account;

namespace Service.Interface
{
    public interface IAccountService
    {
        Task<IEnumerable<CustomerAccountDto>> GetCustomerAccounts();
        Task<IEnumerable<EmployeeAccountDto>> GetEmployeeAccounts();
        Task<IEnumerable<EmployeeAccountDto>> GetStaffAccounts();
        Task<int> CreateEmployeeAccount(NewEmployeeAccountDto newEmployeeAccountDto);
        Task CreateCustomerAccount(NewCustomerAccountDto newCustomerAccountDto);
        Task<bool> ChangeAccountStatus(int accountId, string status);
        Task<CustomerAccountDetailDto> GetCustomerAccountDetail(int accountId);
        Task<EmployeeAccountDto> GetEmployeeAccountDetail(int accountId);
        Task<IEnumerable<StaffAccountDto>> GetActiveStaffAccounts();
        Task<int> UpdateEmployeeAccount(int accountId, EmployeeAccountUpdateDto employeeAccountUpdateDto);
        Task<int> UpdateCustomerAccount(int accountId, CustomerAccountUpdateDto customerAccountUpdateDto);
    }
}
