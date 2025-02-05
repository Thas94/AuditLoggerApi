using AuditLog.API.Models;
using AutoMapper;

namespace AuditLog.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<UserModel, UserIdentity>()
                .ForMember(u => u.UserName, dest => dest.MapFrom(x => x.Email));
        }
    }
}
