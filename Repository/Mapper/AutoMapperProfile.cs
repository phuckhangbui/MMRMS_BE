using AutoMapper;
using BusinessObject;
using DTOs.Account;
using DTOs.Category;
using DTOs.Component;
using DTOs.Content;
using DTOs.Product;

namespace Repository.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, AccountBaseDto>();
            CreateMap<Account, StaffAndManagerAccountDto>();
            CreateMap<Account, CustomerAccountDto>()
                .ForMember(dest => dest.BusinessType, opt => opt.MapFrom(src => src.BusinessType))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.AccountBusinesses.FirstOrDefault().Company))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AccountBusinesses.FirstOrDefault().Address))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.AccountBusinesses.FirstOrDefault().Position))
                .ForMember(dest => dest.TaxNumber, opt => opt.MapFrom(src => src.AccountBusinesses.FirstOrDefault().TaxNumber));
            CreateMap<Account, NewCustomerAccountDto>().ReverseMap();
            CreateMap<Account, NewStaffAndManagerAccountDto>().ReverseMap();

            CreateMap<Category, CategoryDto>();
            CreateMap<Category, CategoryRequestDto>().ReverseMap();

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.ProductImageList, opt => opt.MapFrom(src => src.ProductImages));

            CreateMap<Product, DisplayProductDetailDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.ProductImageList, opt => opt.MapFrom(src => src.ProductImages))
                .ForMember(dest => dest.ProductAttributeList, opt => opt.MapFrom(src => src.ProductAttributes));

            CreateMap<ProductImage, ProductImageDto>();
            CreateMap<ProductAttribute, ProductAttributeDto>();
            CreateMap<ComponentProduct, ComponentProductDto>();

            CreateMap<Component, ComponentDto>();
            CreateMap<CreateComponentDto, Component>();


            CreateMap<SerialNumberProduct, SerialProductNumberDto>();

            CreateMap<Content, ContentDto>();
            CreateMap<Content, ContentCreateRequestDto>().ReverseMap();


        }
    }
}
