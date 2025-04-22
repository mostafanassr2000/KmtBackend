using KmtBackend.API.DTOs.User;
// User DTOs
using KmtBackend.API.Services.Interfaces;
using KmtBackend.DAL.Entities;

// Domain models
using KmtBackend.DAL.Repositories.Interfaces;
using MapsterMapper;
using Org.BouncyCastle.Crypto.Generators;


// Repository interfaces
// Object mapping
using System;
// General utilities
using System.Collections.Generic;
// Collections
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
// Async operations

namespace KmtBackend.API.Services
{
    // User service implementation
    public class UserService : IUserService
    {
        // Data access through repository
        private readonly IUserRepository _userRepository;
        // Object mapping
        private readonly IMapper _mapper;

        // Constructor with dependencies
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            // Store dependencies
            _userRepository = userRepository;
            _mapper = mapper;
        }

        // Create new user (by admin)
        public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
        {
            // Check if email already exists
            if (await _userRepository.EmailExistsAsync(request.Email))
            {
                // Business rule violation
                throw new Exception("Email already exists");
            }

            // Check if username already exists
            if (await _userRepository.UsernameExistsAsync(request.Username))
            {
                // Business rule violation
                throw new Exception("Username already exists");
            }

            // Map request to domain model
            var user = _mapper.Map<User>(request);
            // Hash the password
            user.PasswordHash = "BCrypt.Generate(request.Password, salt, 1)";
            
            // Create the user
            var createdUser = await _userRepository.CreateAsync(user);
            
            // Return mapped response
            return _mapper.Map<UserResponse>(createdUser);
        }

        // Get user by ID
        public async Task<UserResponse?> GetUserByIdAsync(int id)
        {
            // Find user
            var user = await _userRepository.GetByIdAsync(id);
            
            // Return null if not found
            if (user == null) return null;
            
            // Map and return
            return _mapper.Map<UserResponse>(user);
        }

        // Get all users
        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {
            // Get all users
            var users = await _userRepository.GetAllAsync();
            
            // Map collection to DTOs
            return _mapper.Map<IEnumerable<UserResponse>>(users);
        }

        // Update existing user
        public async Task<UserResponse> UpdateUserAsync(int id, UpdateUserRequest request)
        {
            // Find user to update
            var user = await _userRepository.GetByIdAsync(id);
            
            // Validate user exists
            if (user == null)
            {
                // Not found error
                throw new Exception("User not found");
            }

            // Check email uniqueness if changing
            if (request.Email != user.Email && 
                await _userRepository.EmailExistsAsync(request.Email))
            {
                // Business rule violation
                throw new Exception("Email already exists");
            }

            // Check username uniqueness if changing
            if (request.Username != user.Username && 
                await _userRepository.UsernameExistsAsync(request.Username))
            {
                // Business rule violation
                throw new Exception("Username already exists");
            }

            // Update properties
            user.Username = request.Username;
            user.Email = request.Email;
            user.Title = request.Title;
            user.DepartmentId = request.DepartmentId;
            
            // Update password if provided
            if (!string.IsNullOrEmpty(request.Password))
            {
                // Hash new password
                user.PasswordHash = "BCrypt.Generate(request.Password, salt, 1)";
            }

            // Update and save
            var updatedUser = await _userRepository.UpdateAsync(user);
            
            // Return mapped response
            return _mapper.Map<UserResponse>(updatedUser);
        }

        // Delete user
        public async Task<bool> DeleteUserAsync(int id)
        {
            // Delete and return result
            return await _userRepository.DeleteAsync(id);
        }
    }
}
