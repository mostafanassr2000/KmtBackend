namespace KmtBackend.API.DTOs.User
{
    // Create user request
    public class CreateUserRequest
    {
        // Username for login
        public string Username { get; set; } = null!;
        
        // Email for communication
        public string Email { get; set; } = null!;
        
        // Password (will be hashed)
        public string Password { get; set; } = null!;
        
        // Job title
        public Guid? TitleId { get; set; }
        
        // Department ID
        public Guid? DepartmentId { get; set; }
    }
}
