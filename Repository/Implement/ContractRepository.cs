using AutoMapper;
using BusinessObject;
using Common;
using DAO;
using DAO.Enum;
using DTOs.Contract;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;

namespace Repository.Implement
{
    public class ContractRepository : IContractRepository
    {
        private readonly IMapper _mapper;

        public ContractRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContractDto>> GetContracts()
        {
            var contracts = await ContractDao.Instance.GetContracts();

            if (!contracts.IsNullOrEmpty())
            {
                var contractDtos = _mapper.Map<IEnumerable<ContractDto>>(contracts);
                return contractDtos;
            }

            return [];
        }

        public async Task<ContractDetailDto?> GetContractDetailById(string contractId)
        {
            var contract = await ContractDao.Instance.GetContractById(contractId);
            if (contract != null)
            {
                var contractDetailDto = _mapper.Map<ContractDetailDto>(contract);
                return contractDetailDto;
            }

            return null;
        }

        public async Task<IEnumerable<ContractDto>> GetContractsForCustomer(int customerId)
        {
            var contracts = await ContractDao.Instance.GetContractsForCustomer(customerId);

            if (!contracts.IsNullOrEmpty())
            {
                return _mapper.Map<IEnumerable<ContractDto>>(contracts);
            }

            return [];
        }

        public async Task CreateContract(int managerId, ContractRequestDto contractRequestDto)
        {
            var rentingRequest = await RentingRequestDao.Instance.GetRentingRequestById(contractRequestDto.RentingRequestId);

            //Contract
            var contract = InitContract(managerId, contractRequestDto, rentingRequest);

            //Contract Address
            var address = await AddressDao.Instance.GetAddressById((int)rentingRequest.AddressId!);
            var contractAddress = new ContractAddress()
            {
                AddressBody = address.AddressBody,
                City = address.City,
                District = address.District,
            };
            contract.ContractAddress = contractAddress;

            //TODO
            var isOneTimePayment = rentingRequest.IsOnetimePayment;
            if ((bool)isOneTimePayment!)
            {
                var contractPayment = new ContractPayment
                {
                    Title = string.Empty,
                    Price = 0,
                    //CustomerPaidDate = DateTime.Now,
                    //SystemPaidDate = DateTime.Now,
                    Status = string.Empty,
                    DateCreate = DateTime.Now,
                };

                var invoice = new Invoice
                {
                    InvoiceId = GlobalConstant.InvoiceIdPrefixPattern + DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern),
                    AccountPaidId = rentingRequest.AccountOrderId,
                    Amount = contractPayment.Price,
                    PaymentMethod = string.Empty,
                    Type = string.Empty,
                    Note = string.Empty,
                    DateCreate = contractPayment.DateCreate,
                    Status = string.Empty,
                    ContractPayment = contractPayment
                };

                contractPayment.InvoiceId = invoice.InvoiceId;
                contract.ContractPayments.Add(contractPayment);
            }
            else
            {
                var rentingPeriod = rentingRequest.NumberOfMonth;
                for (int i = 0; i < rentingPeriod!.Value; i++)
                {
                    var contractPayment = new ContractPayment
                    {
                        Title = string.Empty,
                        Price = 0,
                        CustomerPaidDate = DateTime.Now,
                        SystemPaidDate = DateTime.Now,
                        Status = string.Empty,
                        DateCreate = DateTime.Now,
                    };

                    contract.ContractPayments.Add(contractPayment);
                }
            }

            await ContractDao.Instance.CreateContract(contract, contractRequestDto);

            rentingRequest.ContractId = contract.ContractId;
            await RentingRequestDao.Instance.UpdateAsync(rentingRequest);
        }

        private Contract InitContract(int managerId, ContractRequestDto contractRequestDto, RentingRequest rentingRequest)
        {
            //Contract
            var contract = new Contract()
            {
                ContractId = GlobalConstant.ContractIdPrefixPattern + DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern),
                DateCreate = DateTime.Now,
                Status = ContractStatusEnum.NotSigned.ToString(),

                ContractName = contractRequestDto.ContractName,
                DateStart = contractRequestDto.DateStart,
                DateEnd = contractRequestDto.DateEnd,
                Content = contractRequestDto.Content,
                RentingRequestId = contractRequestDto.RentingRequestId,
                AccountCreateId = managerId,
                AccountSignId = rentingRequest.AccountOrderId,
            };

            //Contract Term
            foreach (var contractTerm in contractRequestDto.ContractTerms)
            {
                var term = new ContractTerm()
                {
                    Content = contractTerm.Content,
                    Title = contractTerm.Title,
                    DateCreate = DateTime.Now,
                };

                contract.ContractTerms.Add(term);
            }

            //Service Contract
            var serviceRentingRequests = rentingRequest.ServiceRentingRequests;
            foreach (var serviceRentingRequest in serviceRentingRequests)
            {
                var serviceContract = new ServiceContract
                {
                    RentingServiceId = serviceRentingRequest.RentingServiceId,
                    ServicePrice = serviceRentingRequest.ServicePrice,
                    DiscountPrice = serviceRentingRequest.DiscountPrice,
                    FinalPrice = serviceRentingRequest.FinalPrice,
                };

                contract.ServiceContracts.Add(serviceContract);
            }

            return contract;
        }
    }
}
