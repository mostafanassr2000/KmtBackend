namespace KmtBackend.Models.DTOs.Title
{
    // Create title request
    public class CreateTitleRequest
    {
        // Department name (English)
        public string Name { get; set; } = null!;

        // Department name (Arabic)
        public string? NameAr { get; set; }

        // Optional description
        public string? Description { get; set; }
        public string? DescriptionAr { get; set; }
    }
}

