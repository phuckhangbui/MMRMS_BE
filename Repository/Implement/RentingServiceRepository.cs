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

        public async Task<bool> CanDeleteRentingService(int rentingServiceId)
        {
            return await RentingServiceDao.Instance.CanDeleteRentingService(rentingServiceId);
        }

        public async Task CreateRentingService(RentingServiceRequestDto rentingServiceRequestDto)
        {
            var rentingService = _mapper.Map<RentingService>(rentingServiceRequestDto);

            await RentingServiceDao.Instance.CreateAsync(rentingService);
        }

        public async Task DeleteRentingService(int rentingServiceId)
        {
            var rentingServices = await RentingServiceDao.Instance.GetAllAsync();
            var rentingService = rentingServices.FirstOrDefault(rs => rs.RentingServiceId == rentingServiceId);
            if (rentingService != null)
            {
                await RentingServiceDao.Instance.RemoveAsync(rentingService);
            }
        }

        public async Task<RentingServiceDto?> GetRentingServiceById(int rentingServiceId)
        {
            var rentingServices = await RentingServiceDao.Instance.GetAllAsync();
            var rentingService = rentingServices.FirstOrDefault(rs => rs.RentingServiceId == rentingServiceId);
            if (rentingService != null)
            {
                return _mapper.Map<RentingServiceDto>(rentingService);
            }

            return null;
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
                rentingService.IsOptional = rentingServiceRequestDto.IsOptional;

                await RentingServiceDao.Instance.UpdateAsync(rentingService);
            }
        }
    }
}
