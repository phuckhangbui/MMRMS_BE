using AutoMapper;
using Common;
using Common.Enum;
using DTOs.AccountBusiness;
using DTOs.Contract;
using DTOs.ContractPayment;
using DTOs.ContractTerm;
using DTOs.RentingRequest;
using DTOs.RentingRequestAddress;
using Moq;
using Repository.Interface;
using Repository.Mapper;
using Service;
using Service.Exceptions;
using Service.Implement;
using Service.Interface;
using Test.Utils;
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
        private readonly Mock<IBackground> _backgroundMock;
        private readonly Mock<IDateTimeProvider> _dateTimeProvider;
        private readonly IRentingRequestService _rentingRequestService;
        private readonly IMapper _mapper;

        public RentingRequestServiceTest()
        {
            _rentingRequestRepositoryMock = new Mock<IRentingRequestRepository>();
            _machineSerialNumberRepositoryMock = new Mock<IMachineSerialNumberRepository>();
            _addressRepositoryMock = new Mock<IAddressRepository>();
            _contractRepositoryMock = new Mock<IContractRepository>();
            _invoiceRepositoryMock = new Mock<IInvoiceRepository>();
            _backgroundMock = new Mock<IBackground>();
            _dateTimeProvider = new Mock<IDateTimeProvider>();

            _rentingRequestService = new RentingRequestServiceImpl(_rentingRequestRepositoryMock.Object,
                _machineSerialNumberRepositoryMock.Object,
                _addressRepositoryMock.Object,
                _contractRepositoryMock.Object,
                _invoiceRepositoryMock.Object,
                _backgroundMock.Object);

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
            _rentingRequestRepositoryMock.Setup(x => x.GetRentingRequests(It.IsAny<string>())).ReturnsAsync(expected);

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
            var fixedDate = new DateTime(2024, 11, 10);
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(x => x.Now).Returns(fixedDate);

            var customerId = 1;
            var newRentingRequestDto = GetSampleNewRentingRequest();
            var rentingRequestDto = GetSampleRentingRequestDto();

            _addressRepositoryMock.Setup(x => x.IsAddressValid(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            _machineSerialNumberRepositoryMock.Setup(x => x.CheckMachineSerialNumberValidToRent(It.IsAny<List<RentingRequestSerialNumberDto>>())).ReturnsAsync(true);
            _rentingRequestRepositoryMock.Setup(x => x.CreateRentingRequest(It.IsAny<int>(), It.IsAny<NewRentingRequestDto>())).ReturnsAsync(rentingRequestDto);
            _contractRepositoryMock.Setup(x => x.CreateContract(It.IsAny<RentingRequestDto>(), It.IsAny<RentingRequestSerialNumberDto>()));
            _rentingRequestRepositoryMock.Setup(x => x.UpdateRentingRequest(It.IsAny<RentingRequestDto>()));
            _invoiceRepositoryMock.Setup(x => x.CreateInvoice(It.IsAny<string>()));

            //Act
            var result = await _rentingRequestService.CreateRentingRequest(customerId, newRentingRequestDto);

            //Assert
            _addressRepositoryMock.Verify(x => x.IsAddressValid(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _machineSerialNumberRepositoryMock.Verify(x => x.CheckMachineSerialNumberValidToRent(It.IsAny<List<RentingRequestSerialNumberDto>>()), Times.Once);
            _rentingRequestRepositoryMock.Verify(x => x.CreateRentingRequest(It.IsAny<int>(), It.IsAny<NewRentingRequestDto>()), Times.Once);
            _contractRepositoryMock.Verify(x => x.CreateContract(It.IsAny<RentingRequestDto>(), It.IsAny<RentingRequestSerialNumberDto>()), Times.Once);
            _rentingRequestRepositoryMock.Verify(x => x.UpdateRentingRequest(It.IsAny<RentingRequestDto>()), Times.Once);
            _invoiceRepositoryMock.Verify(x => x.CreateInvoice(It.IsAny<string>()), Times.Once);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateRentingRequest_ThrowsException_RequestMachinesInvalid()
        {
            //Arrange
            var customerId = 1;
            var newRentingRequestDto = GetSampleNewRentingRequest();

            _addressRepositoryMock.Setup(x => x.IsAddressValid(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            _machineSerialNumberRepositoryMock.Setup(x => x.CheckMachineSerialNumberValidToRent(It.IsAny<List<RentingRequestSerialNumberDto>>())).ReturnsAsync(false);

            //Act
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _rentingRequestService.CreateRentingRequest(customerId, newRentingRequestDto));

            //Assert
            _addressRepositoryMock.Verify(x => x.IsAddressValid(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _machineSerialNumberRepositoryMock.Verify(x => x.CheckMachineSerialNumberValidToRent(It.IsAny<List<RentingRequestSerialNumberDto>>()), Times.Once);

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
        public async Task CreateRentingRequest_RentPeriodShorterThan90Days_ThrowsException_RentPeriodInValid()
        {
            //Arrange
            var fixedDate = new DateTime(2024, 11, 10);
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(x => x.Now).Returns(fixedDate);

            var customerId = 1;
            var newRentingRequestDto = GetSampleNewRentingRequest();
            var rentingRequestDto = GetSampleRentingRequestDto();

            var rentintRequestSerialNumberDto = new RentingRequestSerialNumberDto
            {
                MachineId = 1,
                SerialNumber = "SN002",
                DateStart = new DateTime(2024, 11, 20),
                DateEnd = new DateTime(2024, 12, 01)
            };
            newRentingRequestDto.RentingRequestSerialNumbers.Add(rentintRequestSerialNumberDto);

            _addressRepositoryMock.Setup(x => x.IsAddressValid(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            _machineSerialNumberRepositoryMock.Setup(x => x.CheckMachineSerialNumberValidToRent(It.IsAny<List<RentingRequestSerialNumberDto>>())).ReturnsAsync(true);

            //Act
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _rentingRequestService.CreateRentingRequest(customerId, newRentingRequestDto));

            //Assert
            Assert.Equal(MessageConstant.RentingRequest.RentPeriodInValid, exception.Message);
        }

        [Fact]
        public async Task CreateRentingRequest_DateStartGreaterThanCurrentDate30Days_ThrowsException_RentPeriodInValid()
        {
            //Arrange
            var fixedDate = new DateTime(2024, 11, 10);
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(x => x.Now).Returns(fixedDate);

            var customerId = 1;
            var newRentingRequestDto = GetSampleNewRentingRequest();
            var rentingRequestDto = GetSampleRentingRequestDto();

            var rentintRequestSerialNumberDto = new RentingRequestSerialNumberDto
            {
                MachineId = 1,
                SerialNumber = "SN002",
                DateStart = new DateTime(2025, 01, 01),
                DateEnd = new DateTime(2025, 05, 01)
            };
            newRentingRequestDto.RentingRequestSerialNumbers.Add(rentintRequestSerialNumberDto);

            _addressRepositoryMock.Setup(x => x.IsAddressValid(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            _machineSerialNumberRepositoryMock.Setup(x => x.CheckMachineSerialNumberValidToRent(It.IsAny<List<RentingRequestSerialNumberDto>>())).ReturnsAsync(true);

            //Act
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _rentingRequestService.CreateRentingRequest(customerId, newRentingRequestDto));

            //Assert
            Assert.Equal(MessageConstant.RentingRequest.RentPeriodInValid, exception.Message);
        }

        [Fact]
        public async Task CreateRentingRequest_RentPeriodGreaterThan365Days_ThrowsException_RentPeriodInValid()
        {
            //Arrange
            var fixedDate = new DateTime(2024, 11, 10);
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(x => x.Now).Returns(fixedDate);

            var customerId = 1;
            var newRentingRequestDto = GetSampleNewRentingRequest();
            var rentingRequestDto = GetSampleRentingRequestDto();

            var rentintRequestSerialNumberDto = new RentingRequestSerialNumberDto
            {
                MachineId = 1,
                SerialNumber = "SN002",
                DateStart = new DateTime(2024, 11, 21),
                DateEnd = new DateTime(2025, 12, 01)
            };
            newRentingRequestDto.RentingRequestSerialNumbers.Add(rentintRequestSerialNumberDto);

            _addressRepositoryMock.Setup(x => x.IsAddressValid(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            _machineSerialNumberRepositoryMock.Setup(x => x.CheckMachineSerialNumberValidToRent(It.IsAny<List<RentingRequestSerialNumberDto>>())).ReturnsAsync(true);

            //Act
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _rentingRequestService.CreateRentingRequest(customerId, newRentingRequestDto));

            //Assert
            Assert.Equal(MessageConstant.RentingRequest.RentPeriodInValid, exception.Message);
        }

        [Fact]
        public async Task CancelRentingRequest_ReturnSuccessfully()
        {
            //Arranage
            //Arranage
            var rentingRequestDetailDto = GetSampleRentingRequestDetail();
            rentingRequestDetailDto.Status = RentingRequestStatusEnum.UnPaid.ToString();
            var rentingRequestId = "REH202410281009449979705";
            _rentingRequestRepositoryMock.Setup(x => x.GetRentingRequestDetailById(It.IsAny<string>())).ReturnsAsync(rentingRequestDetailDto);

            var contractDetatilDto = GetSampleContractDetail();
            List<ContractDetailDto> contractDetails = new List<ContractDetailDto>();
            contractDetails.Add(contractDetatilDto);
            _contractRepositoryMock.Setup(x => x.GetContractDetailListByRentingRequestId(It.IsAny<string>())).ReturnsAsync(contractDetails);

            //Act
            var result = await _rentingRequestService.CancelRentingRequest(rentingRequestId);

            //Assert
            //_rentingRequestRepositoryMock.Verify(x => x.IsRentingRequestValidToCancel(rentingRequestId), Times.Once);
            _rentingRequestRepositoryMock.Verify(x => x.CancelRentingRequest(rentingRequestId), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task CancelRentingRequest_RentingRequestStatusNotUnPaid_ThrowsException_RentingRequestCanNotCancel()
        {
            //Arranage
            var rentingRequestDetailDto = GetSampleRentingRequestDetail();
            var rentingRequestId = "REH202410281009449979705";
            _rentingRequestRepositoryMock.Setup(x => x.GetRentingRequestDetailById(rentingRequestId)).ReturnsAsync(rentingRequestDetailDto);

            //Act
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _rentingRequestService.CancelRentingRequest(rentingRequestId));

            //Assert
            Assert.Equal(MessageConstant.RentingRequest.RentingRequestCanNotCancel, exception.Message);
        }

        [Fact]
        public async Task CancelRentingRequest_AtLeastOneInvoicePaid_ThrowsException_RentingRequestCanNotCancel()
        {
            //Arranage
            var rentingRequestDetailDto = GetSampleRentingRequestDetail();
            rentingRequestDetailDto.Status = RentingRequestStatusEnum.UnPaid.ToString();
            var rentingRequestId = "REH202410281009449979705";
            _rentingRequestRepositoryMock.Setup(x => x.GetRentingRequestDetailById(It.IsAny<string>())).ReturnsAsync(rentingRequestDetailDto);

            var contractDetatilDto = GetSampleContractDetail();
            contractDetatilDto.ContractPayments[0].Status = ContractPaymentStatusEnum.Paid.ToString();
            List<ContractDetailDto> contractDetails = new List<ContractDetailDto>();
            contractDetails.Add(contractDetatilDto);
            _contractRepositoryMock.Setup(x => x.GetContractDetailListByRentingRequestId(It.IsAny<string>())).ReturnsAsync(contractDetails);

            //Act
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _rentingRequestService.CancelRentingRequest(rentingRequestId));

            //Assert
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

        public RentingRequestDto GetSampleRentingRequestDto()
        {
            return new RentingRequestDto
            {
                RentingRequestId = "REH202411011529528422713",
                AccountOrderId = 15,
                AccountOrderName = "KHOA",
                DateCreate = DateTime.Parse("2024-11-01T08:29:52.842Z"),
                TotalRentPrice = 93000000.0,
                TotalDepositPrice = 3000000.0,
                TotalServicePrice = 1000000.0,
                ShippingPrice = 200000.0,
                DiscountPrice = 100000.0,
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
                    TotalRentPrice = 93000000.0,
                    TotalDepositPrice = 3000000.0,
                    TotalServicePrice = 1000000.0,
                    ShippingPrice = 200000.0,
                    DiscountPrice = 100000.0,
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
                    TotalRentPrice = 85000000.0,
                    TotalDepositPrice = 2500000.0,
                    TotalServicePrice = 1500000.0,
                    ShippingPrice = 300000.0,
                    DiscountPrice = 50000.0,
                    TotalAmount = 86000000.0,
                    IsOnetimePayment = false,
                    Note = "NOTE 2",
                    Status = "Signed"
                },
            };
        }

        public RentingRequestDetailDto GetSampleRentingRequestDetail()
        {
            return new RentingRequestDetailDto
            {
                RentingRequestId = "REH202410281009449979705",
                AccountOrderId = 1,
                DateCreate = DateTime.Now.AddMonths(-1),
                TotalRentPrice = 5000.0,
                TotalDepositPrice = 1000.0,
                TotalServicePrice = 300.0,
                ShippingPrice = 50.0,
                DiscountPrice = 200.0,
                TotalAmount = 6150.0,
                IsOnetimePayment = true,
                Note = "Sample renting request note",
                Status = "Signed",
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
                ShippingPrice = 100.50,
                DiscountPrice = 10.0,
                IsOnetimePayment = true,
                Note = "This is a sample note.",
                RentingRequestSerialNumbers = new List<RentingRequestSerialNumberDto>
                {
                    new RentingRequestSerialNumberDto
                    {
                        MachineId = 1,
                        SerialNumber = "SN001",
                        DateStart = new DateTime(2024, 11, 20),
                        DateEnd = new DateTime(2025, 02, 20)
                    }
                },
                ServiceRentingRequests = new List<int> { 201, 202 }
            };
        }

        public ContractDetailDto GetSampleContractDetail()
        {
            return new ContractDetailDto
            {
                IsOnetimePayment = true,
                AccountOrder = new AccountOrderDto
                {
                    AccountId = 15,
                    Name = "khoa",
                    Email = "khoa1@yopmail.com",
                    Phone = "0912345867"
                },
                Content = "",
                AccountBusiness = new AccountBusinessDto
                {
                    AccountId = 15,
                    Company = "FPT",
                    Address = "Thu Duc",
                    Position = "CEO",
                    TaxNumber = "1324"
                },
                ContractPayments = new List<ContractPaymentDto>
                {
                    new ContractPaymentDto
                    {
                        ContractPaymentId = 755,
                        ContractId = "CON20241110NO0007",
                        InvoiceId = "INV20241110NO0011",
                        Title = "Thanh toán tiền đặt cọc cho hợp đồng CON20241110NO0007",
                        Amount = 1500000,
                        Status = "Pending",
                        Type = "Deposit",
                        CustomerPaidDate = null,
                        DateFrom = DateTime.Parse("2024-11-21T00:00:00"),
                        DateTo = DateTime.Parse("2025-02-21T00:00:00"),
                        Period = 93,
                        DateCreate = DateTime.Parse("2024-11-10T21:23:50.33443"),
                        DueDate = DateTime.Parse("2024-11-21T00:00:00"),
                        IsFirstRentalPayment = false,
                        FirstRentalPayment = null
                    },
                    new ContractPaymentDto
                    {
                        ContractPaymentId = 756,
                        ContractId = "CON20241110NO0007",
                        InvoiceId = null,
                        Title = "Hoàn trả tiền đặt cọc cho hợp đồng CON20241110NO0007",
                        Amount = 1500000,
                        Status = "Canceled",
                        Type = "Pending",
                        CustomerPaidDate = null,
                        DateFrom = DateTime.Parse("2025-02-21T00:00:00"),
                        DateTo = DateTime.Parse("2025-02-21T00:00:00"),
                        Period = 93,
                        DateCreate = DateTime.Parse("2024-11-10T21:23:50.3354166"),
                        DueDate = DateTime.Parse("2025-02-21T00:00:00"),
                        IsFirstRentalPayment = false,
                        FirstRentalPayment = null
                    },
                    new ContractPaymentDto
                    {
                        ContractPaymentId = 757,
                        ContractId = "CON20241110NO0007",
                        InvoiceId = "INV20241110NO0012",
                        Title = "Thanh toán tiền thuê cho hợp đồng CON20241110NO0007",
                        Amount = 46500000,
                        Status = "Pending",
                        Type = "Rental",
                        CustomerPaidDate = null,
                        DateFrom = DateTime.Parse("2024-11-21T00:00:00"),
                        DateTo = DateTime.Parse("2025-02-21T00:00:00"),
                        Period = 93,
                        DateCreate = DateTime.Parse("2024-11-10T21:23:50.3355082"),
                        DueDate = DateTime.Parse("2024-11-21T00:00:00"),
                        IsFirstRentalPayment = true,
                        FirstRentalPayment = new FirstRentalPaymentDto
                        {
                            TotalServicePrice = 400000,
                            ShippingPrice = 200000,
                            DiscountPrice = 100000
                        }
                    }
                },
                ContractTerms = new List<ContractTermDto>
                {
                    new ContractTermDto
                    {
                        ContractId = "CON20241110NO0007",
                        Title = "Điều khoản bảo hành",
                        Content = "Bảo hành 12 tháng kể từ ngày giao máy.",
                        DateCreate = DateTime.Parse("2024-11-10T21:23:50.2764151")
                    },
                    new ContractTermDto
                    {
                        ContractId = "CON20241110NO0007",
                        Title = "Điều khoản thanh toán",
                        Content = "Thanh toán trước 30% khi ký hợp đồng, phần còn lại thanh toán sau khi bàn giao.",
                        DateCreate = DateTime.Parse("2024-11-10T21:23:50.2764151")
                    },
                    new ContractTermDto
                    {
                        ContractId = "CON20241110NO0007",
                        Title = "Mục đích sử dụng",
                        Content = "<p>Máy móc cơ khí chỉ được sử dụng cho các công việc chuyên môn theo yêu cầu và chỉ được vận hành bởi những người có đầy đủ trình độ chuyên môn và chứng chỉ liên quan.</p>",
                        DateCreate = DateTime.Parse("2024-11-10T21:23:50.2764151")
                    }
                },
                ContractId = "CON20241110NO0007",
                ContractName = "Hợp đồng thuê máy SN001",
                RentingRequestId = "REH20241110NO0006",
                DateCreate = DateTime.Parse("2024-11-10T21:23:50.2764151"),
                DateSign = null,
                DateStart = DateTime.Parse("2024-11-21T00:00:00"),
                DateEnd = DateTime.Parse("2025-02-21T00:00:00"),
                Status = "Canceled",
                DepositPrice = 1500000,
                RentPeriod = 93,
                TotalRentPrice = 46500000,
                MachineId = 1,
                MachineName = "Máy xúc thủy lực",
                SerialNumber = "SN001",
                RentPrice = 500000,
                Thumbnail = "https://res.cloudinary.com/dfdwupiah/image/upload/v1729328591/MMRMS/lavbqsidqtuvpfnwb57k.jpg",
                AccountSignId = 15
            };
        }
    }
}