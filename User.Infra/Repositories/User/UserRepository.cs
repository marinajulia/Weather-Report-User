using User.Domain.Common.Cryptography;
using User.Domain.Service.User.Dto;
using User.Domain.Service.User.Entities;
using User.Domain.Service.User;
using User.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace User.Infra.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        public bool Allow(int idUser)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserEntity> Get()
        {
            using (var context = new ApplicationContext())
            {
                var user = context.User.AsNoTracking();
                return user.ToList();
            }
        }

        public UserEntity GetById(int id)
        {
            using (var context = new ApplicationContext())
            {
                var usuario = context.User.FirstOrDefault(x => x.Id == id);
                return usuario;
            }
        }

        public UserEntity GetUser(string email, string password)
        {
            using (var context = new ApplicationContext())
            {
                var user = context.User.FirstOrDefault(x => x.Email == email &&
                x.Password == PasswordService.Encrypt(password));
                return user;
            }
        }

        public bool PostBlock(UserDto user)
        {
            throw new NotImplementedException();
        }

        public async Task<UserEntity> PostRegister(UserEntity user)
        {
            using (var context = new ApplicationContext())
            {
                user.Password = PasswordService.Encrypt(user.Password);

                context.User.Add(user);
                await context.SaveChangesAsync();

                return user;
            }
        }

        public bool PostUnlock(UserDto user)
        {
            throw new NotImplementedException();
        }

        public bool PutChangeData(UserDto user)
        {
            throw new NotImplementedException();
        }

        public bool PutChangePassword(UserDto user)
        {
            throw new NotImplementedException();
        }
    }
}
