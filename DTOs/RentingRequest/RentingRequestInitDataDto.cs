﻿using DTOs.AccountPromotion;
using DTOs.MembershipRank;
using DTOs.RentingService;

namespace DTOs.RentingRequest
{
    public class RentingRequestInitDataDto
    {
        public List<AccountPromotionDto> AccountPromotions { get; set; }
        public MembershipRankDto MembershipRank { get; set; }
        public List<RentingRequestProductDataDto> RentingRequestProductDatas { get; set; }
        public List<RentingServiceDto> RentingServices { get; set; }
    }

    public class RentingRequestProductDataDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public double ProductPrice { get; set; }
        public double RentPrice { get; set; }
        public int Quantity { get; set; }
        public string CategoryName { get; set; }
    }
}
