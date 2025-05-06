namespace KmtBackend.Models.DTOs.User
{
    // Update user request
    public class UpdateUserRequest
    {
        // Updated username
        public string? Username { get; set; }

        // Updated email
        public string? Email { get; set; }

        // Updated phone number
        public string? PhoneNumber { get; set; }

        // Updated job title
        public Guid? TitleId { get; set; }

        // Updated department
        public Guid? DepartmentId { get; set; }
    }
}
