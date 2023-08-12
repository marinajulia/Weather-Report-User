using AutoMapper;
using User.Domain.Service.User.Dto;
using User.Domain.Service.User.Entities;

namespace User.Domain.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public static IMapper Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<CreateUserRequest, UserEntity>()
                .ForMember(target => target.Password, opt => opt.MapFrom(source => source.Password));
                cfg.CreateMap<UserEntity, UserLogin>();

                cfg.CreateMap<UserEntity, CreateUserRequest>();
                cfg.CreateMap<CreateUserRequest, UserEntity>();

            });

            IMapper mapper = config.CreateMapper();
            return mapper;
        }
    }
}
