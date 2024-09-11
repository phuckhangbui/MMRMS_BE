using AutoMapper;
using BusinessObject;
using DTOs.Account;

namespace Repository.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, AccountBaseDto>();
            CreateMap<Account, StaffAndManagerAccountDto>();
            CreateMap<Account, CustomerAccountDto>();
            CreateMap<Account, NewCustomerAccountDto>().ReverseMap();
            CreateMap<Account, NewStaffAndManagerAccountDto>().ReverseMap();
        }
    }
}
