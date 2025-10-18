using Microsoft.EntityFrameworkCore;
using UESAN.Ecommerce.CORE.Core.Entities;
using UESAN.Ecommerce.CORE.Core.Interfaces;
using UESAN.Ecommerce.CORE.Infrastructure.Data;

namespace UESAN.Ecommerce.CORE.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly StoreDbContext _context;

        public UserRepository(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<int> InsertUser(User user)
        {
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _context.User.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.User.ToListAsync();
        }

        public async Task UpdateUser(User user)
        {
            var existing = await _context.User.FindAsync(user.Id);
            if (existing != null)
            {
                existing.FirstName = user.FirstName;
                existing.LastName = user.LastName;
                existing.DateOfBirth = user.DateOfBirth;
                existing.Country = user.Country;
                existing.Address = user.Address;
                existing.Email = user.Email;
                existing.Password = user.Password;
                existing.IsActive = user.IsActive;
                existing.Type = user.Type;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteUser(int id)
        {
            var existing = await _context.User.FindAsync(id);
            if (existing != null)
            {
                _context.User.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
    }
}
