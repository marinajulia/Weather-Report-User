using User.Domain.Common.ResponseAuth;
using User.Domain.Common.Validations.Base;
using User.Domain.Service.User.Dto;

namespace User.Domain.Service.User
{
    public interface IUserService
    {
        Task<Response> Create(CreateUserRequest userDto);
        Task<Response<List<UserDto>>> Get();
        Task<Response<UserDto>> GetById(string id);
        Task<Response<AuthResponse>> AuthAsync(UserLogin auth);
    }
}
