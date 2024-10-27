using AutoMapper;
using BusinessObject;
using Common;
using Common.Enum;
using DAO;
using DTOs.AccountAddressDto;
using DTOs.Machine;
using DTOs.MembershipRank;
using DTOs.RentingRequest;
using DTOs.RentingService;
using DTOs.Term;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;

namespace Repository.Implement
{
    public class RentingRequestRepository : IRentingRequestRepository
    {
        private readonly IMapper _mapper;

        public RentingRequestRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        //public async Task<bool> CheckRentingRequestValidToRent(string rentingRequestId)
        //{
        //    var rentingRequest = await RentingRequestDao.Instance.GetRentingRequestByIdAndStatus(rentingRequestId, RentingRequestStatusEnum.Approved.ToString());
        //    return rentingRequest != null;
        //}

        public async Task<RentingRequestDetailDto?> GetRentingRequestDetailById(string rentingRequestId)
        {
            var rentingRequest = await RentingRequestDao.Instance.GetRentingRequestById(rentingRequestId);
            if (rentingRequest != null)
            {
                var rentingRequesteDto = _mapper.Map<RentingRequestDetailDto>(rentingRequest);

                var accountBusinesses = await AccountBusinessDao.Instance.GetAccountBusinessesByAccountId((int)rentingRequest.AccountOrderId!);
                if (accountBusinesses != null)
                {
                    rentingRequesteDto.AccountBusiness = _mapper.Map<AccountBusinessDto>(accountBusinesses);
                }

                return rentingRequesteDto;
            }

            return null;
        }

        public async Task<RentingRequestInitDataDto> InitializeRentingRequestData(int customerId, RentingRequestMachineInRangeDto rentingRequestMachineInRangeDto)
        {
            var rentingRequestInitDataDto = new RentingRequestInitDataDto();

            //Machine data
            var rentingRequestMachineDatas = new List<RentingRequestMachineDataDto>();
            foreach (var productId in rentingRequestMachineInRangeDto.MachineIds)
            {
                var availableMachineSerialNumbers = await MachineSerialNumberDao.Instance
                    .GetMachineSerialNumberValidToRent(productId, rentingRequestMachineInRangeDto.DateStart, rentingRequestMachineInRangeDto.DateEnd);

                if (availableMachineSerialNumbers.IsNullOrEmpty())
                {
                    continue;
                }

                var product = await MachineDao.Instance.GetMachine(productId);
                var prices = availableMachineSerialNumbers
                    .Select(s => s.ActualRentPrice ?? 0)
                    .ToList();

                var rentingRequestMachineDataDto = new RentingRequestMachineDataDto()
                {
                    MachineId = productId,
                    MachineName = product.MachineName,
                    MachinePrice = product.MachinePrice ?? 0,
                    Quantity = availableMachineSerialNumbers.Count,
                    RentPrice = product.RentPrice ?? 0,
                    CategoryName = product.Category!.CategoryName ?? string.Empty,
                    ThumbnailUrl = string.Empty,
                    RentPrices = prices,
                    ShipPricePerKm = product.ShipPricePerKm ?? 0,
                };

                var productTerms = _mapper.Map<List<MachineTermDto>>(product.MachineTerms);
                rentingRequestMachineDataDto.MachineTerms = productTerms;

                if (!product.MachineImages.IsNullOrEmpty())
                {
                    rentingRequestMachineDataDto.ThumbnailUrl = product.MachineImages.First(p => p.IsThumbnail == true).MachineImageUrl ?? string.Empty;
                }

                rentingRequestMachineDatas.Add(rentingRequestMachineDataDto);
            }
            rentingRequestInitDataDto.RentingRequestMachineDatas = rentingRequestMachineDatas;

            //Promotion data
            //var promotions = await AccountPromotionDao.Instance.GetPromotionsByCustomerId(customerId);
            //if (!promotions.IsNullOrEmpty())
            //{
            //    var shippingTypePromotions = promotions.Where(p => p.Promotion!.DiscountTypeName!.Equals(DiscountTypeNameEnum.Shipping.ToString()));
            //    rentingRequestInitDataDto.AccountPromotions = _mapper.Map<List<AccountPromotionDto>>(shippingTypePromotions);
            //}

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

            //Contract term data
            var contractTerms = await TermDao.Instance.GetTermsByTermType(TermTypeEnum.Contract);
            if (!contractTerms.IsNullOrEmpty())
            {
                rentingRequestInitDataDto.Terms = _mapper.Map<List<TermDto>>(contractTerms);
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

        //TODO
        public async Task<string> CreateRentingRequest(int customerId, NewRentingRequestDto newRentingRequestDto)
        {
            var rentingRequest = _mapper.Map<RentingRequest>(newRentingRequestDto);
            rentingRequest.AccountOrderId = customerId;

            rentingRequest.RentingRequestId = GlobalConstant.RentingRequestIdPrefixPattern + DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern);
            rentingRequest.DateCreate = DateTime.Now;
            rentingRequest.Status = RentingRequestStatusEnum.UnPaid.ToString();
            rentingRequest.TotalAmount = 0;

            var address = await AddressDao.Instance.GetAddressById(newRentingRequestDto.AddressId);
            if (address != null)
            {
                var rentingRequestAddress = new RentingRequestAddress()
                {
                    RentingRequestId = rentingRequest.RentingRequestId,
                    AddressBody = address.AddressBody,
                    City = address.City,
                    Coordinates = address.Coordinates,
                    District = address.District,
                };

                rentingRequest.RentingRequestAddress = rentingRequestAddress;
            }

            var rentingServices = await RentingServiceDao.Instance.GetAllAsync();
            double totalRentingServicePrice = 0;
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
                totalRentingServicePrice += (double)requiredRentingService.Price!;
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
                    totalRentingServicePrice += (double)optionalRentingService.Price!;
                }
            }

            rentingRequest.TotalServicePrice = totalRentingServicePrice;

            rentingRequest = await RentingRequestDao.Instance.CreateRentingRequest(rentingRequest, newRentingRequestDto);

            //_background.ScheduleCancelRentingRequestJob(rentingRequest.RentingRequestId);

            return rentingRequest.RentingRequestId;
        }

        public async Task<bool> CancelRentingRequest(string rentingRequestId)
        {
            var result = await RentingRequestDao.Instance.CancelRentingRequest(rentingRequestId);
            if (result != null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsRentingRequestValidToCancel(string rentingRequestId)
        {
            var rentingRequest = await RentingRequestDao.Instance.GetRentingRequestById(rentingRequestId);
            if (rentingRequest == null || !rentingRequest.Status.Equals(RentingRequestStatusEnum.UnPaid.ToString()))
            {
                return false;
            }

            bool areContractsNotSigned = rentingRequest.Contracts.All(c => c.Status == ContractStatusEnum.NotSigned.ToString());

            return areContractsNotSigned;
        }

        public async Task<RentingRequestDto> GetRentingRequest(string rentingRequestId)
        {
            var list = await GetRentingRequests();

            return list.FirstOrDefault(r => r.RentingRequestId == rentingRequestId);
        }

        public async Task UpdateRentingRequest(RentingRequestDto rentingRequestDto)
        {
            var rentingRequest = _mapper.Map<RentingRequest>(rentingRequestDto);

            await RentingRequestDao.Instance.UpdateAsync(rentingRequest);
        }
    }
}
