using KmtBackend.DAL.Context;
using KmtBackend.DAL.Entities;
using KmtBackend.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KmtBackend.DAL.Repositories
{
    // Title repository implementation
    public class TitleRepository : ITitleRepository
    {
        // Database context
        private readonly KmtDbContext _context;
        
        // Constructor with DI
        public TitleRepository(KmtDbContext context)
        {
            // Store context
            _context = context;
        }

        // Get title by ID
        public async Task<Title?> GetByIdAsync(Guid id)
        {
            return await _context.Titles
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        // Get all titles
        public async Task<IEnumerable<Title>> GetAllAsync()
        {
            return await _context.Titles.ToListAsync();
        }

        // Create new title
        public async Task<Title> CreateAsync(Title title)
        {
            // Add to context
            await _context.Titles.AddAsync(title);
            // Save changes
            await _context.SaveChangesAsync();
            // Return created title
            return title;
        }

        // Update existing title
        public async Task<Title> UpdateAsync(Title title)
        {
            _context.Titles.Update(title);
            title.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return title;
        }

        // Delete title
        public async Task<bool> DeleteAsync(Guid id)
        {
            var title = await _context.Titles.FindAsync(id);

            // Return false if not found
            if (title == null) return false;
            
            // Remove from context
            _context.Titles.Remove(title);
            // Save and return result
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

