using UESAN.Ecommerce.CORE.Core.DTOs;

namespace UESAN.Ecommerce.CORE.Core.Interfaces
{
    public interface IUserService
    {
        Task<int> InsertUser(UserCreateDTO userCreateDTO);
        Task<UserListDTO> GetUserById(int id);
        Task<UserListDTO> GetUserByEmail(string email);
        Task<IEnumerable<UserListDTO>> GetUsers();
        Task<int> SignUp(UserCreateDTO userCreateDTO);
        Task<UserListDTO> SignIn(UserSignInDTO userSignInDTO);
    }
}
