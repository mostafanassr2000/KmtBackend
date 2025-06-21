using KmtBackend.BLL.Managers.Interfaces;
using KmtBackend.DAL.Entities;
using KmtBackend.DAL.Repositories.Interfaces;

namespace KmtBackend.BLL.Managers
{
    public class DepartmentFilteringService : IDepartmentFilteringService
    {
        private readonly IUserRepository _userRepository;

        public DepartmentFilteringService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool IsSuperAdmin(User user)
        {
            return user.Roles.Any(r => r.Name == "Super Admin");
        }

        public bool HasDepartmentAccess(User user, Guid departmentId)
        {
            // Super admin has access to all departments
            if (IsSuperAdmin(user))
                return true;

            // User can access their assigned department
            return user.DepartmentId == departmentId;
        }

        public IEnumerable<Guid> GetAccessibleDepartmentIds(User user)
        {
            // Super admin has access to all departments
            if (IsSuperAdmin(user))
                return Enumerable.Empty<Guid>(); // Empty means no filtering (all departments)

            // Regular users can only access their assigned department
            if (user.DepartmentId.HasValue)
                return new[] { user.DepartmentId.Value };

            return Enumerable.Empty<Guid>();
        }

        public IEnumerable<Guid> GetAccessibleUserIds(User user)
        {
            // Super admin has access to all users
            if (IsSuperAdmin(user))
                return Enumerable.Empty<Guid>(); // Empty means no filtering (all users)

            // Regular users can only access users in their department
            if (user.DepartmentId.HasValue)
            {
                // This will be populated by the repository method
                return Enumerable.Empty<Guid>();
            }

            return Enumerable.Empty<Guid>();
        }

        public async Task<IEnumerable<Guid>> GetAccessibleUserIdsAsync(Guid currentUserId)
        {
            var user = await _userRepository.GetUserWithRolesAsync(currentUserId);
            if (user == null)
                return Enumerable.Empty<Guid>();

            // Super admin has access to all users
            if (IsSuperAdmin(user))
            {
                Console.WriteLine("Super Admin");
                return Enumerable.Empty<Guid>(); // Empty means no filtering
            }

            // Regular users can only access users in their department
            if (user.DepartmentId.HasValue)
            {
                var departmentUsers = await _userRepository.GetByDepartmentAsync(user.DepartmentId.Value);
                return departmentUsers.Select(u => u.Id);
            }

            return Enumerable.Empty<Guid>();
        }
    }
}