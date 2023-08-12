using User.Domain.Common.ResponseAuth;
using User.Domain.Common.Validations.Base;
using User.Domain.Service.User.Dto;
using User.Domain.Service.User.Entities;

namespace User.Domain.Service.User
{
    public interface IUserService
    {
        Task<Response> Create(CreateUserRequest userDto);
        IEnumerable<CreateUserRequest> Get();
        CreateUserRequest GetById(int id);
        Task<Response<AuthResponse>> AuthAsync(UserLogin auth);
    }
}
