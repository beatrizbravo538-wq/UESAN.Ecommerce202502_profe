using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UESAN.Ecommerce.CORE.Core.DTOs;
using UESAN.Ecommerce.CORE.Core.Entities;
using UESAN.Ecommerce.CORE.Core.Interfaces;

namespace UESAN.Ecommerce.CORE.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> InsertUser(UserCreateDTO userCreateDTO)
        {
            var user = new User
            {
                FirstName = userCreateDTO.FirstName,
                LastName = userCreateDTO.LastName,
                DateOfBirth = userCreateDTO.DateOfBirth,
                Country = userCreateDTO.Country,
                Address = userCreateDTO.Address,
                Email = userCreateDTO.Email,
                Password = userCreateDTO.Password,
                IsActive = true,
                Type = userCreateDTO.Type
            };

            return await _userRepository.InsertUser(user);
        }

        public async Task<UserListDTO> GetUserById(int id)
        {
            var u = await _userRepository.GetUserById(id);
            if (u == null) return null;
            return new UserListDTO
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                IsActive = u.IsActive,
                Type = u.Type
            };
        }

        public async Task<UserListDTO> GetUserByEmail(string email)
        {
            var u = await _userRepository.GetUserByEmail(email);
            if (u == null) return null;
            return new UserListDTO
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                IsActive = u.IsActive,
                Type = u.Type
            };
        }

        public async Task<IEnumerable<UserListDTO>> GetUsers()
        {
            var users = await _userRepository.GetUsers();
            var list = new List<UserListDTO>();
            foreach (var u in users)
            {
                list.Add(new UserListDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    Type = u.Type
                });
            }
            return list;
        }

        public async Task<int> SignUp(UserCreateDTO userCreateDTO)
        {
            // basic validations
            if (string.IsNullOrEmpty(userCreateDTO.Email) || string.IsNullOrEmpty(userCreateDTO.Password))
                throw new ArgumentException("Email and Password are required");

            var existing = await _userRepository.GetUserByEmail(userCreateDTO.Email);
            if (existing != null)
                throw new InvalidOperationException("User with this email already exists");

            var user = new User
            {
                FirstName = userCreateDTO.FirstName,
                LastName = userCreateDTO.LastName,
                DateOfBirth = userCreateDTO.DateOfBirth,
                Country = userCreateDTO.Country,
                Address = userCreateDTO.Address,
                Email = userCreateDTO.Email,
                Password = userCreateDTO.Password, // in production hash this password
                IsActive = true,
                Type = userCreateDTO.Type ?? "user"
            };

            return await _userRepository.InsertUser(user);
        }

        // helper to verify password - currently plain text, but centralizes logic for future hashing
        private bool VerifyPassword(string storedPassword, string providedPassword)
        {
            // simple equality for now; replace with hashing verification in production
            return string.Equals(storedPassword, providedPassword);
        }

        public async Task<UserListDTO> SignIn(UserSignInDTO userSignInDTO)
        {
            if (string.IsNullOrEmpty(userSignInDTO.Email) || string.IsNullOrEmpty(userSignInDTO.Password))
                throw new ArgumentException("Email and Password are required");

            var user = await _userRepository.GetUserByEmail(userSignInDTO.Email);
            if (user == null) return null;

            if (!VerifyPassword(user.Password ?? string.Empty, userSignInDTO.Password)) return null;

            return new UserListDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsActive = user.IsActive,
                Type = user.Type
            };
        }
    }
}
