using KmtBackend.API.DTOs.User;
using KmtBackend.BLL.Managers.Interfaces;
using KmtBackend.DAL.Entities;
using KmtBackend.DAL.Repositories.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;

namespace KmtBackend.BLL.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserManager(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
        {
            if (await _userRepository.EmailExistsAsync(request.Email))
            {
                throw new Exception("Email already exists");
            }

            if (await _userRepository.UsernameExistsAsync(request.Username))
            {
                throw new Exception("Username already exists");
            }

            var user = _mapper.Map<User>(request);
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            var createdUser = await _userRepository.CreateAsync(user);
            
            return _mapper.Map<UserResponse>(createdUser);
        }

        public async Task<UserResponse?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            
            if (user == null) return null;
            
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            
            return _mapper.Map<IEnumerable<UserResponse>>(users);
        }

        public async Task<UserResponse> UpdateUserAsync(int id, UpdateUserRequest request)
        {
            var user = await _userRepository.GetByIdAsync(id);
            
            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (request.Email != user.Email && 
                await _userRepository.EmailExistsAsync(request.Email))
            {
                throw new Exception("Email already exists");
            }

            if (request.Username != user.Username && 
                await _userRepository.UsernameExistsAsync(request.Username))
            {
                throw new Exception("Username already exists");
            }

            user.Username = request.Username;
            user.Email = request.Email;
            user.Title = request.Title;
            user.DepartmentId = request.DepartmentId;
            
            if (!string.IsNullOrEmpty(request.Password))
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            }

            var updatedUser = await _userRepository.UpdateAsync(user);
            
            return _mapper.Map<UserResponse>(updatedUser);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }
    }
}
