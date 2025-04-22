namespace KmtBackend.Infrastructure.Auth
{
    // Configuration class for JWT settings
    public class JwtSettings
    {
        // Secret key for signing tokens
        public string Secret { get; set; } = null!;
        
        // Token issuer identifier
        public string Issuer { get; set; } = null!;
        
        // Token audience identifier
        public string Audience { get; set; } = null!;
        
        // Token expiration time in minutes
        public int ExpiryMinutes { get; set; }
    }
}
