using UESAN.Ecommerce.CORE.Core.Entities;

namespace UESAN.Ecommerce.CORE.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<int> InsertUser(User user);
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserById(int id);
        Task<IEnumerable<User>> GetUsers();
        Task UpdateUser(User user);
        Task DeleteUser(int id);
    }
}
