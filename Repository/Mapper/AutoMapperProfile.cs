using AutoMapper;
using BusinessObject;
using DTOs.Account;
using DTOs.AccountAddressDto;
using DTOs.Address;
using DTOs.Authentication;
using DTOs.Category;
using DTOs.Component;
using DTOs.Content;
using DTOs.Contract;
using DTOs.ContractPayment;
using DTOs.Delivery;
using DTOs.EmployeeTask;
using DTOs.Invoice;
using DTOs.Log;
using DTOs.MaintenanceRequest;
using DTOs.MaintenanceTicket;
using DTOs.MembershipRank;
using DTOs.Notification;
using DTOs.Product;
using DTOs.RentingRequest;
using DTOs.RentingRequestAddress;
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
            CreateMap<Account, EmployeeAccountDto>();
            CreateMap<Account, CustomerAccountDto>()
                        .ForMember(dest => dest.Company,
                            opt => opt.MapFrom(src => src.AccountBusiness != null
                                ? src.AccountBusiness.Company
                                : null))
                        .ForMember(dest => dest.Address,
                            opt => opt.MapFrom(src => src.AccountBusiness != null
                                ? src.AccountBusiness.Address
                                : null))
                        .ForMember(dest => dest.Position,
                            opt => opt.MapFrom(src => src.AccountBusiness != null
                                ? src.AccountBusiness.Position
                                : null))
                        .ForMember(dest => dest.TaxNumber,
                            opt => opt.MapFrom(src => src.AccountBusiness != null
                                ? src.AccountBusiness.TaxNumber
                                : null));
            CreateMap<Account, CustomerAccountDetailDto>()
                         .ForMember(dest => dest.Company,
                            opt => opt.MapFrom(src => src.AccountBusiness != null
                                ? src.AccountBusiness.Company
                                : null))
                        .ForMember(dest => dest.Address,
                            opt => opt.MapFrom(src => src.AccountBusiness != null
                                ? src.AccountBusiness.Address
                                : null))
                        .ForMember(dest => dest.Position,
                            opt => opt.MapFrom(src => src.AccountBusiness != null
                                ? src.AccountBusiness.Position
                                : null))
                        .ForMember(dest => dest.TaxNumber,
                            opt => opt.MapFrom(src => src.AccountBusiness != null
                                ? src.AccountBusiness.TaxNumber
                                : null));

            CreateMap<Account, NewCustomerAccountDto>().ReverseMap();
            CreateMap<Account, NewEmployeeAccountDto>().ReverseMap();
            CreateMap<Account, AccountOrderDto>();
            CreateMap<Account, StaffAccountDto>();

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
            CreateMap<Product, ProductReviewDto>();

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
                            : null))
                    .ForMember(dest => dest.ProductTermList,
                        opt => opt.MapFrom(src => src.ProductTerms != null && src.ProductTerms.Any()
                            ? src.ProductTerms
                            : null));

            CreateMap<DisplayProductDetailDto, ProductDto>();


            CreateMap<ProductImage, ProductImageDto>();
            CreateMap<ProductAttribute, ProductAttributeDto>();
            CreateMap<ProductTerm, ProductTermDto>();
            CreateMap<ComponentProduct, ComponentProductDto>()
                 .ForMember(dest => dest.ComponentName,
                        opt => opt.MapFrom(src => src.Component != null
                            ? src.Component.ComponentName
                            : null));

            CreateMap<CreateProductDto, Product>();
            CreateMap<CreateProductAttributeDto, ProductAttribute>();
            CreateMap<CreateProductTermDto, ProductTerm>();
            CreateMap<AddExistedComponentToProduct, ComponentProduct>();

            CreateMap<Component, ComponentDto>();
            CreateMap<UpdateComponentDto, Component>();
            CreateMap<CreateComponentDto, Component>();

            CreateMap<SerialNumberProduct, SerialNumberProductDto>();
            CreateMap<SerialNumberProduct, SerialNumberProductOptionDto>();

            CreateMap<Content, ContentDto>();
            CreateMap<Content, ContentCreateRequestDto>().ReverseMap();

            //CreateMap<Promotion, PromotionDto>();
            //CreateMap<Promotion, PromotionRequestDto>().ReverseMap();

            CreateMap<MembershipRank, MembershipRankDto>();
            CreateMap<MembershipRank, MembershipRankRequestDto>().ReverseMap();

            CreateMap<Contract, ContractDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ContractSerialNumberProduct != null &&
                    src.ContractSerialNumberProduct.Product != null ? src.ContractSerialNumberProduct.ProductId : null))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ContractSerialNumberProduct != null &&
                    src.ContractSerialNumberProduct.Product != null ? src.ContractSerialNumberProduct.Product.ProductName : null))
                .ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src => src.ContractSerialNumberProduct != null &&
                    src.ContractSerialNumberProduct.Product != null &&
                    src.ContractSerialNumberProduct.Product.ProductImages != null ? src.ContractSerialNumberProduct.Product.ProductImages.FirstOrDefault(p => p.IsThumbnail == true).ProductImageUrl : null));
            CreateMap<Contract, ContractDetailDto>()
                .ForMember(dest => dest.AccountOrder, opt => opt.MapFrom(src => src.AccountSign))
                .ForMember(dest => dest.AccountBusiness, opt => opt.MapFrom(src => src.AccountSign.AccountBusiness))
                .ForMember(dest => dest.ContractTerms, opt => opt.MapFrom(src => src.ContractTerms))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ContractSerialNumberProduct != null &&
                    src.ContractSerialNumberProduct.Product != null ? src.ContractSerialNumberProduct.ProductId : null))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ContractSerialNumberProduct != null &&
                    src.ContractSerialNumberProduct.Product != null ? src.ContractSerialNumberProduct.Product.ProductName : null))
                .ForMember(dest => dest.IsOnetimePayment, opt => opt.MapFrom(src => src.RentingRequest != null ? src.RentingRequest.IsOnetimePayment : null));
            CreateMap<Contract, ContractRequestDto>().ReverseMap();
            CreateMap<ContractTerm, ContractTermDto>();
            CreateMap<ContractTerm, ContractTermRequestDto>().ReverseMap();

            CreateMap<AccountBusiness, AccountBusinessDto>();

            CreateMap<Invoice, InvoiceDto>()
                 .ForMember(dest => dest.AccountPaidName,
                        opt => opt.MapFrom(src => src.AccountPaid != null
                            ? src.AccountPaid.Name
                            : null));
            CreateMap<InvoiceDto, Invoice>();
            CreateMap<Invoice, ContractInvoiceDto>();

            CreateMap<DigitalTransaction, TransactionReturn>().ReverseMap();

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
            CreateMap<ServiceRentingRequest, ServiceRentingRequestDto>()
                .ForMember(dest => dest.RentingServiceName, opt => opt.MapFrom(src => src.RentingService.RentingServiceName));
            CreateMap<Notification, NotificationDto>().ReverseMap();
            CreateMap<CreateNotificationDto, Notification>();

            CreateMap<RentingService, RentingServiceDto>();
            CreateMap<RentingService, RentingServiceRequestDto>().ReverseMap();

            //CreateMap<AccountPromotion, AccountPromotionDto>();

            CreateMap<Address, AddressDto>();
            CreateMap<Address, AddressRequestDto>().ReverseMap();

            CreateMap<Delivery, DeliveryDto>()
                .ForMember(dest => dest.StaffName,
                opt => opt.MapFrom(src => src.Staff != null
                            ? src.Staff.Name
                            : null))
                .ForMember(dest => dest.ContractAddress,
                 opt => opt.MapFrom(src => src.Contract != null && src.Contract.RentingRequest != null && src.Contract.RentingRequest.RentingRequestAddress != null
                            ? src.Contract.RentingRequest.RentingRequestAddress
                            : null));

            CreateMap<RentingRequestAddress, ContractAddressDto>();
            CreateMap<MaintenanceRequest, MaintenanceRequestDto>()
                .ForMember(dest => dest.ContractAddress,
                 opt => opt.MapFrom(src => src.Contract != null && src.Contract.RentingRequest != null && src.Contract.RentingRequest.RentingRequestAddress != null
                            ? src.Contract.RentingRequest.RentingRequestAddress
                            : null)); ;

            CreateMap<EmployeeTask, EmployeeTaskDto>()
                .ForMember(dest => dest.StaffName,
                opt => opt.MapFrom(src => src.Staff != null
                            ? src.Staff.Name
                            : null))
                .ForMember(dest => dest.ManagerName,
                opt => opt.MapFrom(src => src.Manager != null
                            ? src.Manager.Name
                            : null));

            CreateMap<EmployeeTask, EmployeeTaskDisplayDetail>()
                .ForMember(dest => dest.TaskLogs,
                opt => opt.MapFrom(src => src.TaskLogs != null
                            ? src.TaskLogs
                            : null))
                .ForMember(dest => dest.MaintenanceTicketCreateFromTaskList,
                opt => opt.MapFrom(src => src.MaintenanceTicketsCreateFromTask != null
                            ? src.MaintenanceTicketsCreateFromTask
                            : null));

            CreateMap<TaskLog, TaskLogDto>()
                .ForMember(dest => dest.AccountTriggerName,
                opt => opt.MapFrom(src => src.AccountTrigger != null
                                ? src.AccountTrigger.Name
                                : string.Empty));

            CreateMap<MaintenanceTicket, MaintenanceTicketDto>()
               .ForMember(dest => dest.EmployeeCreateName,
               opt => opt.MapFrom(src => src.EmployeeCreate != null
                           ? src.EmployeeCreate.Name
                           : null))
               .ForMember(dest => dest.ComponentName,
               opt => opt.MapFrom(src => src.Component != null
                           ? src.Component.ComponentName
                           : null));

            CreateMap<LogDetail, LogDetailDto>()
               .ForMember(dest => dest.Name,
               opt => opt.MapFrom(src => src.Account != null
                           ? src.Account.Name
                           : string.Empty))
               .ForMember(dest => dest.Phone,
               opt => opt.MapFrom(src => src.Account != null
                           ? src.Account.Phone
                           : string.Empty));

            CreateMap<RentingRequestAddress, RentingRequestAddressDto>();

            CreateMap<Term, TermDto>();
            CreateMap<CreateTermDto, Term>();
            CreateMap<UpdateTermDto, Term>();

            CreateMap<ContractPayment, ContractPaymentDto>();
        }
    }
}
