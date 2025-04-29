namespace KmtBackend.API.DTOs.User
{
    // User response DTO
    public class UserResponse
    {
        // User ID
        public Guid Id { get; set; }
        
        // Username
        public string Username { get; set; } = null!;
        
        // Email address
        public string Email { get; set; } = null!;
        
        // Role (Admin/User)
        public string Role { get; set; } = null!;
        
        // Job title
        public string? Title { get; set; }
        
        // Department info
        public DepartmentResponse? Department { get; set; }
        
        // Creation date
        public DateTime CreatedAt { get; set; }
        
        // Last update date
        public DateTime? UpdatedAt { get; set; }
    }

    // Department response DTO
    public class DepartmentResponse
    {
        // Department ID
        public Guid Id { get; set; }
        
        // Name (language specific)
        public string Name { get; set; } = null!;
    }
}
