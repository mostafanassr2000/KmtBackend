using KmtBackend.DAL.Entities;

namespace KmtBackend.DAL.Repositories.Interfaces
{
    // Title repository interface
    public interface ITitleRepository
    {
        // Get title by ID
        Task<Title?> GetByIdAsync(Guid id);
        
        // Get all titles
        Task<IEnumerable<Title>> GetAllAsync();
        
        // Create new title
        Task<Title> CreateAsync(Title title);
        
        // Update existing title
        Task<Title> UpdateAsync(Title title);
        
        // Delete title
        Task<bool> DeleteAsync(Guid id);
    }
}

