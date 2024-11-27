using AutoMapper;
using BusinessObject;
using Common;
using Common.Enum;
using DAO;
using DTOs.AccountBusiness;
using DTOs.Machine;
using DTOs.MachineSerialNumber;
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

            var machineSerialNumbers = await MachineSerialNumberDao.Instance
                    .GetMachineSerialNumberAvailablesToRent(rentingRequestMachineInRangeDto.MachineIds, rentingRequestMachineInRangeDto.DateStart, rentingRequestMachineInRangeDto.DateEnd);
            if (machineSerialNumbers.IsNullOrEmpty())
            {
                rentingRequestInitDataDto.RentingRequestMachineDatas = [];
            }
            else
            {
                var groupedMachineSerialNumbers = machineSerialNumbers
                .GroupBy(msn => msn.MachineId)
                .ToDictionary(g => g.Key, g => g.ToList());

                foreach (var group in groupedMachineSerialNumbers)
                {
                    var machine = await MachineDao.Instance.GetMachine((int)group.Key);

                    var rentingRequestMachineDataDto = new RentingRequestMachineDataDto()
                    {
                        MachineId = group.Key ?? 0,
                        MachineName = machine.MachineName,
                        MachinePrice = machine.MachinePrice ?? 0,
                        Quantity = 0,
                        RentPrice = machine.RentPrice ?? 0,
                        CategoryName = machine.Category!.CategoryName ?? string.Empty,
                        ThumbnailUrl = string.Empty,
                        RentPrices = [],
                        //ShipPricePerKm = machine.ShipPricePerKm ?? 0,
                        MachineSerialNumbers = group.Value.Select(sn => new MachineSerialNumberDto
                        {
                            MachineId = sn.MachineId,
                            ActualRentPrice = sn.ActualRentPrice,
                            SerialNumber = sn.SerialNumber,
                            RentDaysCounter = sn.RentDaysCounter,
                            Status = sn.Status,
                            DateCreate = sn.DateCreate
                        }).ToList(),
                    };

                    rentingRequestMachineDataDto.Quantity = rentingRequestMachineDataDto.MachineSerialNumbers.Count;

                    var productTerms = _mapper.Map<List<MachineTermDto>>(machine.MachineTerms);
                    rentingRequestMachineDataDto.MachineTerms = productTerms;

                    if (!machine.MachineImages.IsNullOrEmpty())
                    {
                        rentingRequestMachineDataDto.ThumbnailUrl = machine.MachineImages.First(p => p.IsThumbnail == true).MachineImageUrl ?? string.Empty;
                    }

                    rentingRequestMachineDatas.Add(rentingRequestMachineDataDto);
                }

                rentingRequestInitDataDto.RentingRequestMachineDatas = rentingRequestMachineDatas;
            }

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

        public async Task<IEnumerable<RentingRequestDto>> GetRentingRequests(string? status)
        {
            var rentingRequests = await RentingRequestDao.Instance.GetRentingRequests();

            if (!rentingRequests.IsNullOrEmpty())
            {
                var rentingRequestDtos = _mapper.Map<IEnumerable<RentingRequestDto>>(rentingRequests);

                if (!string.IsNullOrEmpty(status))
                {
                    rentingRequestDtos = rentingRequestDtos.Where(c => c.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
                }

                return rentingRequestDtos;
            }

            return [];
        }

        public async Task<IEnumerable<CustomerRentingRequestDto>> GetRentingRequestsForCustomer(int customerId)
        {
            var rentingRequests = await RentingRequestDao.Instance.GetRentingRequestsForCustomer(customerId);
            var customerRentingRequests = _mapper.Map<IEnumerable<CustomerRentingRequestDto>>(rentingRequests);

            foreach (var rentingRequest in customerRentingRequests)
            {
                if (rentingRequest.Status.Equals(RentingRequestStatusEnum.UnPaid.ToString()))
                {
                    var invoice = await InvoiceDao.Instance.GetInvoicesByRentingRequest(rentingRequest.RentingRequestId);

                    //if (depositInvoice != null)
                    //{
                    //    var depositInvoiceDto = new PendingInvoiceDto
                    //    {
                    //        InvoiceId = depositInvoice.InvoiceId,
                    //        Status = depositInvoice.Status,
                    //        Type = depositInvoice.Type,
                    //    };

                    //    rentingRequest.PendingInvoices.Add(depositInvoiceDto);
                    //}

                    if (invoice != null)
                    {
                        var rentalInvoiceDto = new PendingInvoiceDto
                        {
                            InvoiceId = invoice.InvoiceId,
                            Status = invoice.Status,
                            Type = invoice.Type,
                        };

                        rentingRequest.PendingInvoices.Add(rentalInvoiceDto);
                    }
                }
            }

            return customerRentingRequests;
        }

        public async Task<CustomerRentingRequestDto?> GetCustomerRentingRequest(string rentingRequestId, int customerId)
        {
            var customerRentingRequests = await GetRentingRequestsForCustomer(customerId);
            var customerRentingRequest = customerRentingRequests.FirstOrDefault(r => r.RentingRequestId.Equals(rentingRequestId));
            return customerRentingRequest;
        }

        //TODO
        public async Task<RentingRequestDto> CreateRentingRequest(int customerId, NewRentingRequestDto newRentingRequestDto)
        {
            var rentingRequest = _mapper.Map<RentingRequest>(newRentingRequestDto);

            rentingRequest.AccountOrderId = customerId;
            rentingRequest.RentingRequestId = await GenerateRentingRequestId();
            rentingRequest.DateCreate = DateTime.Now;
            rentingRequest.Status = RentingRequestStatusEnum.UnPaid.ToString();
            rentingRequest.TotalDepositPrice = 0;
            rentingRequest.TotalRentPrice = 0;
            rentingRequest.TotalServicePrice = 0;
            rentingRequest.TotalAmount = 0;
            rentingRequest.AccountNumber = newRentingRequestDto.AccountNumber;
            rentingRequest.BeneficiaryBank = newRentingRequestDto.BeneficiaryBank;
            rentingRequest.BeneficiaryName = newRentingRequestDto.BeneficiaryName;

            var totalRentSerialNumbers = newRentingRequestDto.RentingRequestSerialNumbers.Count;
            rentingRequest.ShippingDistance = newRentingRequestDto.ShippingDistance;
            rentingRequest.ShippingPricePerKm = newRentingRequestDto.ShippingPricePerKm;
            rentingRequest.ShippingPrice = newRentingRequestDto.ShippingDistance * newRentingRequestDto.ShippingPricePerKm * totalRentSerialNumbers;

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
                rentingRequest.TotalServicePrice += requiredRentingService.Price * totalRentSerialNumbers;
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
                    rentingRequest.TotalServicePrice += optionalRentingService.Price * totalRentSerialNumbers;
                }
            }

            //rentingRequest.TotalAmount += rentingRequest.TotalServicePrice + newRentingRequestDto.ShippingPrice - newRentingRequestDto.DiscountPrice;
            rentingRequest.TotalAmount += rentingRequest.TotalServicePrice + rentingRequest.ShippingPrice - newRentingRequestDto.DiscountPrice;

            //rentingRequest = await RentingRequestDao.Instance.CreateRentingRequest(rentingRequest, newRentingRequestDto);
            rentingRequest = await RentingRequestDao.Instance.CreateAsync(rentingRequest);

            //return rentingRequest.RentingRequestId;
            return _mapper.Map<RentingRequestDto>(rentingRequest);
        }

        private async Task<string> GenerateRentingRequestId()
        {
            int currentTotalRentingRequests = await RentingRequestDao.Instance.GetLastestRentingRequestByDate(DateTime.Now);
            string datePart = DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern);
            string sequencePart = (currentTotalRentingRequests + 1).ToString("D3");
            return $"{GlobalConstant.RentingRequestIdPrefixPattern}{datePart}{GlobalConstant.SequenceSeparator}{sequencePart}";
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

        public async Task UpdateRentingRequestStatus(string rentingRequestId, string status)
        {
            var request = await RentingRequestDao.Instance.GetRentingRequestById(rentingRequestId);

            if (request == null)
            {
                throw new Exception(MessageConstant.RentingRequest.RentingRequestNotFound);
            }

            request.Status = status;

            await RentingRequestDao.Instance.UpdateAsync(request);
        }

        public async Task UpdateRentingRequest(RentingRequestDto rentingRequestDto)
        {
            var rentingRequest = _mapper.Map<RentingRequest>(rentingRequestDto);

            await RentingRequestDao.Instance.UpdateAsync(rentingRequest);
        }
    }
}
