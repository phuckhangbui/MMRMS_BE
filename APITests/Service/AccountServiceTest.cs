using AutoMapper;
using Common;
using Common.Enum;
using DTOs.Account;
using Microsoft.Extensions.Configuration;
using Moq;
using Repository.Interface;
using Repository.Mapper;
using Service.Exceptions;
using Service.Implement;
using Service.Interface;
using Xunit;
using Assert = Xunit.Assert;

namespace APITests.Service
{
    public class AccountServiceTest
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<IMailService> _mailMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountServiceTest()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _mailMock = new Mock<IMailService>();
            _configurationMock = new Mock<IConfiguration>();

            var mockConfigurationSection = new Mock<IConfigurationSection>();
            mockConfigurationSection.Setup(x => x.Value).Returns("admin_mmrms");

            _configurationMock.Setup(x => x.GetSection("AdminAccount:Username")).Returns(mockConfigurationSection.Object);

            _accountService = new AccountServiceImpl(_configurationMock.Object, _accountRepositoryMock.Object, _mailMock.Object);

            _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<AutoMapperProfile>();
            }));
        }

        [Fact]
        public async void GetEmployeeAccounts_ReturnSuccessfully()
        {
            var employeeAccounts = GetSampleEmployeeAccounts();
            _accountRepositoryMock.Setup(x => x.GetEmployeeAccounts()).ReturnsAsync(employeeAccounts);

            var result = await _accountService.GetEmployeeAccounts();

            Assert.Equal(4, result.Count());
        }

        [Fact]
        public async void GetEmployeeAccountById_ReturnSuccessfully()
        {
            int accountId = 4;
            var accountBase = GetSampleAccountBaseDto(accountId, (int)AccountRoleEnum.TechnicalStaff);
            var employeeAccount = GetSampleEmployeeAccounts().FirstOrDefault(x => x.AccountId == accountId);

            _accountRepositoryMock.Setup(x => x.GetAccountBaseById(accountId)).ReturnsAsync(accountBase);
            _accountRepositoryMock.Setup(x => x.GetEmployeeAccountById(accountId)).ReturnsAsync(employeeAccount);

            var result = await _accountService.GetEmployeeAccountDetail(accountId);

            Assert.NotNull(result);
            Assert.Equal(employeeAccount.Username, result.Username);
        }

        [Fact]
        public async void GetEmployeeAccountById_ThrowsException_AccountNotFound()
        {
            int accountId = -100;
            _accountRepositoryMock.Setup(x => x.GetAccountBaseById(accountId)).ReturnsAsync((AccountBaseDto)null);

            var exception = await Assert.ThrowsAsync<ServiceException>(() => _accountService.GetEmployeeAccountDetail(accountId));
            Assert.Equal(MessageConstant.Account.AccountNotFound, exception.Message);
        }

        [Fact]
        public async Task CreateEmployeeAccount_ReturnSuccessfully()
        {
            // Arrange
            var newEmployee = new NewEmployeeAccountDto
            {
                Name = "John Doe",
                Email = "johndoe@example.com",
                Phone = "0123456789",
                Gender = 1,
                DateBirth = new DateTime(1990, 5, 1),
                Username = "johndoe",
                DateExpire = DateTime.Now.AddYears(1),
                RoleId = 2
            };

            var createdEmployee = new EmployeeAccountDto
            {
                Username = newEmployee.Username,
                AccountId = 1,
                Name = newEmployee.Name,
                Email = newEmployee.Email,
                Phone = newEmployee.Phone,
                DateCreate = DateTime.Now.ToString(),
                Status = AccountStatusEnum.Active.ToString(),
                RoleId = newEmployee.RoleId,
                Gender = newEmployee.Gender,
                DateExpire = newEmployee.DateExpire.ToString(),
                DateBirth = newEmployee.DateBirth.ToString(),
                AvatarImg = "https://res.cloudinary.com/dfdwupiah/image/upload/v1728103743/MMRMS/qbjsfiyperlviohymsmv.png"
            };

            _accountRepositoryMock.Setup(x => x.IsAccountExistWithEmail(newEmployee.Email)).ReturnsAsync(false);
            _accountRepositoryMock.Setup(x => x.IsAccountExistWithUsername(newEmployee.Username)).ReturnsAsync(false);
            _accountRepositoryMock.Setup(x => x.CreateEmployeeAccount(newEmployee)).ReturnsAsync(createdEmployee);

            var result = await _accountService.CreateEmployeeAccount(newEmployee);

            Assert.Equal(createdEmployee.AccountId, result);
            _accountRepositoryMock.Verify(x => x.CreateEmployeeAccount(It.IsAny<NewEmployeeAccountDto>()), Times.Once);
            //_mailMock.Verify(x => x.SendMail(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CreateEmployeeAccount_ThrowsException_EmailAlreadyExists()
        {
            // Arrange
            var newEmployee = new NewEmployeeAccountDto
            {
                Name = "John Doe",
                Email = "johndoe@example.com",
                Phone = "0123456789",
                Gender = 1,
                DateBirth = new DateTime(1990, 5, 1),
                Username = "johndoe",
                DateExpire = DateTime.Now.AddYears(1),
                RoleId = 2
            };

            _accountRepositoryMock.Setup(x => x.IsAccountExistWithEmail(newEmployee.Email)).ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<ServiceException>(() => _accountService.CreateEmployeeAccount(newEmployee));
            Assert.Equal(MessageConstant.Account.EmailAlreadyExists, exception.Message);
        }

        [Fact]
        public async Task CreateEmployeeAccount_ThrowsException_UsernameAlreadyExists()
        {
            // Arrange
            var newEmployee = new NewEmployeeAccountDto
            {
                Name = "John Doe",
                Email = "johndoe@example.com",
                Phone = "0123456789",
                Gender = 1,
                DateBirth = new DateTime(1990, 5, 1),
                Username = "johndoe",
                DateExpire = DateTime.Now.AddYears(1),
                RoleId = 2
            };

            _accountRepositoryMock.Setup(x => x.IsAccountExistWithEmail(newEmployee.Email)).ReturnsAsync(false);
            _accountRepositoryMock.Setup(x => x.IsAccountExistWithUsername(newEmployee.Username)).ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<ServiceException>(() => _accountService.CreateEmployeeAccount(newEmployee));
            Assert.Equal(MessageConstant.Account.UsernameAlreadyExists, exception.Message);
        }

        [Fact]
        public async Task ChangeAccountStatus_ReturnSuccessfully()
        {
            string validStatus = AccountStatusEnum.Active.ToString();
            int accountId = 4;
            var accountBase = GetSampleAccountBaseDto(accountId, (int)AccountRoleEnum.TechnicalStaff);

            _accountRepositoryMock.Setup(x => x.GetAccountBaseById(accountId)).ReturnsAsync(accountBase);
            _accountRepositoryMock.Setup(x => x.ChangeAccountStatus(accountId, validStatus)).ReturnsAsync(true);

            var result = await _accountService.ChangeAccountStatus(accountId, validStatus);

            Assert.True(result);
        }

        [Fact]
        public async void ChangeAccountStatus_ThrowsException_AccountNotFound()
        {
            string validStatus = AccountStatusEnum.Active.ToString();
            int accountId = 100;
            _accountRepositoryMock.Setup(x => x.GetAccountBaseById(accountId)).ReturnsAsync((AccountBaseDto)null);

            var exception = await Assert.ThrowsAsync<ServiceException>(() => _accountService.ChangeAccountStatus(accountId, validStatus));
            Assert.Equal(MessageConstant.Account.AccountNotFound, exception.Message);
        }

        [Fact]
        public async void ChangeAccountStatus_ThrowsException_InvalidStatusValue()
        {
            string invalidStatus = "InvalidStatus";
            int accountId = 4;
            var accountBase = GetSampleAccountBaseDto(accountId, (int)AccountRoleEnum.TechnicalStaff);

            _accountRepositoryMock.Setup(x => x.GetAccountBaseById(accountId)).ReturnsAsync(accountBase);

            var exception = await Assert.ThrowsAsync<ServiceException>(() => _accountService.ChangeAccountStatus(accountId, invalidStatus));
            Assert.Equal(MessageConstant.InvalidStatusValue, exception.Message);
        }


        public static AccountBaseDto GetSampleAccountBaseDto(int accountId, int roleId)
        {
            return new AccountBaseDto
            {
                AccountId = accountId,
                Name = "khang staff",
                Email = "khangstaff592@gmail.com",
                Phone = "0987876575",
                DateCreate = "10/01/2024 11:51:49",
                Status = "Active",
                RoleId = roleId,
                Gender = 1
            };
        }

        public static List<EmployeeAccountDto> GetSampleEmployeeAccounts()
        {
            return new List<EmployeeAccountDto>{
                new EmployeeAccountDto
                {
                    Username = "khangstaff",
                    AccountId = 4,
                    Name = "khang staff",
                    Email = "khangstaff592@gmail.com",
                    Phone = "0987876575",
                    DateCreate = "10/01/2024 11:51:49",
                    Status = "Active",
                    RoleId = 2,
                    Gender = 1,
                    DateExpire = "12/31/2025 23:59:59",
                    DateBirth = "09/13/2004 04:51:21",
                    AvatarImg = "https://res.cloudinary.com/dfdwupiah/image/upload/v1728103743/MMRMS/qbjsfiyperlviohymsmv.png"
                },
                new EmployeeAccountDto
                {
                    Username = "khangmanager",
                    AccountId = 6,
                    Name = "khang manager",
                    Email = "khangmanager92@gmail.com",
                    Phone = "0987876575",
                    DateCreate = "10/01/2024 11:51:49",
                    Status = "Active",
                    RoleId = 1,
                    Gender = 1,
                    DateExpire = "12/31/2025 23:59:59",
                    DateBirth = "09/13/2004 04:51:21",
                    AvatarImg = "https://res.cloudinary.com/dfdwupiah/image/upload/v1728103743/MMRMS/qbjsfiyperlviohymsmv.png"
                },
                new EmployeeAccountDto
                {
                    Username = "khoamanager",
                    AccountId = 9,
                    Name = "KHOA",
                    Email = "khoamanager@yopmail.com",
                    Phone = "0123456789",
                    DateCreate = "10/02/2024 15:47:10",
                    Status = "Active",
                    RoleId = 1,
                    Gender = 2,
                    DateExpire = "12/31/2025 23:59:59",
                    DateBirth = "06/15/2003 00:00:00",
                    AvatarImg = "https://res.cloudinary.com/dfdwupiah/image/upload/v1728103743/MMRMS/qbjsfiyperlviohymsmv.png"
                },
                new EmployeeAccountDto
                {
                    Username = "khoastaff2",
                    AccountId = 12,
                    Name = "khoa staff2",
                    Email = "khoastaff2@yopmail.com",
                    Phone = "0123456789",
                    DateCreate = "10/02/2024 15:47:10",
                    Status = "Active",
                    RoleId = 3,
                    Gender = 2,
                    DateExpire = "12/31/2025 23:59:59",
                    DateBirth = "06/15/2003 00:00:00",
                    AvatarImg = "https://res.cloudinary.com/dfdwupiah/image/upload/v1728103743/MMRMS/qbjsfiyperlviohymsmv.png"
                }
            };
        }
    }
}
