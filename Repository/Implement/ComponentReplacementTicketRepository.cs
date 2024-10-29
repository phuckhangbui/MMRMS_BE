using AutoMapper;
using BusinessObject;
using Common;
using Common.Enum;
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

        public async Task<ComponentReplacementTicketDto> CreateTicket(int staffId, ComponentReplacementTicketDto componentReplacementTicketDto, int? accountSignId)
        {
            var componentTicket = _mapper.Map<ComponentReplacementTicket>(componentReplacementTicketDto);

            var time = componentTicket.DateCreate ?? DateTime.Now;

            var ticketLog = new ComponentReplacementTicketLog
            {
                AccountTriggerId = staffId,
                DateCreate = time,
                Action = "Ticket được tạo mới",
            };


            var invoice = new Invoice
            {
                InvoiceId = GlobalConstant.InvoiceIdPrefixPattern + "TICKET" + time.ToString(GlobalConstant.DateTimeFormatPattern),
                AccountPaidId = accountSignId,
                Amount = componentReplacementTicketDto.TotalAmount,
                DateCreate = time,
                Type = InvoiceTypeEnum.ComponentTicket.ToString(),
                Status = InvoiceStatusEnum.Pending.ToString()
            };

            await ComponentReplacementTicketDao.Instance.CreateTicket(componentTicket, ticketLog, invoice);

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
