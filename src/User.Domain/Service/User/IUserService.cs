using User.Domain.Service.User.Dto;

namespace User.Domain.Service.User
{
    public interface IUserService
    {
        Task<UserDto> PostRegister(UserDto userDto);
        IEnumerable<UserDto> Get();
        UserDto GetById(int id);
        UserDto PostLogin(UserLoginDto user);
        bool PutChangeData(UserDto user);
        bool PutChangePassword(UserDto user);
    }
}
