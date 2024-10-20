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
        private readonly IComponentReplacementTicketRepository _ComponentReplacementTicketRepository;
        private readonly IComponentRepository _componentRepository;
        private readonly ISerialNumberProductRepository _serialNumberProductRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IHubContext<ComponentReplacementTicketHub> _ComponentReplacementTicketHub;
        private readonly INotificationService _notificationService;


        public ComponentReplacementTicketService(IComponentReplacementTicketRepository ComponentReplacementTicketRepository, ISerialNumberProductRepository serialNumberProductRepository, IComponentRepository componentRepository, IContractRepository contractRepository, IHubContext<ComponentReplacementTicketHub> ComponentReplacementTicketHub, INotificationService notificationService)
        {
            _ComponentReplacementTicketRepository = ComponentReplacementTicketRepository;
            _serialNumberProductRepository = serialNumberProductRepository;
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

            if (!await _serialNumberProductRepository.IsSerialNumberExist(createComponentReplacementTicketDto.ProductSerialNumber))
            {
                throw new ServiceException(MessageConstant.SerialNumberProduct.SerialNumberProductNotFound);
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

            var ComponentReplacementTicketDto = await _ComponentReplacementTicketRepository.CreateTicket(staffId, createComponentReplacementTicketDto);

            await _notificationService.SendNotificationToCustomerWhenCreateComponentReplacementTicket((int)contract.AccountSignId, (double)ComponentReplacementTicketDto.TotalAmount, ComponentReplacementTicketDto.ComponentName);

            await _ComponentReplacementTicketHub.Clients.All.SendAsync("OnCreateComponentReplacementTicket");
        }

        public async Task<IEnumerable<ComponentReplacementTicketDto>> GetComponentReplacementTickets()
        {
            return await _ComponentReplacementTicketRepository.GetTickets();
        }

        public async Task<IEnumerable<ComponentReplacementTicketDto>> GetComponentReplacementTickets(int customerId)
        {
            return await _ComponentReplacementTicketRepository.GetTicketsByCustomerId(customerId);
        }
    }
}
