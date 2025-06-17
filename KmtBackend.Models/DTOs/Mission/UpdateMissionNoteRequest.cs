using System.ComponentModel.DataAnnotations;

namespace KmtBackend.Models.DTOs.Mission
{
    public class UpdateMissionNoteRequest
    {
        [MaxLength(500)]
        public required string Notes { get; set; }
    }
}
