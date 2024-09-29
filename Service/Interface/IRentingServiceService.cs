﻿using DTOs.RentingService;

namespace Service.Interface
{
    public interface IRentingServiceService
    {
        Task<IEnumerable<RentingServiceDto>> GetRentingServices();
        Task CreateRentingService(RentingServiceRequestDto rentingServiceRequestDto);
        Task UpdateRentingService(int rentingServiceId, RentingServiceRequestDto rentingServiceRequestDto);
    }
}
