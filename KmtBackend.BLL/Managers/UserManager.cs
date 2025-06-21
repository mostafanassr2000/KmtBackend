using KmtBackend.BLL.Managers.Interfaces;
using KmtBackend.DAL.Entities;
using KmtBackend.DAL.Repositories.Interfaces;
using KmtBackend.Infrastructure.Helpers;
using KmtBackend.Models.DTOs.Common;
using KmtBackend.Models.DTOs.User;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;

namespace KmtBackend.BLL.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentFilteringService _departmentFilteringService;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly ILeaveBalanceManager _leaveBalanceManager;

        public UserManager(
            IUserRepository userRepository, 
            IDepartmentFilteringService departmentFilteringService,
            IMapper mapper, 
            ILeaveBalanceManager leaveBalanceManager)
        {
            _userRepository = userRepository;
            _departmentFilteringService = departmentFilteringService;
            _mapper = mapper;
            _passwordHasher = new PasswordHasher<User>();
            _leaveBalanceManager = leaveBalanceManager;
        }

        public async Task<UserResponse?> GetUserByIdAsync(Guid id, Guid currentUserId)
        {
            var currentUser = await _userRepository.GetUserWithRolesAsync(currentUserId);
            if (currentUser == null) return null;

            var accessibleUserIds = await _departmentFilteringService.GetAccessibleUserIdsAsync(currentUserId);
            
            // If empty, it means super admin (no filtering)
            if (!accessibleUserIds.Any())
            {
                var user = await _userRepository.GetByIdAsync(id);
                return user != null ? _mapper.Map<UserResponse>(user) : null;
            }
            else
            {
                // Check if the requested user is in accessible users
                if (!accessibleUserIds.Contains(id))
                    return null;

                var user = await _userRepository.GetByIdAsync(id);
                return user != null ? _mapper.Map<UserResponse>(user) : null;
            }
        }

        public async Task<PaginatedResult<UserResponse>> GetAllUsersPaginatedAsync(PaginationQuery pagination, Guid currentUserId)
        {
            var currentUser = await _userRepository.GetUserWithRolesAsync(currentUserId);
            if (currentUser == null)
                return new PaginatedResult<UserResponse> { Items = new List<UserResponse>() };

            var accessibleUserIds = await _departmentFilteringService.GetAccessibleUserIdsAsync(currentUserId);
                        
            // If empty, it means super admin (no filtering)
            if (!accessibleUserIds.Any())
            {
                var users = await _userRepository.GetAllPaginatedAsync(pagination);
                var responses = _mapper.Map<IEnumerable<UserResponse>>(users.Items).ToList();

                return new PaginatedResult<UserResponse>
                {
                    Items = responses,
                    PageNumber = users.PageNumber,
                    PageSize = users.PageSize,
                    TotalRecords = users.TotalRecords
                };
            }
            else
            {
                // Filter by accessible users
                var allUsers = await _userRepository.GetByIdsAsync(accessibleUserIds);
                var responses = _mapper.Map<IEnumerable<UserResponse>>(allUsers).ToList();

                // Apply pagination manually
                var totalCount = responses.Count;
                var items = responses
                    .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToList();

                return new PaginatedResult<UserResponse>
                {
                    Items = items,
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRecords = totalCount
                };
            }
        }

        // Keep existing methods for backward compatibility
        public async Task<UserResponse?> GetUserByIdAsync(Guid id)
        {
            return await GetUserByIdAsync(id, Guid.Empty);
        }

        public async Task<PaginatedResult<UserResponse>> GetAllUsersPaginatedAsync(PaginationQuery pagination)
        {
            return await GetAllUsersPaginatedAsync(pagination, Guid.Empty);
        }

        public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                if (await _userRepository.EmailExistsAsync(request.Email))
                {
                    throw new Exception("Email already exists");
                }
            }

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                if (await _userRepository.PhoneNumberExistsAsync(PhoneNumberHelper.Normalize(request.PhoneNumber)))
                {
                    throw new Exception("Phone number already exists");
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                if (await _userRepository.UsernameExistsAsync(request.Username))
                {
                    throw new Exception("Username already exists");
                }
            }

            var user = _mapper.Map<User>(request);
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            var createdUser = await _userRepository.CreateAsync(user);

            await _leaveBalanceManager.CreateInitialBalancesForUserAsync(createdUser.Id);

            return _mapper.Map<UserResponse>(createdUser);
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserResponse>>(users);
        }

        public async Task<UserResponse> UpdateUserAsync(Guid id, UpdateUserRequest request)
        {
            var user = await _userRepository.GetByIdAsync(id) ?? throw new Exception("User not found");

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                if (await _userRepository.EmailExistsAsync(request.Email))
                {
                    throw new Exception("Email already exists");
                }
            }

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                if (await _userRepository.PhoneNumberExistsAsync(PhoneNumberHelper.Normalize(request.PhoneNumber)))
                {
                    throw new Exception("Phone number already exists");
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                if (await _userRepository.UsernameExistsAsync(request.Username))
                {
                    throw new Exception("Username already exists");
                }
            }

            user.Username = request.Username ?? user.Username;
            user.Email = request.Email ?? user.Email;
            user.TitleId = request.TitleId ?? user.TitleId;
            user.DepartmentId = request.DepartmentId ?? user.DepartmentId;
            user.TerminationDate = request.TerminationDate ?? user.TerminationDate;
            user.HireDate = request.HireDate ?? user.HireDate;
            user.PriorWorkExperienceMonths = request.PriorWorkExperienceMonths ?? user.PriorWorkExperienceMonths;
            user.BirthDate = request.BirthDate ?? user.BirthDate;

            if (request.PhoneNumber != null)
            {
                user.PhoneNumber = PhoneNumberHelper.Normalize(request.PhoneNumber ?? "");
            }

            var updatedUser = await _userRepository.UpdateAsync(user);
            
            return _mapper.Map<UserResponse>(updatedUser);
        }

        public async Task<UserResponse> UpdateUserPasswordAsync(Guid id, UpdateUserPasswordRequest request)
        {
            var user = await _userRepository.GetByIdAsync(id) ?? throw new Exception("User not found");

            if (!string.IsNullOrEmpty(request.Password))
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            }

            var updatedUser = await _userRepository.UpdateAsync(user);

            return _mapper.Map<UserResponse>(updatedUser);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            return await _userRepository.DeleteAsync(id);
        }
    }
}
