using AutoMapper;
using BusinessObject;
using Common.Enum;
using DAO;
using DTOs.MaintenanceRequest;
using Repository.Interface;

namespace Repository.Implement
{
    public class MaintenanceRequestRepository : IMaintenanceRequestRepository
    {
        private readonly IMapper _mapper;

        public MaintenanceRequestRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task CreateMaintenanceRequest(int customerId, CreateMaintenanceRequestDto createMaintenanceRequestDto)
        {
            var request = new MaintenanceRequest
            {
                ContractId = createMaintenanceRequestDto.ContractId,
                Note = createMaintenanceRequestDto.Note,
                Status = MaintenanceRequestStatusEnum.Processing.ToString(),
                DateCreate = DateTime.Now
            };

            await MaintenanceRequestDao.Instance.CreateAsync(request);
        }

        public async Task<IEnumerable<MaintenanceRequestDto>> GetMaintenanceRequests()
        {
            var list = await MaintenanceRequestDao.Instance.GetMaintenanceRequests();

            return _mapper.Map<IEnumerable<MaintenanceRequestDto>>(list);
        }

        public async Task<IEnumerable<MaintenanceRequestDto>> GetMaintenanceRequestsByContractId(string contractId)
        {
            var list = await MaintenanceRequestDao.Instance.GetMaintenanceRequests();

            var resultList = list.Where(c => c.ContractId.Equals(contractId)).ToList();

            return _mapper.Map<IEnumerable<MaintenanceRequestDto>>(list);
        }

        public async Task<IEnumerable<MaintenanceRequestDto>> GetMaintenanceRequestsByCustomerId(int customerId)
        {
            var list = await MaintenanceRequestDao.Instance.GetMaintenanceRequests();

            var resultList = list.Where(c => c.Contract.AccountSignId.Equals(customerId)).ToList();

            return _mapper.Map<IEnumerable<MaintenanceRequestDto>>(list);
        }

        public async Task<MaintenanceRequestDto> GetMaintenanceRequest(string maintenanceRequestId)
        {
            var maintenanceRequest = await MaintenanceRequestDao.Instance.GetMaintenanceRequest(maintenanceRequestId);

            return _mapper.Map<MaintenanceRequestDto>(maintenanceRequest);
        }


        public async Task UpdateRequestStatus(string maintenanceRequestId, string status)
        {
            var maintenanceRequest = await MaintenanceRequestDao.Instance.GetMaintenanceRequest(maintenanceRequestId);

            maintenanceRequest.Status = status;

            await MaintenanceRequestDao.Instance.UpdateAsync(maintenanceRequest);
        }


        public Task UpdateRequestStatus(int maintenanceRequestId, string status)
        {
            throw new NotImplementedException();
        }
    }
}
