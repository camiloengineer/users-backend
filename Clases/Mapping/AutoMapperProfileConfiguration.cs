using AutoMapper;
using User.Backend.Api.Clases.Domain;
using User.Backend.Api.Clases.Dto;

namespace User.Backend.Api.Clases.Mapping
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
            : this("MyProfile")
        {
        }

        protected AutoMapperProfileConfiguration(string profileName)
        : base(profileName)
        {
            CreateMap<CUSTOMER, Customer>()
                        .ForMember(vm => vm.Birthdate, domain => domain.MapFrom(s => s.CUST_BIRTHDAY))
                        .ForMember(vm => vm.DNI, domain => domain.MapFrom(s => s.CUST_DNI))
                        .ForMember(vm => vm.Email, domain => domain.MapFrom(s => s.CUST_EMAIL))
                        .ForMember(vm => vm.Guid, domain => domain.MapFrom(s => s.CUST_GUID))
                        .ForMember(vm => vm.Name, domain => domain.MapFrom(s => s.CUST_NAME))
                        .ForMember(vm => vm.Phone, domain => domain.MapFrom(s => s.CUST_PHONE))
                        .ForMember(vm => vm.Active, domain => domain.MapFrom(s => s.CUST_ACTIVE))
                        .ForMember(vm => vm.Password, domain => domain.MapFrom(s => s.CUST_PASSWORD))
                        .ForMember(vm => vm.Avatar, domain => domain.MapFrom(s => s.CUST_AVATAR));
        }
    }
}
