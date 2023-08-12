using User.Domain.Common.Validations.Base;
using User.Domain.Service.User.Entities;

namespace User.Domain.Common.Security
{
    public class SecurityService : ISecurityService
    {
        public Task<Response<bool>> ComparePassword(string password, string confirmPassword)
        {
            var isEquals = password.Trim().Equals(confirmPassword.Trim());

            return Task.FromResult(Response.OK<bool>(isEquals));
        }

        public Task<Response<string>> EncryptPassword(string password)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            return Task.FromResult(Response.OK<string>(passwordHash));
        }

        public Task<Response<bool>> VerifyPassword(string password, UserEntity user)
        {
            bool validPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);

            return Task.FromResult(Response.OK<bool>(validPassword));
        }
    }
}
