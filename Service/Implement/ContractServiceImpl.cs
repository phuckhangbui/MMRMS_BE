using Common;
using Common.Enum;
using DTOs.Contract;
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
        private readonly INotificationService _notificationService;
        private readonly IAccountRepository _accountRepository;
        private readonly ISettingsService _settingsService;
        private readonly IMachineRepository _machineRepository;

        public ContractServiceImpl(
            IContractRepository contractRepository,
            IMachineSerialNumberRepository machineSerialNumberRepository,
            IInvoiceRepository invoiceRepository,
            IBackground background,
            INotificationService notificationService,
            IAccountRepository accountRepository,
            ISettingsService settingsService,
            IMachineRepository machineRepository)
        {
            _contractRepository = contractRepository;
            _machineSerialNumberRepository = machineSerialNumberRepository;
            _invoiceRepository = invoiceRepository;
            _background = background;
            _notificationService = notificationService;
            _accountRepository = accountRepository;
            _settingsService = settingsService;
            _machineRepository = machineRepository;
        }

        public async Task<ContractDetailDto> GetContractDetail(string contractId, int accountId)
        {
            var contract = await CheckContractExist(contractId);

            var account = await _accountRepository.GetAccounById(accountId);
            if (account != null && account.RoleId == (int)AccountRoleEnum.Customer)
            {
                if (account.AccountId != contract.AccountSignId)
                {
                    throw new ServiceException(MessageConstant.Contract.ContractNotBelongToThisAccount);
                }
            }

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

        public async Task<bool> EndContract(int accountId, string contractId)
        {
            var contract = await _contractRepository.GetContractById(contractId);
            if (contract == null)
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotFound);
            }

            if (contract.AccountSignId != accountId)
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotBelongToCurrentAccount);
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

                //MachineSerialNumber
                await UpdateRentDaysCounterMachineSerialNumber(contract.SerialNumber, actualRentPeriod, (int)contract.AccountSignId);

                //Extend contract
                var extendContract = await _contractRepository.GetExtendContract(contractId);
                if (extendContract != null)
                {
                    await _contractRepository.EndContract(extendContract.ContractId, ContractStatusEnum.Canceled.ToString(), 0, currentDate);
                }

                await _notificationService.SendNotificationToManagerWhenCustomerEndContract(contractId, contractId);

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

        public async Task<bool> ExtendContract(int accountId, string contractId, ContractExtendDto contractExtendDto)
        {
            var baseContract = await _contractRepository.GetContractById(contractId);
            if (baseContract == null)
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotFound);
            }

            if (baseContract.AccountSignId != accountId)
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotBelongToCurrentAccount);
            }

            if (!baseContract.Status.Equals(ContractStatusEnum.Renting.ToString()))
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotValidToExtend);
            }

            if (baseContract.IsExtended == true)
            {
                throw new ServiceException(MessageConstant.Contract.ContractAlreadyExtended);
            }

            if (contractExtendDto.DateEnd < baseContract.DateEnd.Value.AddDays(30))
            {
                throw new ServiceException(MessageConstant.Contract.ExtensionPeriodNotValid);
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var contractDto = await _contractRepository.ExtendContract(contractId, contractExtendDto);

                if (contractDto != null)
                {
                    var invoice = await _invoiceRepository.CreateInvoice((double)contractDto.TotalRentPrice, InvoiceTypeEnum.Rental.ToString(), (int)contractDto.AccountSignId, string.Empty, null);

                    await _contractRepository.SetInvoiceForContractPayment(contractDto.ContractId, invoice.InvoiceId, ContractPaymentTypeEnum.Extend.ToString());

                    TimeSpan timeUntilStart = (TimeSpan)(contractDto.DateStart - DateTime.Now);
                    _background.ProcessExtendContractJob(contractDto.ContractId, timeUntilStart);

                    await _notificationService.SendNotificationToManagerWhenCustomerExtendContract(contractId, contractId);

                    scope.Complete();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new ServiceException(ex.Message);
            }
        }

        public async Task<IEnumerable<ContractDto>> GetRentalHistoryOfSerialNumber(string serialNumber)
        {
            var machineSerialNumber = await _machineSerialNumberRepository.GetMachineSerialNumber(serialNumber);
            if (machineSerialNumber == null)
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.MachineSerialNumberNotFound);
            }

            return await _contractRepository.GetRentalHistoryOfSerialNumber(serialNumber);
        }

        private async Task UpdateRentDaysCounterMachineSerialNumber(string serialNumber, int actualRentPeriod, int accountId)
        {
            var machineSerialNumber = await _machineSerialNumberRepository.GetMachineSerialNumber(serialNumber);
            if (machineSerialNumber != null)
            {
                machineSerialNumber.RentDaysCounter = (machineSerialNumber.RentDaysCounter ?? 0) + actualRentPeriod;

                var machineSetting = await _settingsService.GetMachineSettingsAsync();
                var condition = machineSetting.DaysData
                    .OrderByDescending(d => d.RentedDays)
                    .FirstOrDefault(d => machineSerialNumber.RentDaysCounter >= d.RentedDays);

                if (condition != null)
                {
                    machineSerialNumber.MachineConditionPercent = condition.MachineConditionPercent;
                }

                if (machineSerialNumber.MachineConditionPercent.HasValue)
                {
                    var rate = machineSetting.RateData
                        .OrderByDescending(r => r.MachineConditionPercent)
                        .FirstOrDefault(r => machineSerialNumber.MachineConditionPercent >= r.MachineConditionPercent);

                    var machine = await _machineRepository.GetMachine((int)machineSerialNumber.MachineId);

                    if (machine != null && rate != null)
                    {
                        machineSerialNumber.ActualRentPrice = machine.RentPrice * (rate.RentalPricePercent / 100.0);
                    }
                }

                await _machineSerialNumberRepository.UpdateRentDaysCounterMachineSerialNumber(machineSerialNumber, accountId);
            }
        }

        public async Task CancelExtendContract(string extendContractId)
        {
            var extendContract = await _contractRepository.GetContractById(extendContractId);
            if (extendContract == null)
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotFound);
            }

            if (!extendContract.Status!.Equals(ContractStatusEnum.NotSigned.ToString())) 
            {
                throw new ServiceException(MessageConstant.Contract.ExtendContractNotValidToCancel);
            }

            if (extendContract.BaseContractId == null)
            {
                throw new ServiceException(MessageConstant.Contract.ExtendContractNotValidToCancel);
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                await _contractRepository.CancelExtendContractWhenNotSigned(extendContractId);
                scope.Complete();
            }
            catch (Exception ex)
            {
                throw new ServiceException(ex.Message);
            }
        }
    }
}
