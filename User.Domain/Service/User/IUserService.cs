using User.Domain.Service.User.Dto;

namespace User.Domain.Service.User
{
    public interface IUserService
    {
        Task<UserDto> PostRegister(UserDto userDto);
        bool Allow(int idUser);
        IEnumerable<UserDto> Get();
        UserDto GetById(int id);
        bool PostBlock(UserDto user);
        bool PostUnlock(UserDto user);
        UserDto PostLogin(UserLoginDto user);
        bool PutChangeData(UserDto user);
        bool PutChangePassword(UserDto user);
    }
}
