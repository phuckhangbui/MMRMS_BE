using Common;
using Common.Enum;
using DTOs.Contract;
using DTOs.Invoice;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;

namespace Service.Implement
{
    public class ContractServiceImpl : IContractService
    {
        private readonly IContractRepository _contractRepository;
        private readonly IRentingRequestRepository _rentingRepository;
        private readonly IMachineSerialNumberRepository _machineSerialNumberRepository;

        public ContractServiceImpl(
            IContractRepository contractRepository,
            IRentingRequestRepository rentingRepository,
            IMachineSerialNumberRepository machineSerialNumberRepository)
        {
            _contractRepository = contractRepository;
            _rentingRepository = rentingRepository;
            _machineSerialNumberRepository = machineSerialNumberRepository;
        }

        public async Task<ContractDetailDto> GetContractDetailById(string contractId)
        {
            var contract = await CheckContractExist(contractId);

            return contract;
        }

        public async Task<IEnumerable<ContractDetailDto>> GetContractDetailListByRentingRequestId(string rentingRequestId)
        {
            var contracts = await _contractRepository.GetContractDetailListByRentingRequestId(rentingRequestId);

            if (contracts.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.Contract.ContractListEmpty);
            }

            return contracts;
        }

        public async Task<IEnumerable<ContractDto>> GetContracts(string? status)
        {
            var contracts = await _contractRepository.GetContracts();

            if (!string.IsNullOrEmpty(status))
            {
                if (!Enum.IsDefined(typeof(ContractStatusEnum), status))
                {
                    throw new ServiceException(MessageConstant.InvalidStatusValue);
                }

                contracts = contracts.Where(c => c.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }

            return contracts;
        }

        public async Task<IEnumerable<ContractDto>> GetContractsForCustomer(int customerId, string? status)
        {
            var contracts = await _contractRepository.GetContractsForCustomer(customerId);

            if (!string.IsNullOrEmpty(status))
            {
                if (!Enum.IsDefined(typeof(ContractStatusEnum), status))
                {
                    throw new ServiceException(MessageConstant.InvalidStatusValue);
                }

                contracts = contracts.Where(c => c.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }

            return contracts;
        }

        public async Task<List<ContractInvoiceDto>> SignContract(string rentingRequestId)
        {
            //var rentingRequest = await _rentingRepository.GetRentingRequestDetailById(rentingRequestId);
            //if (rentingRequest == null)
            //{
            //    throw new ServiceException(MessageConstant.RentingRequest.RentingRequestNotFound);
            //}

            //var isValidToSign = await _contractRepository.IsContractValidToSign(rentingRequestId);
            //if (!isValidToSign)
            //{
            //    throw new ServiceException(MessageConstant.Contract.ContractNotValidToSign);
            //}

            //var contractInvoices = await _contractRepository.SignContract(rentingRequestId);
            //if (contractInvoices.IsNullOrEmpty())
            //{
            //    throw new ServiceException(MessageConstant.Contract.SignContractFail);
            //}

            return null;
        }

        private async Task<ContractDetailDto> CheckContractExist(string contractId)
        {
            var contract = await _contractRepository.GetContractDetailById(contractId);

            if (contract == null)
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotFound);
            }

            return contract;
        }
    }
}
