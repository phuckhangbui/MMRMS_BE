using AutoMapper;
using BusinessObject;
using Common;
using Common.Enum;
using DAO;
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
                return _mapper.Map<ContractDetailDto>(contract);
            }

            return null;
        }

        public async Task<IEnumerable<ContractDetailDto>> GetContractDetailListByRentingRequestId(string rentingRequestId)
        {
            var contracts = await ContractDao.Instance.GetContractsByRentingRequestId(rentingRequestId);

            if (!contracts.IsNullOrEmpty())
            {
                return _mapper.Map<IEnumerable<ContractDetailDto>>(contracts);
            }

            return [];
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

        public async Task SignContract(string rentingRequestId)
        {
            await ContractDao.Instance.SignContract(rentingRequestId);
        }


        //TODO: Remove
        public async Task<string> CreateContract(int managerId, ContractRequestDto contractRequestDto)
        {
            var rentingRequest = await RentingRequestDao.Instance.GetRentingRequestById(contractRequestDto.RentingRequestId);

            //Contract
            var contract = InitContract(managerId, contractRequestDto, rentingRequest);

            //Contract Address
            //var address = await AddressDao.Instance.GetAddressById((int)rentingRequest.AddressId!);
            //var contractAddress = new ContractAddress()
            //{
            //    AddressBody = address.AddressBody,
            //    City = address.City,
            //    District = address.District,
            //};
            //contract.ContractAddress = contractAddress;

            var isOneTimePayment = rentingRequest.IsOnetimePayment;
            if ((bool)isOneTimePayment!)
            {
                var contractPayment = new ContractPayment
                {
                    Title = string.Empty,
                    Amount = 0,
                    //CustomerPaidDate = DateTime.Now,
                    //SystemPaidDate = DateTime.Now,
                    Status = string.Empty,
                    DateCreate = DateTime.Now,
                };

                var invoice = new Invoice
                {
                    InvoiceId = GlobalConstant.InvoiceIdPrefixPattern + DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern),
                    AccountPaidId = rentingRequest.AccountOrderId,
                    Amount = contractPayment.Amount,
                    PaymentMethod = string.Empty,
                    Type = string.Empty,
                    Note = string.Empty,
                    DateCreate = contractPayment.DateCreate,
                    Status = string.Empty,
                    //ContractPayment = contractPayment
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
                        Amount = 0,
                        CustomerPaidDate = DateTime.Now,
                        //SystemPaidDate = DateTime.Now,
                        Status = string.Empty,
                        DateCreate = DateTime.Now,
                    };

                    contract.ContractPayments.Add(contractPayment);
                }
            }

            await ContractDao.Instance.CreateContract(contract, contractRequestDto);

            //Update renting request
            //rentingRequest.ContractId = contract.ContractId;
            //rentingRequest.Status = RentingRequestStatusEnum.Approved.ToString();
            await RentingRequestDao.Instance.UpdateAsync(rentingRequest);

            return contract.ContractId;
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
                //AccountCreateId = managerId,
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

            return contract;
        }
        public async Task<ContractAddressDto> GetContractAddressById(string contractId)
        {
            var address = await ContractDao.Instance.GetRentingRequestAddressByContractId(contractId);

            if (address == null)
            {
                return null;
            }

            return _mapper.Map<ContractAddressDto>(address);
        }

        public async Task<bool> IsContractValidToSign(string rentingRequestId)
        {
            var contracts = await ContractDao.Instance.GetContractsByRentingRequestId(rentingRequestId);

            return contracts.All(c => c.Status.Equals(ContractStatusEnum.NotSigned.ToString()));
        }
    }
}
