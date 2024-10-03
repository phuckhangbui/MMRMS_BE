using Common;
using DAO.Enum;
using DTOs.RentingRequest;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;

namespace Service.Implement
{
    public class RentingRequestServiceImpl : IRentingRequestService
    {
        private readonly IRentingRepository _rentingRepository;
        private readonly ISerialNumberProductRepository _serialNumberProductRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAddressRepository _addressRepository;

        public RentingRequestServiceImpl(
            IRentingRepository rentingRepository,
            ISerialNumberProductRepository serialNumberProductRepository,
            IAccountRepository accountRepository,
            IAddressRepository addressRepository)
        {
            _rentingRepository = rentingRepository;
            _serialNumberProductRepository = serialNumberProductRepository;
            _accountRepository = accountRepository;
            _addressRepository = addressRepository;
        }

        public async Task CreateRentingRequest(int customerId, NewRentingRequestDto newRentingRequestDto)
        {
            //Check product valid (quantity + status)
            var isProductsValid = await _serialNumberProductRepository.CheckSerialNumberProductValidToRequest(newRentingRequestDto.RentingRequestProductDetails);
            if (!isProductsValid)
            {
                throw new ServiceException(MessageConstant.RentingRequest.RequestProductsInvalid);
            }

            //Check account rent valid
            var rentAccount = await _accountRepository.GetAccounById(customerId);
            if (rentAccount == null || !rentAccount.Status!.Equals(AccountStatusEnum.Active.ToString()))
            {
                throw new ServiceException(MessageConstant.RentingRequest.RequestAccountInvalid);
            }

            //Check address valid
            var isAddressValid = await _addressRepository.CheckAddressValid(newRentingRequestDto.AddressId, customerId);
            if (!isAddressValid)
            {
                throw new ServiceException(MessageConstant.RentingRequest.RequestAddressInvalid);
            }

            await _rentingRepository.CreateRentingRequest(customerId, newRentingRequestDto);
        }

        public async Task<IEnumerable<RentingRequestDto>> GetAll()
        {
            return await _rentingRepository.GetRentingRequests();
        }

        public async Task<RentingRequestDetailDto> GetRentingRequestDetailById(string rentingRequestId)
        {
            return await _rentingRepository.GetRentingRequestDetailById(rentingRequestId);
        }

        public async Task<RentingRequestInitDataDto> GetRentingRequestInitData(int customerId, List<int> productIds)
        {
            return await _rentingRepository.GetRentingRequestInitData(customerId, productIds);
        }

        public async Task<IEnumerable<RentingRequestDto>> GetRentingRequestsForCustomer(int customerId)
        {
            return await _rentingRepository.GetRentingRequestsForCustomer(customerId);
        }
    }
}
