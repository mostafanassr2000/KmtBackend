namespace KmtBackend.Models.DTOs.Department
{
    // Department response DTO
    public class DepartmentResponse
    {
        // Department ID
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;
        public string NameAr { get; set; } = null!;

        // Optional description
        public string? Description { get; set; }
        public string? DescriptionAr { get; set; }

        // Creation date
        public DateTime CreatedAt { get; set; }

        // Last update date
        public DateTime? UpdatedAt { get; set; }

        // Count of users in department
        public int UserCount { get; set; }
    }
}

