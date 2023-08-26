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

        public IEnumerable<UserEntity> Get()
        {
            return _context.User.AsNoTracking().ToList();
        }

        public UserEntity GetById(string id)
        { 
            return _context.User.FirstOrDefault(x => x.Id == id);
        }

        public async Task<UserEntity> Register(UserEntity user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            var user = _context.User.FirstOrDefault(x => x.Email == email);
            if(user != null)
                return true;
            return false;
        }
        public async Task<UserEntity> GetByEmailAsync(string email)
        {
            var user = _context.User.FirstOrDefault(x => x.Email == email);

            return user;
        }

    }
}
