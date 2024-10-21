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
        private readonly ISerialNumberProductRepository _serialNumberProductRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAddressRepository _addressRepository;

        public RentingRequestServiceImpl(
            IRentingRequestRepository rentingRepository,
            ISerialNumberProductRepository serialNumberProductRepository,
            IAccountRepository accountRepository,
            IAddressRepository addressRepository)
        {
            _rentingRepository = rentingRepository;
            _serialNumberProductRepository = serialNumberProductRepository;
            _accountRepository = accountRepository;
            _addressRepository = addressRepository;
        }

        public async Task<string> CreateRentingRequest(int customerId, NewRentingRequestDto newRentingRequestDto)
        {
            //Check product valid (quantity + status)
            var isProductsValid = await _serialNumberProductRepository.CheckSerialNumberProductValidToRequest(newRentingRequestDto);
            if (!isProductsValid)
            {
                throw new ServiceException(MessageConstant.RentingRequest.RequestProductsInvalid);
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

        public async Task<RentingRequestDetailDto> GetRentingRequestDetailById(string rentingRequestId)
        {
            var rentingRequest = await _rentingRepository.GetRentingRequestDetailById(rentingRequestId);
            if (rentingRequest == null)
            {
                throw new ServiceException(MessageConstant.RentingRequest.RentingRequestNotFound);
            }

            return rentingRequest;
        }

        public async Task<RentingRequestInitDataDto> InitializeRentingRequestData(int customerId, RentingRequestProductInRangeDto rentingRequestProductInRangeDto)
        {
            return await _rentingRepository.InitializeRentingRequestData(customerId, rentingRequestProductInRangeDto);
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
