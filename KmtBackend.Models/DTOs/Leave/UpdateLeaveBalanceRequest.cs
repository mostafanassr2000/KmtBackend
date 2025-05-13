namespace KmtBackend.Models.DTOs.Leave
{
    public class UpdateLeaveBalanceRequest
    {
        public int? TotalDays { get; set; } = null;
        public int? UsedDays { get; set; } = null;
    }
}
