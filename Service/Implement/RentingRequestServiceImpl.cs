using Common;
using DTOs.RentingRequest;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;

namespace Service.Implement
{
    public class RentingRequestServiceImpl : IRentingRequestService
    {
        private readonly IRentingRequestRepository _rentingRepository;
        private readonly IMachineSerialNumberRepository _machineSerialNumberRepository;
        private readonly IAddressRepository _addressRepository;

        public RentingRequestServiceImpl(
            IRentingRequestRepository rentingRepository,
            IMachineSerialNumberRepository machineSerialNumberRepository,
            IAddressRepository addressRepository)
        {
            _rentingRepository = rentingRepository;
            _machineSerialNumberRepository = machineSerialNumberRepository;
            _addressRepository = addressRepository;
        }

        public async Task<string> CreateRentingRequest(int customerId, NewRentingRequestDto newRentingRequestDto)
        {
            //Check product valid (quantity + status)
            var isMachinesValid = await _machineSerialNumberRepository.CheckMachineSerialNumberValidToRequest(newRentingRequestDto);
            if (!isMachinesValid)
            {
                throw new ServiceException(MessageConstant.RentingRequest.RequestMachinesInvalid);
            }

            //Check address valid
            var isAddressValid = await _addressRepository.IsAddressValid(newRentingRequestDto.AddressId, customerId);
            if (!isAddressValid)
            {
                throw new ServiceException(MessageConstant.RentingRequest.RequestAddressInvalid);
            }

            return await _rentingRepository.CreateRentingRequest(customerId, newRentingRequestDto);
        }

        public async Task<IEnumerable<RentingRequestDto>> GetAll()
        {
            return await _rentingRepository.GetRentingRequests();
        }

        public async Task<RentingRequestDetailDto> GetRentingRequestDetail(string rentingRequestId)
        {
            var rentingRequest = await _rentingRepository.GetRentingRequestDetailById(rentingRequestId);
            if (rentingRequest == null)
            {
                throw new ServiceException(MessageConstant.RentingRequest.RentingRequestNotFound);
            }

            return rentingRequest;
        }

        public async Task<RentingRequestInitDataDto> InitializeRentingRequestData(int customerId, RentingRequestMachineInRangeDto rentingRequestMachineInRangeDto)
        {
            return await _rentingRepository.InitializeRentingRequestData(customerId, rentingRequestMachineInRangeDto);
        }

        public async Task<IEnumerable<RentingRequestDto>> GetRentingRequestsForCustomer(int customerId)
        {
            return await _rentingRepository.GetRentingRequestsForCustomer(customerId);
        }

        public async Task<bool> CancelRentingRequest(string rentingRequestId)
        {
            var isValid = await _rentingRepository.IsRentingRequestValidToCancel(rentingRequestId);
            if (!isValid)
            {
                throw new ServiceException(MessageConstant.RentingRequest.RentingRequestCanNotCancel);
            }

            return await _rentingRepository.CancelRentingRequest(rentingRequestId);
        }
    }
}
