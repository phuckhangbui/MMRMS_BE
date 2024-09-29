using Common;
using DTOs.RentingService;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;

namespace Service.Implement
{
    public class RentingServiceServiceImpl : IRentingServiceService
    {
        private readonly IRentingServiceRepository _rentingServiceRepository;

        public RentingServiceServiceImpl(IRentingServiceRepository rentingServiceRepository)
        {
            _rentingServiceRepository = rentingServiceRepository;
        }

        public async Task CreateRentingService(RentingServiceRequestDto rentingServiceRequestDto)
        {
            await _rentingServiceRepository.CreateRentingService(rentingServiceRequestDto);
        }

        public async Task<IEnumerable<RentingServiceDto>> GetRentingServices()
        {
            var rentingServices = await _rentingServiceRepository.GetRentingServices();

            if (rentingServices.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.RentingService.RentingServiceListEmpty);
            }

            return rentingServices;
        }

        public async Task UpdateRentingService(int rentingServiceId, RentingServiceRequestDto rentingServiceRequestDto)
        {
            await _rentingServiceRepository.UpdateRentingService(rentingServiceId, rentingServiceRequestDto);
        }
    }
}
