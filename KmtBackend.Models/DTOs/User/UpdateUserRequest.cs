namespace KmtBackend.Models.DTOs.User
{
    // Update user request
    public class UpdateUserRequest
    {
        // Updated username
        public string Username { get; set; } = null!;

        // Updated email
        public string Email { get; set; } = null!;

        // Optional new password
        public string? Password { get; set; }

        // Updated job title
        public Guid? TitleId { get; set; }

        // Updated department
        public Guid? DepartmentId { get; set; }
    }
}
