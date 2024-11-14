using Common;
using Common.Enum;
using DTOs.Contract;
using DTOs.Invoice;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using System.Transactions;

namespace Service.Implement
{
    public class ContractServiceImpl : IContractService
    {
        private readonly IContractRepository _contractRepository;
        private readonly IMachineSerialNumberRepository _machineSerialNumberRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IBackground _background;

        public ContractServiceImpl(
            IContractRepository contractRepository,
            IMachineSerialNumberRepository machineSerialNumberRepository,
            IInvoiceRepository invoiceRepository,
            IBackground background)
        {
            _contractRepository = contractRepository;
            _machineSerialNumberRepository = machineSerialNumberRepository;
            _invoiceRepository = invoiceRepository;
            _background = background;
        }

        public async Task<ContractDetailDto> GetContractDetailById(string contractId)
        {
            var contract = await CheckContractExist(contractId);

            return contract;
        }

        public async Task<IEnumerable<ContractDetailDto>> GetContractDetailListByRentingRequestId(string rentingRequestId)
        {
            return await _contractRepository.GetContractDetailListByRentingRequestId(rentingRequestId);
        }

        public async Task<IEnumerable<ContractDto>> GetContracts(string? status)
        {
            if (!string.IsNullOrEmpty(status) && !Enum.IsDefined(typeof(ContractStatusEnum), status))
            {
                throw new ServiceException(MessageConstant.InvalidStatusValue);
            }

            return await _contractRepository.GetContracts(status);
        }

        public async Task<IEnumerable<ContractDto>> GetContractsForCustomer(int customerId, string? status)
        {
            if (!string.IsNullOrEmpty(status) && !Enum.IsDefined(typeof(ContractStatusEnum), status))
            {
                throw new ServiceException(MessageConstant.InvalidStatusValue);
            }

            return await _contractRepository.GetContractsForCustomer(customerId, status);
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

        public async Task<bool> EndContract(string contractId)
        {
            var contract = await _contractRepository.GetContractById(contractId);
            if (contract == null)
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotFound);
            }

            if (contract.Status != ContractStatusEnum.Renting.ToString())
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotValidToEnd);
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var currentDate = DateTime.Now.Date;
                var actualRentPeriod = (currentDate - contract.DateStart.Value.Date).Days;
                if (actualRentPeriod < 0)
                {
                    actualRentPeriod = 0;
                }

                var updatedContract = await _contractRepository.EndContract(contractId, ContractStatusEnum.InspectionPending.ToString(), actualRentPeriod, currentDate);

                await _machineSerialNumberRepository.UpdateRentDaysCounterMachineSerialNumber(contract.SerialNumber, actualRentPeriod);

                //Extend contract
                var extendContract = await _contractRepository.GetExtendContract(contractId);
                if (extendContract != null)
                {
                    await _contractRepository.EndContract(extendContract.ContractId, ContractStatusEnum.Canceled.ToString(), 0, currentDate);
                }

                scope.Complete();

                return updatedContract.Status.Equals(ContractStatusEnum.InspectionPending.ToString());
            }
            catch (Exception ex)
            {
                throw new ServiceException(ex.Message);
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

        public async Task<bool> ExtendContract(string contractId, ContractExtendDto contractExtendDto)
        {
            var baseContract = await _contractRepository.GetContractById(contractId);
            if (baseContract == null)
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotFound);
            }

            if (!baseContract.Status.Equals(ContractStatusEnum.Renting.ToString()))
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotValidToExtend);
            }

            var extendContract = await _contractRepository.GetExtendContract(contractId);
            if (extendContract != null)
            {
                throw new ServiceException(MessageConstant.Contract.ContractAlreadyExtended);
            }

            //if (contractExtendDto.DateStart < baseContract.DateEnd)
            //{
            //    throw new ServiceException(MessageConstant.Contract.ExtensionStartDateNotValid);
            //}

            if (contractExtendDto.DateEnd < baseContract.DateStart.Value.AddDays(30))
            {
                throw new ServiceException(MessageConstant.Contract.ExtensionPeriodNotValid);
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var contractDto = await _contractRepository.ExtendContract(contractId, contractExtendDto);

                if (contractDto != null)
                {
                    var invoice = await _invoiceRepository.CreateInvoice((double)contractDto.TotalRentPrice, InvoiceTypeEnum.Rental.ToString(), (int)contractDto.AccountSignId);

                    await _contractRepository.SetInvoiceForContractPayment(contractDto.ContractId, invoice.InvoiceId, ContractPaymentTypeEnum.Extend.ToString());

                    TimeSpan timeUntilEnd = (TimeSpan)(contractDto.DateEnd - DateTime.Now);
                    _background.ProcessExtendContractJob(contractDto.ContractId, timeUntilEnd);
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
}
