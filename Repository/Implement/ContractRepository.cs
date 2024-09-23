using AutoMapper;
using BusinessObject;
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

        public async Task CreateContract(ContractRequestDto contractRequestDto)
        {
            var context = new MmrmsContext();

            var contract = new Contract()
            {
                ContractId = Guid.NewGuid().ToString(),
                DateCreate = DateTime.Now,
                Status = ContractStatusEnum.Pending.ToString(),

                ContractName = contractRequestDto.ContractName,
                DateStart = contractRequestDto.DateStart,
                DateEnd = contractRequestDto.DateEnd,
                Content = contractRequestDto.Content,

                AccountSignId = contractRequestDto.AccountSignId,
                HiringRequestId = contractRequestDto.HiringRequestId,
                AddressId = contractRequestDto.AddressId,
            };

            foreach(var contractTerm in contractRequestDto.ContractTerms)
            {
                var term = new ContractTerm()
                {
                    Content = contractTerm.Content,
                    Title = contractTerm.Title,
                    DateCreate = DateTime.Now,
                };

                contract.ContractTerms.Add(term);
            }

            foreach (var rentSerialNumberProduct in contractRequestDto.SerialNumberProducts)
            {
                var contractSerialNumberProduct = new ContractSerialNumberProduct()
                {
                    SerialNumber = rentSerialNumberProduct.SerialNumber,

                };
                contract.ContractSerialNumberProducts.Add(contractSerialNumberProduct);

                //TODO
                //Product quantity -- ??
                var serialNumberProduct = await ProductNumberDao.Instance
                    .GetSerialNumberProductBySerialNumberAndProductId(rentSerialNumberProduct.SerialNumber, rentSerialNumberProduct.ProductId);

                serialNumberProduct.Status = SerialMachineStatusEnum.Rented.ToString();
                serialNumberProduct.RentTimeCounter++;

                await ProductNumberDao.Instance.UpdateAsync(serialNumberProduct);
            }

            //var hiringRequest = await HiringRequestDao.Instance.GetHiringRequestById(contractRequestDto.HiringRequestId);
            //var hiringAccount = await AccountDao.Instance.GetAccountAsyncById(contractRequestDto.AccountSignId);
            //var hiringAddress = await AddressDao.Instance.GetAddressById(contractRequestDto.AddressId);

            //contract.HiringRequest = hiringRequest;
            //contract.AccountSign = hiringAccount;
            //contract.Address = hiringAddress;

            //await ContractDao.Instance.CreateAsync(contract);

            context.Contracts.Add(contract);
            await context.SaveChangesAsync();
        }
    }
}
