using Common;
using DTOs.MaintenanceTicket;
using Microsoft.AspNetCore.SignalR;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using Service.SignalRHub;

namespace Service.Implement
{
    public class MaintenanceTicketService : IMaintenanceTicketService
    {
        private readonly IMaintenanceTicketRepository _maintenanceTicketRepository;
        private readonly IComponentRepository _componentRepository;
        private readonly ISerialNumberProductRepository _serialNumberProductRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IHubContext<MaintenanceTicketHub> _maintenanceTicketHub;
        private readonly INotificationService _notificationService;


        public MaintenanceTicketService(IMaintenanceTicketRepository maintenanceTicketRepository, ISerialNumberProductRepository serialNumberProductRepository, IComponentRepository componentRepository, IContractRepository contractRepository, IHubContext<MaintenanceTicketHub> maintenanceTicketHub, INotificationService notificationService)
        {
            _maintenanceTicketRepository = maintenanceTicketRepository;
            _serialNumberProductRepository = serialNumberProductRepository;
            _componentRepository = componentRepository;
            _contractRepository = contractRepository;
            _maintenanceTicketHub = maintenanceTicketHub;
            _notificationService = notificationService;
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

            var maintenanceTicketDto = await _maintenanceTicketRepository.CreateTicket(staffId, createMaintenanceTicketDto);

            await _notificationService.SendNotificationToCustomerWhenCreateMaintenanceTicket((int)contract.AccountSignId, (double)maintenanceTicketDto.TotalAmount, maintenanceTicketDto.ComponentName);

            await _maintenanceTicketHub.Clients.All.SendAsync("OnCreateMaintenanceTicket");
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
