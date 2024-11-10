using AutoMapper;
using Castle.Core.Resource;
using Common;
using Common.Enum;
using DTOs.Contract;
using DTOs.MachineSerialNumber;
using DTOs.RentingRequest;
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
    public class ContractServiceTest
    {
        private readonly Mock<IContractRepository> _contractRepositoryMock;
        private readonly Mock<IMachineSerialNumberRepository> _machineSerialNumberRepositoryMock;
        private readonly Mock<IInvoiceRepository> _invoiceRepositoryMock;
        private readonly IContractService _contractService;
        private readonly IMapper _mapper;

        public ContractServiceTest()
        {
            _machineSerialNumberRepositoryMock = new Mock<IMachineSerialNumberRepository>();
            _contractRepositoryMock = new Mock<IContractRepository>();
            _invoiceRepositoryMock = new Mock<IInvoiceRepository>();

            _contractService = new ContractServiceImpl(
                _contractRepositoryMock.Object,
                _machineSerialNumberRepositoryMock.Object,
                _invoiceRepositoryMock.Object);

            _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<AutoMapperProfile>();
            }));
        }

        [Fact]
        public async Task EndContract_ReturnSuccessfully()
        {
            //Arrange
            var contractId = "CON20241104NO0005";
            var currentContractDto = GetSampleContractDtoStatusRenting();
            var updatedContractDto = new ContractDto
            {
                ContractId = currentContractDto.ContractId,
                ContractName = currentContractDto.ContractName,
                RentingRequestId = currentContractDto.RentingRequestId,
                DateCreate = currentContractDto.DateCreate,
                DateSign = currentContractDto.DateSign,
                DateStart = currentContractDto.DateStart,
                DateEnd = currentContractDto.DateEnd,
                Status = ContractStatusEnum.InspectionPending.ToString(),
                DepositPrice = currentContractDto.DepositPrice,
                RentPeriod = currentContractDto.RentPeriod,
                TotalRentPrice = currentContractDto.TotalRentPrice,
                MachineId = currentContractDto.MachineId,
                MachineName = currentContractDto.MachineName,
                SerialNumber = currentContractDto.SerialNumber,
                RentPrice = currentContractDto.RentPrice,
                Thumbnail = currentContractDto.Thumbnail,
                AccountSignId = currentContractDto.AccountSignId
            };

            _contractRepositoryMock.Setup(x => x.GetContractById(It.IsAny<string>())).ReturnsAsync(currentContractDto);
            _contractRepositoryMock.Setup(x => x.EndContract(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>())).ReturnsAsync(updatedContractDto);
            _machineSerialNumberRepositoryMock.Setup(x => x.UpdateRentDaysCounterMachineSerialNumber(It.IsAny<string>(), It.IsAny<int>()));

            //Act
            var result = await _contractService.EndContract(contractId);

            //Assert
            _contractRepositoryMock.Verify(x => x.GetContractById(It.IsAny<string>()), Times.Once);
            _contractRepositoryMock.Verify(x => x.EndContract(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>()), Times.Once);
            _machineSerialNumberRepositoryMock.Verify(x => x.UpdateRentDaysCounterMachineSerialNumber(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task EndContract_ThrowsException_ContractNotFound()
        {
            //Arrange
            var contractId = "CON20241104NO0005";
            _contractRepositoryMock.Setup(x => x.GetContractById(It.IsAny<string>())).ReturnsAsync((ContractDto)null);

            //Act
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _contractService.EndContract(contractId));

            //Assert
            Assert.Equal(MessageConstant.Contract.ContractNotFound, exception.Message);
        }

        [Fact]
        public async Task EndContract_ThrowsException_ContractNotValidToEnd()
        {
            //Arrange
            var contractId = "CON20241104NO0005";
            var currentContractDto = GetSampleContractDtoStatusRenting();
            currentContractDto.Status = ContractStatusEnum.Completed.ToString();

            _contractRepositoryMock.Setup(x => x.GetContractById(It.IsAny<string>())).ReturnsAsync(currentContractDto);

            //Act
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _contractService.EndContract(contractId));

            //Assert
            Assert.Equal(MessageConstant.Contract.ContractNotValidToEnd, exception.Message);
        }

        [Fact]
        public async Task ExtendContract_ReturnSuccessfully()
        {
            var contractId = "CON20241104NO0005";
            var contractExtendDto = new ContractExtendDto
            {
                DateStart = DateTime.Parse("2025-02-10T00:00:00"),
                DateEnd = DateTime.Parse("2025-01-31T00:00:00"),
            };
            var currentContractDto = GetSampleContractDtoStatusRenting();
            var extendContractDto = new ContractDto
            {
                ContractId = "CON20241109NO0007",
                ContractName = "Hợp đồng thuê máy SN001",
                RentingRequestId = "REH20241025111527",
                DateCreate = DateTime.Parse("2024-11-09T16:55:45.1049981"),
                DateSign = null,
                DateStart = DateTime.Parse("2025-02-10T00:00:00"),
                DateEnd = DateTime.Parse("2025-03-31T00:00:00"),
                Status = "NotSigned",
                DepositPrice = 0,
                RentPeriod = 50,
                TotalRentPrice = 25000000,
                MachineId = 1,
                MachineName = "Máy xúc thủy lực",
                SerialNumber = "SN001",
                RentPrice = 500000,
                Thumbnail = "https://res.cloudinary.com/dfdwupiah/image/upload/v1729328591/MMRMS/lavbqsidqtuvpfnwb57k.jpg",
                AccountSignId = 15
            };

            _contractRepositoryMock.Setup(x => x.GetContractById(It.IsAny<string>())).ReturnsAsync(currentContractDto);
            _contractRepositoryMock.Setup(x => x.ExtendContract(It.IsAny<string>(), It.IsAny<ContractExtendDto>())).ReturnsAsync(extendContractDto);
        }

        public ContractDto GetSampleContractDtoStatusRenting()
        {
            return new ContractDto
            {
                ContractId = "CON20241104NO0005",
                ContractName = "Hợp đồng thuê máy SN009",
                RentingRequestId = "REH20241104NO0003",
                DateCreate = DateTime.Parse("2024-11-04T09:47:24.9814697"),
                DateSign = DateTime.Parse("2024-11-04T10:58:53.4701082"),
                DateStart = DateTime.Parse("2024-11-10T00:00:00"),
                DateEnd = DateTime.Parse("2025-01-10T00:00:00"),
                Status = "Renting",
                DepositPrice = 1500000,
                RentPeriod = 62,
                TotalRentPrice = 31000000,
                MachineId = 1,
                MachineName = "Máy xúc thủy lực",
                SerialNumber = "SN009",
                RentPrice = 500000,
                Thumbnail = "https://res.cloudinary.com/dfdwupiah/image/upload/v1729328591/MMRMS/lavbqsidqtuvpfnwb57k.jpg",
                AccountSignId = 15
            };
        }
    }
}
