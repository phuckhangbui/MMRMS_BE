using Common;
using DTOs.ComponentReplacementTicket;
using Microsoft.AspNetCore.SignalR;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using Service.SignalRHub;

namespace Service.Implement
{
    public class ComponentReplacementTicketService : IComponentReplacementTicketService
    {
        private readonly IComponentReplacementTicketRepository _componentReplacementTicketRepository;
        private readonly IComponentRepository _componentRepository;
        private readonly IMachineSerialNumberRepository _machineSerialNumberRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IHubContext<ComponentReplacementTicketHub> _ComponentReplacementTicketHub;
        private readonly INotificationService _notificationService;


        public ComponentReplacementTicketService(IComponentReplacementTicketRepository ComponentReplacementTicketRepository, IMachineSerialNumberRepository machineSerialNumberRepository, IComponentRepository componentRepository, IContractRepository contractRepository, IHubContext<ComponentReplacementTicketHub> ComponentReplacementTicketHub, INotificationService notificationService)
        {
            _componentReplacementTicketRepository = ComponentReplacementTicketRepository;
            _machineSerialNumberRepository = machineSerialNumberRepository;
            _componentRepository = componentRepository;
            _contractRepository = contractRepository;
            _ComponentReplacementTicketHub = ComponentReplacementTicketHub;
            _notificationService = notificationService;
        }

        public async Task CreateComponentReplacementTicket(int staffId, CreateComponentReplacementTicketDto createComponentReplacementTicketDto)
        {
            if (!await _componentRepository.IsComponentIdExisted(createComponentReplacementTicketDto.ComponentId))
            {
                throw new ServiceException(MessageConstant.Component.ComponentNotExisted);
            }

            if (!await _machineSerialNumberRepository.IsSerialNumberExist(createComponentReplacementTicketDto.MachineSerialNumber))
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.MachineSerialNumberNotFound);
            }

            var contract = await _contractRepository.GetContractDetailById(createComponentReplacementTicketDto.ContractId);

            if (contract == null)
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotFound);
            }

            if (DateTime.Today < contract.DateStart || DateTime.Today > contract.DateEnd)
            {
                throw new ServiceException(MessageConstant.Contract.ContractOutOfRange);
            }

            var ComponentReplacementTicketDto = await _componentReplacementTicketRepository.CreateTicket(staffId, createComponentReplacementTicketDto);

            await _notificationService.SendNotificationToCustomerWhenCreateComponentReplacementTicket((int)contract.AccountSignId, (double)ComponentReplacementTicketDto.TotalAmount, ComponentReplacementTicketDto.ComponentName);

            await _ComponentReplacementTicketHub.Clients.All.SendAsync("OnCreateComponentReplacementTicket");
        }

        public async Task<IEnumerable<ComponentReplacementTicketDto>> GetComponentReplacementTickets()
        {
            return await _componentReplacementTicketRepository.GetTickets();
        }

        public async Task<IEnumerable<ComponentReplacementTicketDto>> GetComponentReplacementTickets(int customerId)
        {
            return await _componentReplacementTicketRepository.GetTicketsByCustomerId(customerId);
        }
    }
}
