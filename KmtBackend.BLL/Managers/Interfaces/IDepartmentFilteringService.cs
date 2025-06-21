using KmtBackend.DAL.Entities;

namespace KmtBackend.BLL.Managers.Interfaces
{
    public interface IDepartmentFilteringService
    {
        bool IsSuperAdmin(User user);
        bool HasDepartmentAccess(User user, Guid departmentId);
        IEnumerable<Guid> GetAccessibleDepartmentIds(User user);
        IEnumerable<Guid> GetAccessibleUserIds(User user);
        Task<IEnumerable<Guid>> GetAccessibleUserIdsAsync(Guid currentUserId);
    }
} 