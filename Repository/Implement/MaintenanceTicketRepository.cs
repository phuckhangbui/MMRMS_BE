using AutoMapper;
using BusinessObject;
using Common.Enum;
using DAO;
using DTOs.MaintenanceTicket;
using Repository.Interface;

namespace Repository.Implement
{
    public class MaintenanceTicketRepository : IMaintenanceTicketRepository
    {
        private readonly IMapper _mapper;

        public MaintenanceTicketRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<MaintenanceTicketDto> CreateTicket(int staffId, CreateMaintenanceTicketDto createMaintenanceTicketDto)
        {
            var maintenanceTicket = new MaintenanceTicket
            {
                EmployeeCreateId = staffId,
                DateCreate = DateTime.Now,
                ComponentId = createMaintenanceTicketDto.ComponentId,
                ProductSerialNumber = createMaintenanceTicketDto.ProductSerialNumber,
                ComponentPrice = createMaintenanceTicketDto.ComponentPrice,
                AdditionalFee = createMaintenanceTicketDto.AdditionalFee,
                Type = createMaintenanceTicketDto.Type,
                Note = createMaintenanceTicketDto.Note,
                Quantity = createMaintenanceTicketDto.Quantity,
            };

            maintenanceTicket.TotalAmount = maintenanceTicket.ComponentPrice + maintenanceTicket.AdditionalFee;
            maintenanceTicket.Status = MaintenanceTicketStatusEnum.Created.ToString();

            maintenanceTicket = await MaintenanceTicketDao.Instance.CreateAsync(maintenanceTicket);

            return _mapper.Map<MaintenanceTicketDto>(maintenanceTicket);
        }

        public async Task<IEnumerable<MaintenanceTicketDto>> GetTickets()
        {
            var list = await MaintenanceTicketDao.Instance.GetMaintenanceTickets();

            return _mapper.Map<IEnumerable<MaintenanceTicketDto>>(list);
        }

        public async Task<IEnumerable<MaintenanceTicketDto>> GetTicketsByCustomerId(int customerId)
        {
            var list = await MaintenanceTicketDao.Instance.GetMaintenanceTickets();

            var resultList = list.Where(c => c.Contract?.AccountSignId == customerId).ToList();

            return _mapper.Map<IEnumerable<MaintenanceTicketDto>>(list);
        }
    }
}
