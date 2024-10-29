using AutoMapper;
using DAO;
using DTOs.ComponentReplacementTicket;
using Repository.Interface;

namespace Repository.Implement
{
    public class ComponentReplacementTicketRepository : IComponentReplacementTicketRepository
    {
        private readonly IMapper _mapper;

        public ComponentReplacementTicketRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ComponentReplacementTicketDto> CreateTicket(int staffId, CreateComponentReplacementTicketDto createComponentReplacementTicketDto)
        {
            //var ComponentReplacementTicket = new ComponentReplacementTicket
            //{
            //    EmployeeCreateId = staffId,
            //    DateCreate = DateTime.Now,
            //    ComponentId = createComponentReplacementTicketDto.ComponentId,
            //    SerialNumber = createComponentReplacementTicketDto.MachineSerialNumber,
            //    ComponentPrice = createComponentReplacementTicketDto.ComponentPrice,
            //    AdditionalFee = createComponentReplacementTicketDto.AdditionalFee,
            //    Type = createComponentReplacementTicketDto.Type,
            //    Note = createComponentReplacementTicketDto.Note,
            //    Quantity = createComponentReplacementTicketDto.Quantity,
            //};

            //ComponentReplacementTicket.TotalAmount = ComponentReplacementTicket.ComponentPrice + ComponentReplacementTicket.AdditionalFee;
            //ComponentReplacementTicket.Status = ComponentReplacementTicketStatusEnum.Unpaid.ToString();

            //ComponentReplacementTicket = await ComponentReplacementTicketDao.Instance.CreateAsync(ComponentReplacementTicket);

            //return _mapper.Map<ComponentReplacementTicketDto>(ComponentReplacementTicket);

            return new ComponentReplacementTicketDto();
        }

        public async Task<ComponentReplacementTicketDto> GetTicket(string ComponentReplacementTicketId)
        {
            var list = await ComponentReplacementTicketDao.Instance.GetComponentReplacementTickets();

            var result = list.FirstOrDefault(t => t.ComponentReplacementTicketId == ComponentReplacementTicketId);

            return _mapper.Map<ComponentReplacementTicketDto>(result);
        }

        public async Task<IEnumerable<ComponentReplacementTicketDto>> GetTickets()
        {
            var list = await ComponentReplacementTicketDao.Instance.GetComponentReplacementTickets();

            return _mapper.Map<IEnumerable<ComponentReplacementTicketDto>>(list);
        }

        public async Task<IEnumerable<ComponentReplacementTicketDto>> GetTicketsByCustomerId(int customerId)
        {
            var list = await ComponentReplacementTicketDao.Instance.GetComponentReplacementTickets();

            var resultList = list.Where(c => c.Contract?.AccountSignId == customerId).ToList();

            return _mapper.Map<IEnumerable<ComponentReplacementTicketDto>>(list);
        }
    }
}
