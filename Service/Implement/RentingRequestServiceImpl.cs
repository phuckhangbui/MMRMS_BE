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
        private readonly IProductRepository _productRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAddressRepository _addressRepository;

        public RentingRequestServiceImpl(
            IRentingRepository rentingRepository,
            IProductRepository productRepository,
            IAccountRepository accountRepository,
            IAddressRepository addressRepository)
        {
            _rentingRepository = rentingRepository;
            _productRepository = productRepository;
            _accountRepository = accountRepository;
            _addressRepository = addressRepository;
        }

        public async Task CreateRentingRequest(NewRentingRequestDto newRentingRequestDto)
        {
            //Check product valid (quantity + status)
            var isProductsValid = await _productRepository.CheckProductValidToRent(newRentingRequestDto.RentingRequestProductDetails);
            if (!isProductsValid)
            {
                throw new ServiceException(MessageConstant.RentingRequest.RequestProductsInvalid);
            }

            //Check account rent valid
            var rentAccount = await _accountRepository.GetAccounById(newRentingRequestDto.AccountOrderId);
            if (rentAccount == null || !rentAccount.Status!.Equals(AccountStatusEnum.Active.ToString()))
            {
                throw new ServiceException(MessageConstant.RentingRequest.RequestAccountInvalid);
            }

            //Check address valid
            var isAddressValid = await _addressRepository.CheckAddressValid(newRentingRequestDto.AddressId, newRentingRequestDto.AccountOrderId);
            if (!isAddressValid)
            {
                throw new ServiceException(MessageConstant.RentingRequest.RequestAddressInvalid);
            }

            await _rentingRepository.CreateRentingRequest(newRentingRequestDto);
        }

        public async Task<IEnumerable<RentingRequestDto>> GetAll()
        {
            return await _rentingRepository.GetRentingRequests();
        }
    }
}
