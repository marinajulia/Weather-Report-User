using User.Domain.Service.User.Dto;
using User.Domain.Service.User.Entities;

namespace User.Domain.Service.User
{
    public interface IUserRepository
    {
        UserEntity GetById(int id);
        Task<UserEntity> PostRegister(UserEntity user);
        bool Allow(int idUser);
        IEnumerable<UserEntity> Get();
        bool PostBlock(UserDto user);
        bool PostUnlock(UserDto user);
        bool PutChangeData(UserDto user);
        bool PutChangePassword(UserDto user);
        UserEntity GetUser(string email, string password);
    }
}
