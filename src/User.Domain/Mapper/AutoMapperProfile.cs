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

                cfg.CreateMap<UserEntity, UserLogin>();
                cfg.CreateMap<UserLogin, UserEntity>();

                cfg.CreateMap<UserEntity, CreateUserRequest>();
                cfg.CreateMap<CreateUserRequest, UserEntity>();

                cfg.CreateMap<UserEntity, UserDto>();
                cfg.CreateMap<UserDto, UserEntity>();


            });

            IMapper mapper = config.CreateMapper();
            return mapper;
        }
    }
}
