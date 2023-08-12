using User.Domain.Common.ResponseAuth;
using User.Domain.Common.Validations.Base;
using User.Domain.Service.User.Dto;
using User.Domain.Service.User.Entities;

namespace User.Domain.Service.User
{
    public interface IUserService
    {
        Task<Response> PostRegister(CreateUserRequest userDto);
        IEnumerable<CreateUserRequest> Get();
        CreateUserRequest GetById(int id);
        CreateUserRequest PostLogin(UserEntity user);
        bool PutChangeData(CreateUserRequest user);
        bool PutChangePassword(CreateUserRequest user);
        Task<Response<AuthResponse>> AuthAsync(UserEntity auth);
    }
}
