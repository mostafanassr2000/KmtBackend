namespace KmtBackend.API.DTOs.Department
{
    // Department response DTO
    public class DepartmentResponse
    {
        // Department ID
        public Guid Id { get; set; }
        
        // Department name (language-specific)
        public string Name { get; set; } = null!;
        
        // Optional description
        public string? Description { get; set; }
        
        // Creation date
        public DateTime CreatedAt { get; set; }
        
        // Last update date
        public DateTime? UpdatedAt { get; set; }
        
        // Count of users in department
        public int UserCount { get; set; }
    }
}

