using AutoMapper;
using Common;
using Common.Enum;
using DTOs.AccountBusiness;
using DTOs.Contract;
using DTOs.Invoice;
using DTOs.MachineSerialNumber;
using DTOs.RentingRequest;
using DTOs.RentingRequestAddress;
using Moq;
using Repository.Interface;
using Repository.Mapper;
using Service.Exceptions;
using Service.Implement;
using Service.Interface;
using Xunit;
using Assert = Xunit.Assert;

namespace Test.Service
{
    public class RentingRequestServiceTest
    {
        private readonly Mock<IRentingRequestRepository> _rentingRequestRepositoryMock;
        private readonly Mock<IMachineSerialNumberRepository> _machineSerialNumberRepositoryMock;
        private readonly Mock<IAddressRepository> _addressRepositoryMock;
        private readonly Mock<IContractRepository> _contractRepositoryMock;
        private readonly Mock<IInvoiceRepository> _invoiceRepositoryMock;
        private readonly IRentingRequestService _rentingRequestService;
        private readonly IMapper _mapper;

        public RentingRequestServiceTest()
        {
            _rentingRequestRepositoryMock = new Mock<IRentingRequestRepository>();
            _machineSerialNumberRepositoryMock = new Mock<IMachineSerialNumberRepository>();
            _addressRepositoryMock = new Mock<IAddressRepository>();
            _contractRepositoryMock = new Mock<IContractRepository>();
            _invoiceRepositoryMock = new Mock<IInvoiceRepository>();

            _rentingRequestService = new RentingRequestServiceImpl(_rentingRequestRepositoryMock.Object,
                _machineSerialNumberRepositoryMock.Object,
                _addressRepositoryMock.Object,
                _contractRepositoryMock.Object,
                _invoiceRepositoryMock.Object);

            _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<AutoMapperProfile>();
            }));
        }

        [Fact]
        public async Task GetRentingRequests_ReturnSuccessfully()
        {
            //Arrange
            string validStatus = "Signed";
            var expected = GetSampleRentingRequestDtos();
            _rentingRequestRepositoryMock.Setup(x => x.GetRentingRequests()).ReturnsAsync(expected);

            //Act
            var result = await _rentingRequestService.GetRentingRequests(validStatus);

            //Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetRentingRequests_ThrowsException_InvalidStatusValue()
        {
            //Arrange
            string invalidStatus = "INVALID-STATUS";

            //Act
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _rentingRequestService.GetRentingRequests(invalidStatus));

            //Assert
            Assert.Equal(MessageConstant.InvalidStatusValue, exception.Message);
        }

        [Fact]
        public async Task CreateRentingRequest_ReturnSuccessfully()
        {
            //Arrange
            var customerId = 1;
            var newRentingRequestDto = GetSampleNewRentingRequest();
            var machineIds = newRentingRequestDto.RentingRequestMachineDetails.Select(m => m.MachineId).Distinct().ToList();
            var rentingRequestDto = GetSampleRentingRequestDto();
            var allAvailableSerialNumbers = GetSampleMachineSerialNumberDtoList();
            var (depositInvoice, rentalInvoice) = GetSampleInvoiceDtos(customerId);

            _addressRepositoryMock.Setup(x => x.IsAddressValid(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            _rentingRequestRepositoryMock.Setup(x => x.CreateRentingRequest(It.IsAny<int>(), It.IsAny<NewRentingRequestDto>())).ReturnsAsync(rentingRequestDto);
            _machineSerialNumberRepositoryMock.Setup(x => x.GetMachineSerialNumberAvailablesToRent(It.Is<List<int>>(list => list != null && list.Count > 0), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(allAvailableSerialNumbers);
            _invoiceRepositoryMock.Setup(x => x.InitInvoices(It.IsAny<RentingRequestDto>())).ReturnsAsync(((depositInvoice, rentalInvoice)));
            _contractRepositoryMock.Setup(x => x.CreateContract(It.IsAny<RentingRequestDto>(), It.IsAny<MachineSerialNumberDto>(), It.IsAny<InvoiceDto>(), It.IsAny<InvoiceDto>())).ReturnsAsync((depositInvoice, rentalInvoice));

            //Act
            var result = await _rentingRequestService.CreateRentingRequest(customerId, newRentingRequestDto);

            //Assert
            _addressRepositoryMock.Verify(x => x.IsAddressValid(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _rentingRequestRepositoryMock.Verify(x => x.CreateRentingRequest(It.IsAny<int>(), It.IsAny<NewRentingRequestDto>()), Times.Once);
            _machineSerialNumberRepositoryMock.Verify(x => x.GetMachineSerialNumberAvailablesToRent(It.Is<List<int>>(list => list != null && list.Count > 0), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
            _invoiceRepositoryMock.Verify(x => x.InitInvoices(It.IsAny<RentingRequestDto>()), Times.Once);
            _contractRepositoryMock.Verify(x => x.CreateContract(It.IsAny<RentingRequestDto>(), It.IsAny<MachineSerialNumberDto>(), It.IsAny<InvoiceDto>(), It.IsAny<InvoiceDto>()), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateRentingRequest_ThrowsException_RequestMachinesInvalid()
        {
            //Arrange
            var customerId = 1;
            var newRentingRequestDto = GetSampleNewRentingRequest();
            var machineIds = newRentingRequestDto.RentingRequestMachineDetails.Select(m => m.MachineId).Distinct().ToList();
            var rentingRequestDto = GetSampleRentingRequestDto();
            var allAvailableSerialNumbers = GetSampleMachineSerialNumberDtoList();
            var (depositInvoice, rentalInvoice) = GetSampleInvoiceDtos(customerId);

            _addressRepositoryMock.Setup(x => x.IsAddressValid(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            _rentingRequestRepositoryMock.Setup(x => x.CreateRentingRequest(It.IsAny<int>(), It.IsAny<NewRentingRequestDto>())).ReturnsAsync(rentingRequestDto);
            _machineSerialNumberRepositoryMock.Setup(x => x.GetMachineSerialNumberAvailablesToRent(It.Is<List<int>>(list => list != null && list.Count > 0), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync([]);
            _invoiceRepositoryMock.Setup(x => x.InitInvoices(It.IsAny<RentingRequestDto>())).ReturnsAsync(((depositInvoice, rentalInvoice)));

            //Act
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _rentingRequestService.CreateRentingRequest(customerId, newRentingRequestDto));

            //Assert
            _addressRepositoryMock.Verify(x => x.IsAddressValid(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _rentingRequestRepositoryMock.Verify(x => x.CreateRentingRequest(It.IsAny<int>(), It.IsAny<NewRentingRequestDto>()), Times.Once);
            _machineSerialNumberRepositoryMock.Verify(x => x.GetMachineSerialNumberAvailablesToRent(It.Is<List<int>>(list => list != null && list.Count > 0), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
            _invoiceRepositoryMock.Verify(x => x.InitInvoices(It.IsAny<RentingRequestDto>()), Times.Once);
            Assert.Equal(MessageConstant.RentingRequest.RequestMachinesInvalid, exception.Message);
        }

        [Fact]
        public async Task CreateRentingRequest_ThrowsException_RequestAddressInvalid()
        {
            //Arrange
            var newRentingRequestDto = GetSampleNewRentingRequest();
            var customerId = 1;
            _addressRepositoryMock.Setup(x => x.IsAddressValid(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            //Act
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _rentingRequestService.CreateRentingRequest(customerId, newRentingRequestDto));

            //Assert
            _addressRepositoryMock.Verify(x => x.IsAddressValid(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            Assert.Equal(MessageConstant.RentingRequest.RequestAddressInvalid, exception.Message);
        }

        [Fact]
        public async Task CancelRentingRequest_ReturnSuccessfully()
        {
            //Arranage
            var rentingRequestId = "REH202410281009449979705";
            _rentingRequestRepositoryMock.Setup(x => x.IsRentingRequestValidToCancel(rentingRequestId)).ReturnsAsync(true);
            _rentingRequestRepositoryMock.Setup(x => x.CancelRentingRequest(rentingRequestId)).ReturnsAsync(true);

            //Act
            var result = await _rentingRequestService.CancelRentingRequest(rentingRequestId);

            //Assert
            _rentingRequestRepositoryMock.Verify(x => x.IsRentingRequestValidToCancel(rentingRequestId), Times.Once);
            _rentingRequestRepositoryMock.Verify(x => x.CancelRentingRequest(rentingRequestId), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task CancelRentingRequest_ThrowsException_RentingRequestCanNotCancel()
        {
            //Arranage
            var rentingRequestId = "REH202410281009449979705";
            _rentingRequestRepositoryMock.Setup(x => x.IsRentingRequestValidToCancel(rentingRequestId)).ReturnsAsync(false);

            //Act
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _rentingRequestService.CancelRentingRequest(rentingRequestId));

            //Assert
            _rentingRequestRepositoryMock.Verify(x => x.IsRentingRequestValidToCancel(It.IsAny<string>()), Times.Once);
            Assert.Equal(MessageConstant.RentingRequest.RentingRequestCanNotCancel, exception.Message);
        }

        [Fact]
        public async Task GetRentingRequestDetail_ReturnSuccessfully()
        {
            //Arrange
            var rentingRequestDetailDto = GetSampleRentingRequestDetail();
            var rentingRequestId = "REH202410281009449979705";
            _rentingRequestRepositoryMock.Setup(x => x.GetRentingRequestDetailById(rentingRequestId)).ReturnsAsync(rentingRequestDetailDto);

            //Act
            var result = await _rentingRequestService.GetRentingRequestDetail(rentingRequestId);

            //Assert
            _rentingRequestRepositoryMock.Verify(x => x.GetRentingRequestDetailById(It.IsAny<string>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(rentingRequestId, result.RentingRequestId);
        }

        [Fact]
        public async Task GetRentingRequestDetail_ThrowsException_RentingRequestNotFound()
        {
            //Arrange
            var rentingRequestId = "INVALIDRENTINGREQUSTID";
            _rentingRequestRepositoryMock.Setup(x => x.GetRentingRequestDetailById(rentingRequestId)).ReturnsAsync((RentingRequestDetailDto)null);

            //Act
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _rentingRequestService.GetRentingRequestDetail(rentingRequestId));

            //Assert
            _rentingRequestRepositoryMock.Verify(x => x.GetRentingRequestDetailById(It.IsAny<string>()), Times.Once);
            Assert.Equal(MessageConstant.RentingRequest.RentingRequestNotFound, exception.Message);
        }

        public (InvoiceDto DepositInvoice, InvoiceDto RentalInvoice) GetSampleInvoiceDtos(int accountPaidId)
        {
            var depositInvoiceDto = new InvoiceDto
            {
                InvoiceId = GlobalConstant.InvoiceIdPrefixPattern + "DEPOSIT" + DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern),
                Amount = 0,
                AccountPaidId = accountPaidId,
                Status = InvoiceStatusEnum.Pending.ToString(),
                Type = InvoiceTypeEnum.Deposit.ToString(),
                DateCreate = DateTime.Now,
                Note = "Sample deposit invoice",
            };

            var rentalInvoiceDto = new InvoiceDto
            {
                InvoiceId = GlobalConstant.InvoiceIdPrefixPattern + "RENTAL" + DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern),
                Amount = 0,
                AccountPaidId = accountPaidId,
                Status = InvoiceStatusEnum.Pending.ToString(),
                Type = InvoiceTypeEnum.Rental.ToString(),
                DateCreate = DateTime.Now,
                Note = "Sample rental invoice",
            };

            return (depositInvoiceDto, rentalInvoiceDto);
        }

        public List<MachineSerialNumberDto> GetSampleMachineSerialNumberDtoList()
        {
            return new List<MachineSerialNumberDto>
            {
                new MachineSerialNumberDto
                {
                    SerialNumber = "SN001",
                    MachineId = 1,
                    ActualRentPrice = 500000.0,
                    Status = "Available",
                    DateCreate = DateTime.Parse("2024-09-29T17:00:00.000Z"),
                    RentDaysCounter = 0
                }
            };
        }

        public RentingRequestDto GetSampleRentingRequestDto()
        {
            return new RentingRequestDto
            {
                RentingRequestId = "REH202411011529528422713",
                AccountOrderId = 15,
                AccountOrderName = "KHOA",
                DateCreate = DateTime.Parse("2024-11-01T08:29:52.842Z"),
                DateStart = DateTime.Parse("2024-11-09T17:00:00.000Z"),
                DateEnd = DateTime.Parse("2025-02-09T17:00:00.000Z"),
                TotalRentPrice = 93000000.0,
                TotalDepositPrice = 3000000.0,
                TotalServicePrice = 1000000.0,
                ShippingPrice = 200000.0,
                DiscountPrice = 100000.0,
                NumberOfMonth = 3,
                TotalAmount = 97100000.0,
                IsOnetimePayment = true,
                Note = "NOTE",
                Status = "UnPaid"
            };
        }

        public List<RentingRequestDto> GetSampleRentingRequestDtos()
        {
            return new List<RentingRequestDto>
            {
                new RentingRequestDto
                {
                    RentingRequestId = "REH202411011529528422713",
                    AccountOrderId = 15,
                    AccountOrderName = "KHOA",
                    DateCreate = DateTime.Parse("2024-11-01T08:29:52.842Z"),
                    DateStart = DateTime.Parse("2024-11-09T17:00:00.000Z"),
                    DateEnd = DateTime.Parse("2025-02-09T17:00:00.000Z"),
                    TotalRentPrice = 93000000.0,
                    TotalDepositPrice = 3000000.0,
                    TotalServicePrice = 1000000.0,
                    ShippingPrice = 200000.0,
                    DiscountPrice = 100000.0,
                    NumberOfMonth = 3,
                    TotalAmount = 97100000.0,
                    IsOnetimePayment = true,
                    Note = "NOTE",
                    Status = "Signed"
                },
                new RentingRequestDto
                {
                    RentingRequestId = "REH202411011529528422714",
                    AccountOrderId = 16,
                    AccountOrderName = "ANH",
                    DateCreate = DateTime.Parse("2024-11-01T08:30:00.000Z"),
                    DateStart = DateTime.Parse("2024-11-10T17:00:00.000Z"),
                    DateEnd = DateTime.Parse("2025-02-10T17:00:00.000Z"),
                    TotalRentPrice = 85000000.0,
                    TotalDepositPrice = 2500000.0,
                    TotalServicePrice = 1500000.0,
                    ShippingPrice = 300000.0,
                    DiscountPrice = 50000.0,
                    NumberOfMonth = 2,
                    TotalAmount = 86000000.0,
                    IsOnetimePayment = false,
                    Note = "NOTE 2",
                    Status = "Signed"
                },
                new RentingRequestDto
                {
                    RentingRequestId = "REH202411011529528422715",
                    AccountOrderId = 17,
                    AccountOrderName = "LAN",
                    DateCreate = DateTime.Parse("2024-11-01T08:31:00.000Z"),
                    DateStart = DateTime.Parse("2024-11-11T17:00:00.000Z"),
                    DateEnd = DateTime.Parse("2025-02-11T17:00:00.000Z"),
                    TotalRentPrice = 100000000.0,
                    TotalDepositPrice = 5000000.0,
                    TotalServicePrice = 2000000.0,
                    ShippingPrice = 400000.0,
                    DiscountPrice = 100000.0,
                    NumberOfMonth = 4,
                    TotalAmount = 99000000.0,
                    IsOnetimePayment = true,
                    Note = "NOTE 3",
                    Status = "Canceled"
                }
            };
        }

        public RentingRequestDetailDto GetSampleRentingRequestDetail()
        {
            return new RentingRequestDetailDto
            {
                RentingRequestId = "REH202410281009449979705",
                AccountOrderId = 1,
                DateCreate = DateTime.Now.AddMonths(-1),
                DateStart = DateTime.Now,
                TotalRentPrice = 5000.0,
                TotalDepositPrice = 1000.0,
                TotalServicePrice = 300.0,
                ShippingPrice = 50.0,
                DiscountPrice = 200.0,
                NumberOfMonth = 12,
                TotalAmount = 6150.0,
                IsOnetimePayment = true,
                Note = "Sample renting request note",
                Status = "Active",
                ServiceRentingRequests = new List<ServiceRentingRequestDto>
                {
                    new ServiceRentingRequestDto
                    {
                        RentingServiceId = 201,
                        ServicePrice = 150.0,
                        RentingServiceName = "Sample Service 1"
                    },
                    new ServiceRentingRequestDto
                    {
                        RentingServiceId = 202,
                        ServicePrice = 150.0,
                        RentingServiceName = "Sample Service 2"
                    }
                },
                AccountOrder = new AccountOrderDto
                {
                    AccountId = 1,
                    Name = "John Doe",
                    Email = "johndoe@example.com",
                    Phone = "0123456789"
                },
                RentingRequestAddress = new RentingRequestAddressDto
                {
                    RentingRequestId = "REH202410281009449979705",
                    AddressBody = "123 Sample Street",
                    District = "Sample District",
                    City = "Sample City",
                    Coordinates = "10.762622, 106.660172"
                },
                AccountBusiness = new AccountBusinessDto
                {
                    AccountId = 1,
                    Company = "Sample Company Ltd.",
                    Address = "456 Business Avenue",
                    Position = "Manager",
                    TaxNumber = "123456789"
                },
                Contracts = new List<ContractDto>
                {
                    new ContractDto
                    {
                        ContractId = "CON202410281009449979705",
                        ContractName = "Sample Contract",
                        RentingRequestId = "REH202410281009449979705",
                        DateCreate = DateTime.Now.AddMonths(-1),
                        DateSign = DateTime.Now.AddDays(-15),
                        DateStart = DateTime.Now,
                        DateEnd = DateTime.Now.AddMonths(12),
                        Status = "Signed",
                        MachineId = 101,
                        MachineName = "Excavator Model X",
                        SerialNumber = "SN123456789",
                        RentPrice = 5000.0,
                        Thumbnail = "https://example.com/sample-thumbnail.jpg"
                    }
                }
            };
        }

        public NewRentingRequestDto GetSampleNewRentingRequest()
        {
            return new NewRentingRequestDto
            {
                AddressId = 1,
                DateStart = new DateTime(2024, 10, 1),
                DateEnd = new DateTime(2025, 10, 1),
                ShippingPrice = 100.50,
                DiscountPrice = 10.0,
                NumberOfMonth = 12,
                IsOnetimePayment = true,
                Note = "This is a sample note.",
                RentingRequestMachineDetails = new List<NewRentingRequestMachineDetailDto>
                {
                    new NewRentingRequestMachineDetailDto
                    {
                        MachineId = 1,
                        Quantity = 1
                    }
                },
                ServiceRentingRequests = new List<int> { 201, 202 }
            };
        }
    }
}
