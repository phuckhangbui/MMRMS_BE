using AutoMapper;
using BusinessObject;
using DTOs;
using DTOs.Account;
using DTOs.AccountAddressDto;
using DTOs.AccountPromotion;
using DTOs.Address;
using DTOs.Authentication;
using DTOs.Category;
using DTOs.Component;
using DTOs.Content;
using DTOs.Contract;
using DTOs.Delivery;
using DTOs.MaintenanceRequest;
using DTOs.MembershipRank;
using DTOs.Notification;
using DTOs.Product;
using DTOs.Promotion;
using DTOs.RentingRequest;
using DTOs.RentingService;
using DTOs.SerialNumberProduct;
using DTOs.Term;

namespace Repository.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<AccountDto, LoginAccountDto>();

            CreateMap<Account, AccountBaseDto>();
            CreateMap<Account, StaffAndManagerAccountDto>();
            CreateMap<Account, CustomerAccountDto>()
                        .ForMember(dest => dest.Company,
                            opt => opt.MapFrom(src => src.AccountBusinesses != null && src.AccountBusinesses.Any()
                                ? src.AccountBusinesses.FirstOrDefault().Company
                                : null))
                        .ForMember(dest => dest.Address,
                            opt => opt.MapFrom(src => src.AccountBusinesses != null && src.AccountBusinesses.Any()
                                ? src.AccountBusinesses.FirstOrDefault().Address
                                : null))
                        .ForMember(dest => dest.Position,
                            opt => opt.MapFrom(src => src.AccountBusinesses != null && src.AccountBusinesses.Any()
                                ? src.AccountBusinesses.FirstOrDefault().Position
                                : null))
                        .ForMember(dest => dest.TaxNumber,
                            opt => opt.MapFrom(src => src.AccountBusinesses != null && src.AccountBusinesses.Any()
                                ? src.AccountBusinesses.FirstOrDefault().TaxNumber
                                : null));

            CreateMap<Account, NewCustomerAccountDto>().ReverseMap();
            CreateMap<Account, NewStaffAndManagerAccountDto>().ReverseMap();
            CreateMap<Account, AccountOrderDto>();

            CreateMap<Category, CategoryDto>();
            CreateMap<Category, CategoryRequestDto>().ReverseMap();

            CreateMap<Product, ProductDto>()
                     .ForMember(dest => dest.CategoryName,
                         opt => opt.MapFrom(src => src.Category != null
                             ? src.Category.CategoryName
                             : null))
                     .ForMember(dest => dest.ProductImageList,
                         opt => opt.MapFrom(src => src.ProductImages != null && src.ProductImages.Any()
                             ? src.ProductImages
                             : null));
            CreateMap<ProductDto, Product>();

            CreateMap<UpdateProductDto, ProductDto>();

            CreateMap<Product, DisplayProductDetailDto>()
                    .ForMember(dest => dest.CategoryName,
                        opt => opt.MapFrom(src => src.Category != null
                            ? src.Category.CategoryName
                            : null))
                    .ForMember(dest => dest.ProductImageList,
                        opt => opt.MapFrom(src => src.ProductImages != null && src.ProductImages.Any()
                            ? src.ProductImages
                            : null))
                    .ForMember(dest => dest.ProductAttributeList,
                        opt => opt.MapFrom(src => src.ProductAttributes != null && src.ProductAttributes.Any()
                            ? src.ProductAttributes
                            : null))
                    .ForMember(dest => dest.ComponentProductList,
                        opt => opt.MapFrom(src => src.ComponentProducts != null && src.ComponentProducts.Any()
                            ? src.ComponentProducts
                            : null));


            CreateMap<ProductImage, ProductImageDto>();
            CreateMap<ProductAttribute, ProductAttributeDto>();
            CreateMap<ComponentProduct, ComponentProductDto>()
                 .ForMember(dest => dest.ComponentName,
                        opt => opt.MapFrom(src => src.Component != null
                            ? src.Component.ComponentName
                            : null));

            CreateMap<CreateProductDto, Product>();
            CreateMap<CreateProductAttributeDto, ProductAttribute>();
            CreateMap<AddExistedComponentToProduct, ComponentProduct>();

            CreateMap<Component, ComponentDto>();
            CreateMap<UpdateComponentDto, Component>();
            CreateMap<CreateComponentDto, Component>();


            CreateMap<SerialNumberProduct, SerialNumberProductDto>();

            CreateMap<Content, ContentDto>();
            CreateMap<Content, ContentCreateRequestDto>().ReverseMap();

            CreateMap<Promotion, PromotionDto>();
            CreateMap<Promotion, PromotionRequestDto>().ReverseMap();

            CreateMap<MembershipRank, MembershipRankDto>();
            CreateMap<MembershipRank, MembershipRankRequestDto>().ReverseMap();

            CreateMap<Contract, ContractDto>()
                .ForMember(dest => dest.AccountBusinesses, opt => opt.MapFrom(src => src.AccountSign.AccountBusinesses))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AccountSign.Name));
            CreateMap<Contract, ContractDetailDto>()
                .ForMember(dest => dest.AccountBusinesses, opt => opt.MapFrom(src => src.AccountSign.AccountBusinesses))
                .ForMember(dest => dest.ContractTerms, opt => opt.MapFrom(src => src.ContractTerms));
            CreateMap<Contract, ContractRequestDto>().ReverseMap();
            CreateMap<ContractTerm, ContractTermDto>();
            CreateMap<ContractTerm, ContractTermRequestDto>().ReverseMap();

            CreateMap<AccountBusiness, AccountBusinessDto>();

            CreateMap<Invoice, InvoiceDto>()
                 .ForMember(dest => dest.AccountPaidName,
                        opt => opt.MapFrom(src => src.AccountPaid != null
                            ? src.AccountPaid.Name
                            : null));

            CreateMap<RentingRequest, RentingRequestDto>()
                 .ForMember(dest => dest.AccountOrderName,
                        opt => opt.MapFrom(src => src.AccountOrder != null
                            ? src.AccountOrder.Name
                            : null));
            CreateMap<RentingRequest, NewRentingRequestDto>()
                .ForMember(dest => dest.ServiceRentingRequests, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.ServiceRentingRequests, opt => opt.Ignore());
            CreateMap<RentingRequestProductDetail, NewRentingRequestProductDetailDto>().ReverseMap();
            CreateMap<RentingRequest, RentingRequestDetailDto>();
            CreateMap<RentingRequestProductDetail, RentingRequestProductDetailDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName));
            CreateMap<ServiceRentingRequest, ServiceRentingRequestDto>()
                .ForMember(dest => dest.RentingServiceName, opt => opt.MapFrom(src => src.RentingService.RentingServiceName));

            CreateMap<Notification, NotificationDto>().ReverseMap();
            CreateMap<CreateNotificationDto, Notification>();

            CreateMap<RentingService, RentingServiceDto>();
            CreateMap<RentingService, RentingServiceRequestDto>().ReverseMap();

            CreateMap<AccountPromotion, AccountPromotionDto>();

            CreateMap<Address, AddressDto>();
            CreateMap<Address, AddressRequestDto>().ReverseMap();

            CreateMap<Delivery, DeliveryDto>()
                .ForMember(dest => dest.StaffName,
                opt => opt.MapFrom(src => src.Staff != null
                            ? src.Staff.Name
                            : null))
                .ForMember(dest => dest.ContractAddress,
                 opt => opt.MapFrom(src => src.Contract != null && src.Contract.ContractAddress != null
                            ? src.Contract.ContractAddress
                            : null));

            CreateMap<ContractAddress, ContractAddressDto>();
            CreateMap<MaintenanceRequest, MaintenanceRequestDto>()
                .ForMember(dest => dest.ContractAddress,
                 opt => opt.MapFrom(src => src.Contract != null && src.Contract.ContractAddress != null
                            ? src.Contract.ContractAddress
                            : null));

        }
    }
}
