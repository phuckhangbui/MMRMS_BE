using AutoMapper;
using BusinessObject;
using Common;
using Common.Enum;
using DAO;
using DTOs.MachineCheckRequest;
using Repository.Interface;

namespace Repository.Implement
{
    public class MachineCheckRequestRepository : IMachineCheckRequestRepository
    {
        private readonly IMapper _mapper;

        public MachineCheckRequestRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        private async Task<string> GenerateCheckRequestId()
        {
            int currentTotalRequest = await MachineCheckRequestDao.Instance.GetTotalRequestByDate(DateTime.UtcNow);
            string datePart = DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern);
            string sequencePart = (currentTotalRequest + 1).ToString("D3");
            return $"{GlobalConstant.MachineCheckRequestIdPrefixPattern}{datePart}{GlobalConstant.SequenceSeparator}{sequencePart}";
        }

        public async Task CreateMachineCheckRequest(int customerId, CreateMachineCheckRequestDto createMachineCheckRequestDto)
        {
            var request = new MachineCheckRequest
            {
                MachineCheckRequestId = await GenerateCheckRequestId(),
                ContractId = createMachineCheckRequestDto.ContractId,
                Note = createMachineCheckRequestDto.Note,
                Status = MachineCheckRequestStatusEnum.New.ToString(),
                DateCreate = DateTime.Now
            };

            var machineCheckRequestCriterias = new List<MachineCheckRequestCriteria>();

            foreach (var criteria in createMachineCheckRequestDto.CheckCriterias)
            {
                var checkCriteria = new MachineCheckRequestCriteria
                {
                    CriteriaName = criteria.CriteriaName,
                    CustomerNote = criteria.CustomerNote,
                    MachineCheckRequestId = request.MachineCheckRequestId
                };

                machineCheckRequestCriterias.Add(checkCriteria);
            }


            MachineCheckRequestDao.Instance.CreateMachineCheckRequest(request, machineCheckRequestCriterias);
        }

        public async Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequests()
        {
            var list = await MachineCheckRequestDao.Instance.GetMachineCheckRequests();

            return _mapper.Map<IEnumerable<MachineCheckRequestDto>>(list);
        }

        public async Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequestsByContractId(string contractId)
        {
            var list = await MachineCheckRequestDao.Instance.GetMachineCheckRequests();

            var resultList = list.Where(c => c.ContractId.Equals(contractId)).ToList();

            return _mapper.Map<IEnumerable<MachineCheckRequestDto>>(resultList);
        }

        public async Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequestsByCustomerId(int customerId)
        {
            var list = await MachineCheckRequestDao.Instance.GetMachineCheckRequests();

            var resultList = list.Where(c => c.Contract.AccountSignId.Equals(customerId)).ToList();

            return _mapper.Map<IEnumerable<MachineCheckRequestDto>>(list);
        }

        public async Task<MachineCheckRequestDto> GetMachineCheckRequest(string MachineCheckRequestId)
        {
            var MachineCheckRequest = await MachineCheckRequestDao.Instance.GetMachineCheckRequest(MachineCheckRequestId);

            return _mapper.Map<MachineCheckRequestDto>(MachineCheckRequest);
        }


        public async Task UpdateRequestStatus(string machineCheckRequestId, string status)
        {
            var machineCheckRequest = await MachineCheckRequestDao.Instance.GetMachineCheckRequest(machineCheckRequestId);

            if (machineCheckRequest == null)
            {
                throw new Exception(MessageConstant.MachineCheckRequest.RequestNotFound);
            }

            machineCheckRequest.Status = status;

            await MachineCheckRequestDao.Instance.UpdateAsync(machineCheckRequest);
        }


        public Task UpdateRequestStatus(int MachineCheckRequestId, string status)
        {
            throw new NotImplementedException();
        }

        public async Task<MachineCheckRequestDetailDto> GetMachineCheckRequestDetail(string machineCheckRequestId)
        {
            var requestDetail = await MachineCheckRequestDao.Instance.GetMachineCheckRequestDetail(machineCheckRequestId);

            var requestDto = _mapper.Map<MachineCheckRequestDto>(requestDetail);

            var requestCriteriaList = _mapper.Map<IEnumerable<MachineCheckRequestCriteriaDto>>(requestDetail.MachineCheckRequestCriterias);

            return new MachineCheckRequestDetailDto
            {
                MachineCheckRequest = requestDto,
                CheckCriteriaList = requestCriteriaList,
            };
        }

        public async Task<IEnumerable<MachineCheckCriteriaDto>> GetMachineCheckCriteriaList()
        {
            var list = await MachineCheckCriteriaDao.Instance.GetAllAsync();

            return _mapper.Map<IEnumerable<MachineCheckCriteriaDto>>(list);
        }

        public async Task UpdateRequest(MachineCheckRequestDto requestDto)
        {
            var request = _mapper.Map<MachineCheckRequest>(requestDto);

            await MachineCheckRequestDao.Instance.UpdateAsync(request);
        }

        public async Task CreateMachineCheckCriteria(string name)
        {
            var machineCheckCriteria = new MachineCheckCriteria
            {
                Name = name
            };

            await MachineCheckCriteriaDao.Instance.CreateAsync(machineCheckCriteria);
        }

        public async Task<bool> UpdateMachineCheckCriteria(int id, string name)
        {
            var machineCheckCriterias = await MachineCheckCriteriaDao.Instance.GetAllAsync();
            var machineCheckCriteria = machineCheckCriterias.FirstOrDefault(m => m.MachineCheckCriteriaId == id);
            if (machineCheckCriteria == null)
            {
                return false;
            }

            machineCheckCriteria.Name = name;
            await MachineCheckCriteriaDao.Instance.UpdateAsync(machineCheckCriteria);

            return true;
        }

        public async Task<bool> DeleteMachineCheckCriteria(int id)
        {
            var machineCheckCriterias = await MachineCheckCriteriaDao.Instance.GetAllAsync();
            var machineCheckCriteria = machineCheckCriterias.FirstOrDefault(m => m.MachineCheckCriteriaId == id);
            if (machineCheckCriteria == null)
            {
                return false;
            }

            await MachineCheckCriteriaDao.Instance.RemoveAsync(machineCheckCriteria);

            return true;
        }
    }
}
