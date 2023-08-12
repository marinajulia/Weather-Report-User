using User.Domain.Common.ResponseAuth;
using User.Domain.Service.User.Entities;

namespace User.Domain.Common.Security
{
    public interface ITokenManager
    {
        Task<AuthResponse> GenerateTokenAsync(UserEntity user);
    }
}
