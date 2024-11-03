using Common;
using Common.Enum;
using DTOs.Contract;
using DTOs.Invoice;
using DTOs.MachineComponentStatus;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using System.Transactions;

namespace Service.Implement
{
    public class ContractServiceImpl : IContractService
    {
        private readonly IContractRepository _contractRepository;
        private readonly IRentingRequestRepository _rentingRepository;
        private readonly IMachineSerialNumberRepository _machineSerialNumberRepository;
        private readonly IInvoiceRepository _invoiceRepository;

        public ContractServiceImpl(
            IContractRepository contractRepository,
            IRentingRequestRepository rentingRepository,
            IMachineSerialNumberRepository machineSerialNumberRepository,
            IInvoiceRepository invoiceRepository)
        {
            _contractRepository = contractRepository;
            _rentingRepository = rentingRepository;
            _machineSerialNumberRepository = machineSerialNumberRepository;
            _invoiceRepository = invoiceRepository;
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

        //TODO: Calculate ActualRentPrice of machineSerialNumber
        public async Task<bool> EndContract(string contractId)
        {
            var contract = await _contractRepository.GetContractById(contractId);
            if (contract == null || !contract.Status.Equals(ContractStatusEnum.Renting.ToString()))
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotValidToEnd);
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var currentDate = DateTime.Now;
                    bool isTerminatedEarly = currentDate < contract.DateEnd;
                    string newStatus = isTerminatedEarly ? ContractStatusEnum.Terminated.ToString() : ContractStatusEnum.Completed.ToString();
                    var actualRentPeriod = (currentDate - contract.DateStart).Value.Days;

                    await _contractRepository.EndContract(contractId, newStatus, actualRentPeriod, currentDate);

                    var refundInvoice = await _invoiceRepository.CreateInvoice((double)contract.TotalRentPrice, InvoiceTypeEnum.Refund.ToString(), (int)contract.AccountSignId);

                    var machineSerialNumber = await _machineSerialNumberRepository.GetMachineSerialNumber(contract.SerialNumber);
                    if (machineSerialNumber != null)
                    {
                        var updatedRentDaysCounter = (machineSerialNumber.RentDaysCounter ?? 0) + actualRentPeriod;
                        var machineSerialNumberUpdateDto = new MachineSerialNumberUpdateDto
                        {
                            ActualRentPrice = machineSerialNumber.ActualRentPrice ?? 0,
                            RentDaysCounter = updatedRentDaysCounter
                        };

                        await _machineSerialNumberRepository.Update(contract.SerialNumber, machineSerialNumberUpdateDto);
                        await _machineSerialNumberRepository.UpdateStatus(contract.SerialNumber, MachineSerialNumberStatusEnum.Available.ToString(), (int)contract.AccountSignId);
                    }

                    scope.Complete();

                    return true;
                }
                catch (Exception ex)
                {
                    throw new ServiceException(ex.Message);
                }
            }
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
