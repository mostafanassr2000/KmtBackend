namespace KmtBackend.API.DTOs.Auth
{
    // Login request data
    public class LoginRequest
    {
        // Email for authentication
        public string Email { get; set; } = null!;
        
        // Password (plaintext for request only)
        public string Password { get; set; } = null!;
    }
}
