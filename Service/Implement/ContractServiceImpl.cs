using Common;
using Common.Enum;
using DTOs.Contract;
using DTOs.Invoice;
using DTOs.MachineSerialNumber;
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

        public ContractServiceImpl(
            IContractRepository contractRepository,
            IMachineSerialNumberRepository machineSerialNumberRepository,
            IInvoiceRepository invoiceRepository)
        {
            _contractRepository = contractRepository;
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

        public async Task<bool> EndContract(string contractId, int? accountId)
        {
            var contract = await _contractRepository.GetContractById(contractId);
            if (contract == null)
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

                //Manager
                if (contract.Status.Equals(ContractStatusEnum.Signed.ToString()))
                {
                    await _contractRepository.EndContract(contractId, ContractStatusEnum.Terminated.ToString(), 0, currentDate);

                    if (accountId != null)
                    {
                        var invoice = await _invoiceRepository.CreateInvoice(contract.DepositPrice ?? 0, InvoiceTypeEnum.Refund.ToString(), (int)accountId);

                        await _contractRepository.UpdateRefundContractPayment(contract.ContractId, invoice.InvoiceId);
                    }
                }

                //Customer
                if (contract.Status.Equals(ContractStatusEnum.Renting.ToString()))
                {
                    await _contractRepository.EndContract(contractId, ContractStatusEnum.InspectionPending.ToString(), actualRentPeriod, currentDate);

                    var machineSerialNumber = await _machineSerialNumberRepository.GetMachineSerialNumber(contract.SerialNumber);
                    if (machineSerialNumber != null)
                    {
                        var updatedRentDaysCounter = (machineSerialNumber.RentDaysCounter ?? 0) + actualRentPeriod;
                        var machineSerialNumberUpdateDto = new MachineSerialNumberUpdateDto
                        {
                            ActualRentPrice = machineSerialNumber.ActualRentPrice ?? 0,
                            RentDaysCounter = updatedRentDaysCounter,
                            Status = machineSerialNumber.Status,
                        };

                        if (machineSerialNumber.Status == MachineSerialNumberStatusEnum.Renting.ToString())
                        {
                            machineSerialNumberUpdateDto.Status = MachineSerialNumberStatusEnum.Available.ToString();
                        }

                        await _machineSerialNumberRepository.UpdateMachineSerialNumber(machineSerialNumber.SerialNumber, machineSerialNumberUpdateDto, (int)contract.AccountSignId);
                    }
                }

                scope.Complete();

                return true;
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
    }
}
