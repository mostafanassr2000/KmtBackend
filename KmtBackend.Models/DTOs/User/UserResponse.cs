using KmtBackend.API.DTOs.Department;
using KmtBackend.Models.DTOs.Title;

namespace KmtBackend.Models.DTOs.User
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

        // Phone Number
        public string PhoneNumber { get; set; } = null!;

        // Department info
        public DepartmentResponse? Department { get; set; }

        // Title info
        public TitleResponse? Title { get; set; }

        // Creation date
        public DateTime CreatedAt { get; set; }

        // Last update date
        public DateTime? UpdatedAt { get; set; }
    }
}
