using AutoMapper;
using DTOs.Contract;
using Moq;
using Repository.Interface;
using Repository.Mapper;
using Service.Implement;
using Service.Interface;
using Xunit;

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
            var contractId = "CON20241109NO0014";
            var contractDto = GetSampleContractDtoStatusRenting();

            _contractRepositoryMock.Setup(x => x.GetContractById(It.IsAny<string>())).ReturnsAsync(contractDto);


            //Act

            //Assert
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
