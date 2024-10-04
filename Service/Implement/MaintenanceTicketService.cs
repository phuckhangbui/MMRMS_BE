using Common;
using DTOs.MaintenanceTicket;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;

namespace Service.Implement
{
    public class MaintenanceTicketService : IMaintenanceTicketService
    {
        private readonly IMaintenanceTicketRepository _maintenanceTicketRepository;

        private readonly IComponentRepository _componentRepository;

        private readonly ISerialNumberProductRepository _serialNumberProductRepository;

        private readonly IContractRepository _contractRepository;

        public MaintenanceTicketService(IMaintenanceTicketRepository maintenanceTicketRepository, ISerialNumberProductRepository serialNumberProductRepository, IComponentRepository componentRepository, IContractRepository contractRepository)
        {
            _maintenanceTicketRepository = maintenanceTicketRepository;
            _serialNumberProductRepository = serialNumberProductRepository;
            _componentRepository = componentRepository;
            _contractRepository = contractRepository;
        }

        public async Task CreateMaintenanceTicket(int staffId, CreateMaintenanceTicketDto createMaintenanceTicketDto)
        {
            if (!await _componentRepository.IsComponentIdExisted(createMaintenanceTicketDto.ComponentId))
            {
                throw new ServiceException(MessageConstant.Component.ComponentNotExisted);
            }

            if (!await _serialNumberProductRepository.IsSerialNumberExist(createMaintenanceTicketDto.ProductSerialNumber))
            {
                throw new ServiceException(MessageConstant.SerialNumberProduct.SerialNumberProductNotFound);
            }

            var contract = await _contractRepository.GetContractDetailById(createMaintenanceTicketDto.ContractId);

            if (contract == null)
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotFound);
            }

            if (DateTime.Today < contract.DateStart || DateTime.Today > contract.DateEnd)
            {
                throw new ServiceException(MessageConstant.Contract.ContractOutOfRange);
            }

            await _maintenanceTicketRepository.CreateTicket(staffId, createMaintenanceTicketDto);
        }

        public async Task<IEnumerable<MaintenanceTicketDto>> GetMaintenanceTickets()
        {
            return await _maintenanceTicketRepository.GetTickets();
        }

        public async Task<IEnumerable<MaintenanceTicketDto>> GetMaintenanceTickets(int customerId)
        {
            return await _maintenanceTicketRepository.GetTicketsByCustomerId(customerId);
        }
    }
}
