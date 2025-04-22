using KmtBackend.API.Infrastructure.Auth;
using KmtBackend.DAL.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KmtBackend.Infrastructure.Auth
{
    // Implementation of token generator
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        // JWT configuration
        private readonly JwtSettings _jwtSettings;
        
        // Inject configuration
        public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
        {
            // Store settings for use in token generation
            _jwtSettings = jwtSettings.Value;
        }

        // Generate token from user entity
        public string GenerateToken(User user)
        {
            // Security key derived from our secret
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                SecurityAlgorithms.HmacSha256);

            // Claims contain user information
            var claims = new[]
            {
                // Subject identifier
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                // Unique name for the user
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                // Email address
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                // Custom claim for role
                new Claim(ClaimTypes.Role, user.Role),
                // Prevent token reuse
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // Create the token with our parameters
            var securityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                claims: claims,
                signingCredentials: signingCredentials);

            // Convert token to string format
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
