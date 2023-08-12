using User.Domain.Service.User.Dto;
using User.Domain.Service.User.Entities;
using User.Domain.Service.User;
using User.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace User.Infra.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;
        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }
        public bool Allow(int idUser)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserEntity> Get()
        {
            var user = _context.User.AsNoTracking();
            return user.ToList();
        }

        public UserEntity GetById(int id)
        {
            var usuario = _context.User.FirstOrDefault(x => x.Id == id);
            return usuario;
        }

        public UserEntity GetUser(string email, string password)
        {
            var user = _context.User.FirstOrDefault(x => x.Email == email);
            return user;
        }

        public async Task<UserEntity> PostRegister(UserEntity user)
        {

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return user;
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
