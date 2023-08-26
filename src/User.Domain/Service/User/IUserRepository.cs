using User.Domain.Service.User.Dto;
using User.Domain.Service.User.Entities;

namespace User.Domain.Service.User
{
    public interface IUserRepository
    {
        UserEntity GetById(string id);
        Task<UserEntity> Register(UserEntity user);
        IEnumerable<UserEntity> Get();
        Task<bool> ExistsByEmailAsync(string email);
        Task<UserEntity> GetByEmailAsync(string email);
    }
}
