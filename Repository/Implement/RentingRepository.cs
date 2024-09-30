using AutoMapper;
using BusinessObject;
using DAO;
using DAO.Enum;
using DTOs.AccountPromotion;
using DTOs.MembershipRank;
using DTOs.RentingRequest;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;

namespace Repository.Implement
{
    public class RentingRepository : IRentingRepository
    {
        private readonly IMapper _mapper;

        public RentingRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CheckRentingRequestValidToRent(string rentingRequestId)
        {
            var rentingRequest = await RentingRequestDao.Instance.GetRentingRequestByIdAndStatus(rentingRequestId, RentingRequestStatusEnum.Approved.ToString());
            return rentingRequest != null;
        }

        public async Task CreateRentingRequest(NewRentingRequestDto newRentingRequestDto)
        {
            var rentingRequest = _mapper.Map<RentingRequest>(newRentingRequestDto);

            //TODO
            rentingRequest.RentingRequestId = Guid.NewGuid().ToString();
            rentingRequest.DateCreate = DateTime.Now;
            rentingRequest.Status = RentingRequestStatusEnum.Pending.ToString();

            var rentingServices = await RentingServiceDao.Instance.GetAllAsync();

            //Required renting services
            var requiredRentingServices = rentingServices.Where(rs => rs.IsOptional == false).ToList();
            foreach (var requiredRentingService in requiredRentingServices)
            {
                //TODO
                var serviceRentingRequest = new ServiceRentingRequest()
                {
                    ServicePrice = 0,
                    DiscountPrice = 0,
                    FinalPrice = 0,
                    RentingServiceId = requiredRentingService.RentingServiceId,
                };

                rentingRequest.ServiceRentingRequests.Add(serviceRentingRequest);
            }

            //Optional renting services
            if (newRentingRequestDto.ServiceRentingRequests.Count != 0)
            {
                var optionalRentingServices = rentingServices.Where(rs => rs.IsOptional == true).ToList();
                var optionalServiceRentingRequests = rentingServices
                    .Where(srr => newRentingRequestDto.ServiceRentingRequests.Contains(srr.RentingServiceId))
                    .ToList();

                foreach (var optionalRentingService in optionalServiceRentingRequests)
                {
                    //TODO
                    var serviceRentingRequest = new ServiceRentingRequest()
                    {
                        ServicePrice = 0,
                        DiscountPrice = 0,
                        FinalPrice = 0,
                        RentingServiceId = optionalRentingService.RentingServiceId,
                    };

                    rentingRequest.ServiceRentingRequests.Add(serviceRentingRequest);
                }
            }

            await RentingRequestDao.Instance.CreateAsync(rentingRequest);
        }

        public async Task<RentingRequestDetailDto?> GetRentingRequestDetailById(string rentingRequestId)
        {
            var rentingRequest = await RentingRequestDao.Instance.GetRentingRequestById(rentingRequestId);
            if (rentingRequest != null)
            {
                var rentingRequesteDto = _mapper.Map<RentingRequestDetailDto>(rentingRequest);
                return rentingRequesteDto;
            }

            return null;
        }

        public async Task<RentingRequestInitDataDto> GetRentingRequestInitData(int customerId, List<int> productIds)
        {
            var rentingRequestInitDataDto = new RentingRequestInitDataDto();

            //Product data
            var rentingRequestProductDatas = new List<RentingRequestProductDataDto>();
            foreach (var productId in productIds)
            {
                var availableSerialNumberProducts = await SerialNumberProductDao.Instance
                    .GetSerialNumberProductsByProductIdAndStatus(productId, SerialNumberProductStatusEnum.Available.ToString());
                var product = await ProductDao.Instance.GetProduct(productId);

                var rentingRequestProductDataDto = new RentingRequestProductDataDto()
                {
                    ProductId = productId,
                    ProductName = product.ProductName,
                    ProductPrice = product.ProductPrice ?? 0,
                    Quantity = availableSerialNumberProducts.Count(),
                    RentPrice = product.RentPrice ?? 0,
                    CategoryName = product.Category!.CategoryName ?? string.Empty,
                };

                rentingRequestProductDatas.Add(rentingRequestProductDataDto);
            }
            rentingRequestInitDataDto.RentingRequestProductDatas = rentingRequestProductDatas;

            //Promotion data
            var promotions = await AccountPromotionDao.Instance.GetPromotionsByCustomerId(customerId);
            if (!promotions.IsNullOrEmpty())
            {
                rentingRequestInitDataDto.AccountPromotions = (List<AccountPromotionDto>)_mapper.Map<IEnumerable<AccountPromotionDto>>(promotions);
            }

            //Membership data
            var membershipRank = await MembershipRankDao.Instance.GetMembershipRanksForCustomer(customerId);
            if (membershipRank != null)
            {
                rentingRequestInitDataDto.MembershipRank = _mapper.Map<MembershipRankDto>(membershipRank);
            }

            return rentingRequestInitDataDto;
        }

        public async Task<IEnumerable<RentingRequestDto>> GetRentingRequests()
        {
            IEnumerable<RentingRequest> rentingRequests = await RentingRequestDao.Instance.GetRentingRequests();

            return _mapper.Map<IEnumerable<RentingRequestDto>>(rentingRequests);
        }
    }
}
