using AutoMapper;
using BusinessObject;
using DAO;
using DTOs.RentingService;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;

namespace Repository.Implement
{
    public class RentingServiceRepository : IRentingServiceRepository
    {
        private readonly IMapper _mapper;

        public RentingServiceRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task CreateRentingService(RentingServiceRequestDto rentingServiceRequestDto)
        {
            var rentingService = _mapper.Map<RentingService>(rentingServiceRequestDto);

            await RentingServiceDao.Instance.CreateAsync(rentingService);
        }

        public async Task<IEnumerable<RentingServiceDto>> GetRentingServices()
        {
            var rentingServices = await RentingServiceDao.Instance.GetAllAsync();

            if (!rentingServices.IsNullOrEmpty())
            {
                return _mapper.Map<IEnumerable<RentingServiceDto>>(rentingServices);
            }

            return [];
        }

        public async Task UpdateRentingService(int rentingServiceId, RentingServiceRequestDto rentingServiceRequestDto)
        {
            var rentingService = await RentingServiceDao.Instance.GetRentingServiceById(rentingServiceId);
            if (rentingService != null)
            {
                rentingService.RentingServiceName = rentingServiceRequestDto.RentingServiceName;
                rentingService.Description = rentingServiceRequestDto.Description;
                rentingService.Price = rentingServiceRequestDto.Price;
                rentingService.PayType = rentingServiceRequestDto.PayType;
                rentingService.IsOptional = rentingServiceRequestDto.IsOptional;

                await RentingServiceDao.Instance.UpdateAsync(rentingService);
            }
        }
    }
}
