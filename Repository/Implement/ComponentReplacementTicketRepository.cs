using AutoMapper;
using BusinessObject;
using Common;
using Common.Enum;
using DAO;
using DTOs.ComponentReplacementTicket;
using Repository.Interface;
using ComponentReplacementTicket = BusinessObject.ComponentReplacementTicket;
using Invoice = BusinessObject.Invoice;

namespace Repository.Implement
{
    public class ComponentReplacementTicketRepository : IComponentReplacementTicketRepository
    {
        private readonly IMapper _mapper;

        public ComponentReplacementTicketRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        private async Task<string> GenerateTicketId()
        {
            int currentTicketId = await ComponentReplacementTicketDao.Instance.GetTotalTicketByDate(DateTime.UtcNow);
            string datePart = DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern);
            string sequencePart = (currentTicketId + 1).ToString("D3");
            return $"{GlobalConstant.ComponentReplacementTicketIdPrefixPattern}{datePart}{GlobalConstant.SequenceSeparator}{sequencePart}";
        }

        private async Task<string> GenerateInvoiceId()
        {
            int currentInvoiceId = await InvoiceDao.Instance.GetTotalInvoiceByDate(DateTime.UtcNow);
            string datePart = DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern);
            string sequencePart = (currentInvoiceId + 1).ToString("D3");
            return $"{GlobalConstant.InvoiceIdPrefixPattern}{datePart}{GlobalConstant.SequenceSeparator}{sequencePart}";
        }
        public async Task<ComponentReplacementTicketDto> CreateTicket(int staffId, ComponentReplacementTicketDto componentReplacementTicketDto, int? accountSignId)
        {
            var componentTicket = _mapper.Map<ComponentReplacementTicket>(componentReplacementTicketDto);

            var time = componentTicket.DateCreate ?? DateTime.Now;
            componentTicket.ComponentReplacementTicketId = await GenerateTicketId();

            var ticketLog = new ComponentReplacementTicketLog
            {
                ComponentReplacementTicketId = componentTicket.ComponentReplacementTicketId,
                AccountTriggerId = staffId,
                DateCreate = time,
                Action = "Ticket được tạo mới",
            };


            var invoice = new Invoice
            {
                InvoiceId = await GenerateInvoiceId(),
                ComponentReplacementTicketId = componentTicket.ComponentReplacementTicketId,
                AccountPaidId = accountSignId,
                Amount = componentReplacementTicketDto.TotalAmount,
                DateCreate = time,
                Type = InvoiceTypeEnum.ComponentTicket.ToString(),
                Status = InvoiceStatusEnum.Pending.ToString()
            };

            componentTicket.InvoiceId = invoice.InvoiceId;

            componentTicket = await ComponentReplacementTicketDao.Instance.CreateTicket(componentTicket, ticketLog);

            await InvoiceDao.Instance.CreateAsync(invoice);

            var result = _mapper.Map<ComponentReplacementTicketDto>(componentTicket);

            return result;
        }

        public async Task<ComponentReplacementTicketDetailDto> GetComponentReplacementTicketDetail(string replacementTicketId)
        {
            var ticket = await ComponentReplacementTicketDao.Instance.GetComponentReplacementTicketDetail(replacementTicketId);

            if (ticket == null)
            {
                throw new Exception(MessageConstant.ComponentReplacementTicket.TicketNotFound);
            }

            var ticketDto = _mapper.Map<ComponentReplacementTicketDto>(ticket);

            var logs = _mapper.Map<IEnumerable<ComponentReplacementTicketLogDto>>(ticket.ComponentReplacementTicketLogs);

            return new ComponentReplacementTicketDetailDto
            {
                ComponentReplacementTicket = ticketDto,
                Logs = logs
            };
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

        public async Task UpdateTicketStatus(string componentTicketId, string status, int accountId)
        {
            var ticket = await ComponentReplacementTicketDao.Instance.GetComponentReplacementTicket(componentTicketId);

            if (ticket == null)
            {
                throw new Exception(MessageConstant.ComponentReplacementTicket.TicketNotFound);
            }

            string oldStatus = ticket.Status;

            ticket.Status = status;

            await ComponentReplacementTicketDao.Instance.UpdateAsync(ticket);

            string action = $"Thay đổi trạng thái từ [{EnumExtensions.TranslateStatus<ComponentReplacementTicketStatusEnum>(oldStatus)}] trở thành [{EnumExtensions.TranslateStatus<ComponentReplacementTicketStatusEnum>(status)}]";

            var ticketLog = new ComponentReplacementTicketLog
            {
                ComponentReplacementTicketId = componentTicketId,
                AccountTriggerId = accountId,
                DateCreate = DateTime.Now,
                Action = action,
            };

            await ComponentReplacementicketLogDao.Instance.CreateAsync(ticketLog);
        }

        public async Task UpdateTicketStatusToComplete(string componentReplacementTicketId, int staffId)
        {
            var ticket = await ComponentReplacementTicketDao.Instance.GetComponentReplacementTicket(componentReplacementTicketId);

            if (ticket == null)
            {
                throw new Exception(MessageConstant.ComponentReplacementTicket.TicketNotFound);
            }

            string status = ComponentReplacementTicketStatusEnum.Completed.ToString();

            string oldStatus = ticket.Status;

            ticket.Status = status;

            await ComponentReplacementTicketDao.Instance.UpdateAsync(ticket);

            string action = $"Thay đổi trạng thái từ [{EnumExtensions.TranslateStatus<ComponentReplacementTicketStatusEnum>(oldStatus)}] trở thành [{EnumExtensions.TranslateStatus<ComponentReplacementTicketStatusEnum>(status)}]";

            var ticketLog = new ComponentReplacementTicketLog
            {
                ComponentReplacementTicketId = componentReplacementTicketId,
                AccountTriggerId = staffId,
                DateCreate = DateTime.Now,
                Action = action,
            };

            await ComponentReplacementicketLogDao.Instance.CreateAsync(ticketLog);
        }
    }
}
