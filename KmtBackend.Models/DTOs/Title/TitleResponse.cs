namespace KmtBackend.Models.DTOs.Title
{
    // Title response DTO
    public class TitleResponse
    {
        // Department ID
        public Guid Id { get; set; }

        // Department name
        public string Name { get; set; } = null!;

        // Optional description
        public string? Description { get; set; }

        // Creation date
        public DateTime CreatedAt { get; set; }

        // Last update date
        public DateTime? UpdatedAt { get; set; }

        // Count of users having title
        public int UserCount { get; set; }
    }
}

