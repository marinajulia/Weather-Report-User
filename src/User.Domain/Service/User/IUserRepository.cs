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
        bool PutChangeData(CreateUserRequest user);
        bool PutChangePassword(CreateUserRequest user);
        UserEntity GetUser(string email, string password);
        Task<bool> ExistsByEmailAsync(string email);
        Task<UserEntity> GetByEmailAsync(string email);
    }
}
