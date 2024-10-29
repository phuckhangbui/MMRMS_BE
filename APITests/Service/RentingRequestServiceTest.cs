using AutoMapper;
using Common;
using DTOs.AccountAddressDto;
using DTOs.Contract;
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
        private readonly IRentingRequestService _rentingRequestService;
        private readonly IMapper _mapper;

        public RentingRequestServiceTest()
        {
            _rentingRequestRepositoryMock = new Mock<IRentingRequestRepository>();
            _machineSerialNumberRepositoryMock = new Mock<IMachineSerialNumberRepository>();
            _addressRepositoryMock = new Mock<IAddressRepository>();

            _rentingRequestService = new RentingRequestServiceImpl(_rentingRequestRepositoryMock.Object, _machineSerialNumberRepositoryMock.Object, _addressRepositoryMock.Object);

            _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<AutoMapperProfile>();
            }));
        }

        [Fact]
        public async Task CreateRentingRequest_ReturnSuccessfully()
        {
            //Arrange
            var newRentingRequestDto = GetSampleNewRentingRequest();
            var customerId = 1;
            var newRentingRequestId = "REH202410281009449979705";
            _machineSerialNumberRepositoryMock.Setup(x => x.CheckMachineSerialNumberValidToRequest(newRentingRequestDto)).ReturnsAsync(true);
            _addressRepositoryMock.Setup(x => x.IsAddressValid(newRentingRequestDto.AddressId, customerId)).ReturnsAsync(true);
            _rentingRequestRepositoryMock.Setup(x => x.CreateRentingRequest(customerId, newRentingRequestDto)).ReturnsAsync(newRentingRequestId);

            //Act
            var result = await _rentingRequestService.CreateRentingRequest(customerId, newRentingRequestDto);

            //Assert
            _machineSerialNumberRepositoryMock.Verify(x => x.CheckMachineSerialNumberValidToRequest(It.IsAny<NewRentingRequestDto>()), Times.Once);
            _addressRepositoryMock.Verify(x => x.IsAddressValid(It.IsAny<int>(), customerId), Times.Once);
            _rentingRequestRepositoryMock.Verify(x => x.CreateRentingRequest(customerId, newRentingRequestDto), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(newRentingRequestId, result);
        }

        [Fact]
        public async Task CreateRentingRequest_ThrowsException_RequestMachinesInvalid()
        {
            //Arrange
            var newRentingRequestDto = GetSampleNewRentingRequest();
            var customerId = 1;
            _machineSerialNumberRepositoryMock.Setup(x => x.CheckMachineSerialNumberValidToRequest(newRentingRequestDto)).ReturnsAsync(false);

            //Act
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _rentingRequestService.CreateRentingRequest(customerId, newRentingRequestDto));

            //Assert
            _machineSerialNumberRepositoryMock.Verify(x => x.CheckMachineSerialNumberValidToRequest(It.IsAny<NewRentingRequestDto>()), Times.Once);
            Assert.Equal(MessageConstant.RentingRequest.RequestMachinesInvalid, exception.Message);
        }

        [Fact]
        public async Task CreateRentingRequest_ThrowsException_RequestAddressInvalid()
        {
            //Arrange
            var newRentingRequestDto = GetSampleNewRentingRequest();
            var customerId = 1;
            _machineSerialNumberRepositoryMock.Setup(x => x.CheckMachineSerialNumberValidToRequest(newRentingRequestDto)).ReturnsAsync(true);
            _addressRepositoryMock.Setup(x => x.IsAddressValid(newRentingRequestDto.AddressId, customerId)).ReturnsAsync(false);

            //Act
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _rentingRequestService.CreateRentingRequest(customerId, newRentingRequestDto));

            //Assert
            _machineSerialNumberRepositoryMock.Verify(x => x.CheckMachineSerialNumberValidToRequest(It.IsAny<NewRentingRequestDto>()), Times.Once);
            _addressRepositoryMock.Verify(x => x.IsAddressValid(It.IsAny<int>(), customerId), Times.Once);
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
                        MachineId = 101,
                        Quantity = 2
                    },
                    new NewRentingRequestMachineDetailDto
                    {
                        MachineId = 102,
                        Quantity = 1
                    }
                },
                ServiceRentingRequests = new List<int> { 201, 202 }
            };
        }
    }
}
