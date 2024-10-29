using AutoMapper;
using BusinessObject;
using DTOs.Account;
using DTOs.AccountAddressDto;
using DTOs.Address;
using DTOs.Authentication;
using DTOs.Category;
using DTOs.Component;
using DTOs.ComponentReplacementTicket;
using DTOs.Content;
using DTOs.Contract;
using DTOs.ContractPayment;
using DTOs.Delivery;
using DTOs.DeliveryTask;
using DTOs.Invoice;
using DTOs.Log;
using DTOs.Machine;
using DTOs.MachineCheckRequest;
using DTOs.MachineSerialNumber;
using DTOs.MachineTask;
using DTOs.MembershipRank;
using DTOs.Notification;
using DTOs.RentingRequest;
using DTOs.RentingRequestAddress;
using DTOs.RentingService;
using DTOs.Role;
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

            CreateMap<Machine, MachineDto>()
                     .ForMember(dest => dest.CategoryName,
                         opt => opt.MapFrom(src => src.Category != null
                             ? src.Category.CategoryName
                             : null))
                     .ForMember(dest => dest.MachineImageList,
                         opt => opt.MapFrom(src => src.MachineImages != null && src.MachineImages.Any()
                             ? src.MachineImages
                             : null));
            CreateMap<MachineDto, Machine>();
            CreateMap<Machine, MachineReviewDto>();

            CreateMap<UpdateMachineDto, MachineDto>();

            CreateMap<Machine, DisplayMachineDetailDto>()
                    .ForMember(dest => dest.CategoryName,
                        opt => opt.MapFrom(src => src.Category != null
                            ? src.Category.CategoryName
                            : null))
                    .ForMember(dest => dest.MachineImageList,
                        opt => opt.MapFrom(src => src.MachineImages != null && src.MachineImages.Any()
                            ? src.MachineImages
                            : null))
                    .ForMember(dest => dest.MachineAttributeList,
                        opt => opt.MapFrom(src => src.MachineAttributes != null && src.MachineAttributes.Any()
                            ? src.MachineAttributes
                            : null))
                    .ForMember(dest => dest.MachineComponentList,
                        opt => opt.MapFrom(src => src.MachineComponents != null && src.MachineComponents.Any()
                            ? src.MachineComponents
                            : null))
                    .ForMember(dest => dest.MachineTermList,
                        opt => opt.MapFrom(src => src.MachineTerms != null && src.MachineTerms.Any()
                            ? src.MachineTerms
                            : null));

            CreateMap<DisplayMachineDetailDto, MachineDto>();


            CreateMap<MachineImage, MachineImageDto>();
            CreateMap<MachineAttribute, MachineAttributeDto>();
            CreateMap<MachineTerm, MachineTermDto>();
            CreateMap<MachineComponent, MachineComponentDto>()
                 .ForMember(dest => dest.ComponentName,
                        opt => opt.MapFrom(src => src.Component != null
                            ? src.Component.ComponentName
                            : null));

            CreateMap<CreateMachineDto, Machine>();
            CreateMap<CreateMachineAttributeDto, MachineAttribute>();
            CreateMap<CreateMachineTermDto, MachineTerm>();
            CreateMap<AddExistedComponentToMachine, MachineComponent>();

            CreateMap<Component, ComponentDto>();
            CreateMap<UpdateComponentDto, Component>();
            CreateMap<CreateComponentDto, Component>();

            CreateMap<MachineSerialNumber, MachineSerialNumberDto>();
            CreateMap<MachineSerialNumber, MachineSerialNumberOptionDto>();
            CreateMap<MachineSerialNumberLog, MachineSerialNumberLogDto>()
                .ForMember(dest => dest.AccountTriggerName,
                        opt => opt.MapFrom(src => src.AccountTrigger != null
                            ? src.AccountTrigger.Name
                            : null));
            CreateMap<MachineComponentStatus, MachineComponentStatusDto>()
                .ForMember(dest => dest.ComponentName,
                        opt => opt.MapFrom(src => src.Component != null
                            ? src.Component.Component.ComponentName
                            : null));

            CreateMap<Content, ContentDto>()
                .ForMember(dest => dest.AccountCreateName, opt => opt.MapFrom(src => src.Account != null ? src.Account.Name : null));
            CreateMap<Content, ContentCreateRequestDto>().ReverseMap();

            //CreateMap<Promotion, PromotionDto>();
            //CreateMap<Promotion, PromotionRequestDto>().ReverseMap();

            CreateMap<MembershipRank, MembershipRankDto>();
            CreateMap<MembershipRank, MembershipRankRequestDto>().ReverseMap();

            CreateMap<Contract, ContractDto>()
                .ForMember(dest => dest.MachineId, opt => opt.MapFrom(src => src.ContractMachineSerialNumber != null &&
                    src.ContractMachineSerialNumber.Machine != null ? src.ContractMachineSerialNumber.MachineId : null))
                .ForMember(dest => dest.MachineName, opt => opt.MapFrom(src => src.ContractMachineSerialNumber != null &&
                    src.ContractMachineSerialNumber.Machine != null ? src.ContractMachineSerialNumber.Machine.MachineName : null))
                .ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src => src.ContractMachineSerialNumber != null &&
                    src.ContractMachineSerialNumber.Machine != null &&
                    src.ContractMachineSerialNumber.Machine.MachineImages != null ? src.ContractMachineSerialNumber.Machine.MachineImages.FirstOrDefault(p => p.IsThumbnail == true).MachineImageUrl : null));
            CreateMap<Contract, ContractDetailDto>()
                .ForMember(dest => dest.AccountOrder, opt => opt.MapFrom(src => src.AccountSign))
                .ForMember(dest => dest.AccountBusiness, opt => opt.MapFrom(src => src.AccountSign.AccountBusiness))
                .ForMember(dest => dest.ContractTerms, opt => opt.MapFrom(src => src.ContractTerms))
                .ForMember(dest => dest.MachineId, opt => opt.MapFrom(src => src.ContractMachineSerialNumber != null &&
                    src.ContractMachineSerialNumber.Machine != null ? src.ContractMachineSerialNumber.MachineId : null))
                .ForMember(dest => dest.MachineName, opt => opt.MapFrom(src => src.ContractMachineSerialNumber != null &&
                    src.ContractMachineSerialNumber.Machine != null ? src.ContractMachineSerialNumber.Machine.MachineName : null))
                .ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src => src.ContractMachineSerialNumber != null &&
                    src.ContractMachineSerialNumber.Machine != null &&
                    src.ContractMachineSerialNumber.Machine.MachineImages != null ? src.ContractMachineSerialNumber.Machine.MachineImages.FirstOrDefault(p => p.IsThumbnail == true).MachineImageUrl : null))
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

            CreateMap<RentingRequestDto, RentingRequest>();
            CreateMap<RentingRequest, NewRentingRequestDto>()
                .ForMember(dest => dest.ServiceRentingRequests, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.ServiceRentingRequests, opt => opt.Ignore());
            CreateMap<RentingRequestMachineDetail, NewRentingRequestMachineDetailDto>().ReverseMap();
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

            CreateMap<DeliveryTask, DeliveryTaskDto>()
                    .ForMember(dest => dest.StaffName,
                        opt => opt.MapFrom(src => src.Staff != null ? src.Staff.Name : null))
                    .ForMember(dest => dest.ManagerName,
                        opt => opt.MapFrom(src => src.Manager != null ? src.Manager.Name : null))
                    .ForMember(dest => dest.SerialNumber,
                        opt => opt.MapFrom(src =>
                            src.ContractDeliveries != null
                            && src.ContractDeliveries.Any(d => d.Contract != null)
                            && src.ContractDeliveries.FirstOrDefault(d => d.Contract != null).Contract != null
                            ? src.ContractDeliveries
                                .FirstOrDefault(d => d.Contract != null)
                                .Contract.SerialNumber
                            : null))
                    .ForMember(dest => dest.ContractAddress,
                        opt => opt.MapFrom(src =>
                            src.ContractDeliveries != null
                            && src.ContractDeliveries.Any(d => d.Contract != null)
                            && src.ContractDeliveries.FirstOrDefault(d => d.Contract != null).Contract.RentingRequest != null
                            ? src.ContractDeliveries
                                .FirstOrDefault(d => d.Contract != null)
                                .Contract.RentingRequest.RentingRequestAddress
                            : null));

            CreateMap<DeliveryTaskLog, DeliveryTaskLogDto>()
                .ForMember(dest => dest.AccountTriggerName,
                        opt => opt.MapFrom(src => src.AccountTrigger != null ? src.AccountTrigger.Name : null));
            CreateMap<ContractDelivery, ContractDeliveryDto>();



            CreateMap<RentingRequestAddress, ContractAddressDto>();
            CreateMap<MachineCheckRequest, MachineCheckRequestDto>()
                .ForMember(dest => dest.SerialNumber,
                opt => opt.MapFrom(src => src.Contract != null
                            ? src.Contract.SerialNumber
                            : null))
                .ForMember(dest => dest.ContractAddress,
                 opt => opt.MapFrom(src => src.Contract != null && src.Contract.RentingRequest != null && src.Contract.RentingRequest.RentingRequestAddress != null
                            ? src.Contract.RentingRequest.RentingRequestAddress
                            : null));

            CreateMap<MachineCheckRequestCriteria, MachineCheckRequestCriteriaDto>()
                .ForMember(dest => dest.CriteriaName,
                opt => opt.MapFrom(src => src.MachineCheckCriteria != null
                            ? src.MachineCheckCriteria.Name
                            : null));

            CreateMap<MachineCheckCriteria, MachineCheckCriteriaDto>();

            CreateMap<RequestResponse, RequestResponseDto>();



            CreateMap<MachineTask, MachineTaskDto>()
                .ForMember(dest => dest.StaffName,
                opt => opt.MapFrom(src => src.Staff != null
                            ? src.Staff.Name
                            : null))
                .ForMember(dest => dest.ManagerName,
                opt => opt.MapFrom(src => src.Manager != null
                            ? src.Manager.Name
                            : null));

            CreateMap<MachineTask, MachineTaskDisplayDetail>()
                .ForMember(dest => dest.MachineCheckRequestId,
                opt => opt.MapFrom(src => src.RequestResponse != null
                            ? src.RequestResponse.MachineCheckRequestId
                            : null));
            //.ForMember(dest => dest.TaskLogs,
            //opt => opt.MapFrom(src => src.MachineTaskLogs != null
            //            ? src.MachineTaskLogs
            //            : null))
            //.ForMember(dest => dest.ComponentReplacementTicketCreateFromTaskList,
            //opt => opt.MapFrom(src => src.ComponentReplacementTicketsCreateFromTask != null
            //            ? src.ComponentReplacementTicketsCreateFromTask
            //            : null));

            CreateMap<MachineTaskLog, MachineTaskLogDto>()
            .ForMember(dest => dest.AccountTriggerName,
                   opt => opt.MapFrom(src => src.AccountTrigger != null ? src.AccountTrigger.Name : null));

            CreateMap<ComponentReplacementTicket, ComponentReplacementTicketDto>()
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

            CreateMap<Role, RoleDto>();
        }
    }
}
