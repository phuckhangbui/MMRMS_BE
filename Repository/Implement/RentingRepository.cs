using AutoMapper;
using BusinessObject;
using Common;
using Common.Enum;
using DAO;
using DTOs.AccountAddressDto;
using DTOs.AccountPromotion;
using DTOs.MembershipRank;
using DTOs.RentingRequest;
using DTOs.RentingService;
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

        public async Task<string> CreateRentingRequest(int customerId, NewRentingRequestDto newRentingRequestDto)
        {
            var rentingRequest = _mapper.Map<RentingRequest>(newRentingRequestDto);
            rentingRequest.AccountOrderId = customerId;

            //TODO
            rentingRequest.RentingRequestId = GlobalConstant.RentingRequestIdPrefixPattern + DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern);
            rentingRequest.DateCreate = DateTime.Now;
            rentingRequest.Status = RentingRequestStatusEnum.Pending.ToString();

            var rentingServices = await RentingServiceDao.Instance.GetAllAsync();

            //Required renting services
            var requiredRentingServices = rentingServices.Where(rs => rs.IsOptional == false).ToList();
            foreach (var requiredRentingService in requiredRentingServices)
            {
                var serviceRentingRequest = new ServiceRentingRequest()
                {
                    ServicePrice = requiredRentingService.Price,
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
                    var serviceRentingRequest = new ServiceRentingRequest()
                    {
                        ServicePrice = optionalRentingService.Price,
                        RentingServiceId = optionalRentingService.RentingServiceId,
                    };

                    rentingRequest.ServiceRentingRequests.Add(serviceRentingRequest);
                }
            }

            rentingRequest = await RentingRequestDao.Instance.CreateAsync(rentingRequest);
            return rentingRequest.RentingRequestId;
        }

        public async Task<RentingRequestDetailDto?> GetRentingRequestDetailById(string rentingRequestId)
        {
            var rentingRequest = await RentingRequestDao.Instance.GetRentingRequestById(rentingRequestId);
            if (rentingRequest != null)
            {
                var rentingRequesteDto = _mapper.Map<RentingRequestDetailDto>(rentingRequest);

                var accountBusinesses = await AccountBusinessDao.Instance.GetAccountBusinessesByAccountId((int)rentingRequest.AccountOrderId!);
                if (accountBusinesses.Any())
                {
                    rentingRequesteDto.AccountBusinesses = _mapper.Map<List<AccountBusinessDto>>(accountBusinesses);
                }

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
                    ThumbnailUrl = string.Empty,
                };
                if (!product.ProductImages.IsNullOrEmpty())
                {
                    rentingRequestProductDataDto.ThumbnailUrl = product.ProductImages.First(p => p.IsThumbnail == true).ProductImageUrl ?? string.Empty;
                }

                rentingRequestProductDatas.Add(rentingRequestProductDataDto);
            }
            rentingRequestInitDataDto.RentingRequestProductDatas = rentingRequestProductDatas;

            //Promotion data
            var promotions = await AccountPromotionDao.Instance.GetPromotionsByCustomerId(customerId);
            if (!promotions.IsNullOrEmpty())
            {
                var shippingTypePromotions = promotions.Where(p => p.Promotion!.DiscountTypeName!.Equals(DiscountTypeNameEnum.Shipping.ToString()));
                rentingRequestInitDataDto.AccountPromotions = _mapper.Map<List<AccountPromotionDto>>(shippingTypePromotions);
            }

            //Membership data
            var membershipRank = await MembershipRankDao.Instance.GetMembershipRanksForCustomer(customerId);
            if (membershipRank != null)
            {
                rentingRequestInitDataDto.MembershipRank = _mapper.Map<MembershipRankDto>(membershipRank);
            }

            //Renting service data
            var rentingServices = await RentingServiceDao.Instance.GetAllAsync();
            if (!rentingServices.IsNullOrEmpty())
            {
                rentingRequestInitDataDto.RentingServices = _mapper.Map<List<RentingServiceDto>>(rentingServices);
            }

            return rentingRequestInitDataDto;
        }

        public async Task<IEnumerable<RentingRequestDto>> GetRentingRequests()
        {
            IEnumerable<RentingRequest> rentingRequests = await RentingRequestDao.Instance.GetRentingRequests();

            return _mapper.Map<IEnumerable<RentingRequestDto>>(rentingRequests);
        }

        public async Task<IEnumerable<RentingRequestDto>> GetRentingRequestsForCustomer(int customerId)
        {
            var rentingRequests = await RentingRequestDao.Instance.GetRentingRequestsForCustomer(customerId);
            return _mapper.Map<IEnumerable<RentingRequestDto>>(rentingRequests);
        }
    }
}
