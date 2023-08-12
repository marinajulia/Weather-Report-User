using User.Domain.Common.Validations.Base;
using User.Domain.Service.User.Entities;

namespace User.Domain.Common.Security
{
    public interface ISecurityService
    {
        Task<Response<bool>> ComparePassword(string password, string confirmPassword);
        Task<Response<bool>> VerifyPassword(string password, UserEntity user);
        Task<Response<string>> EncryptPassword(string password);
    }
}
