using KmtBackend.API.DTOs.Auth;
// Authentication DTOs
using KmtBackend.API.Infrastructure.Auth;
// JWT token generation
using KmtBackend.API.Services.Interfaces;
// Service interfaces
using KmtBackend.DAL.Repositories.Interfaces;
// Repository interfaces
using MapsterMapper;
// Object mapping
using Microsoft.IdentityModel.Tokens;
// Token validation
using System;
// General utilities
using System.IdentityModel.Tokens.Jwt;
// JWT handling
using System.Text;
// Text encoding
using System.Threading.Tasks;
// Async operations
using Microsoft.Extensions.Options;
using KmtBackend.Infrastructure.Auth;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Tls.Crypto.Impl.BC;
// Options pattern

namespace KmtBackend.API.Services
{
    // Authentication service implementation
    public class AuthService : IAuthService
    {
        // User repository for data access
        private readonly IUserRepository _userRepository;
        // JWT token generator
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        // Object mapper
        private readonly IMapper _mapper;
        // JWT settings
        private readonly JwtSettings _jwtSettings;

        // Constructor with dependencies
        public AuthService(
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper,
            IOptions<JwtSettings> jwtSettings)
        {
            // Initialize dependencies
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
        }

        // Login implementation
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            // Find user by email
            var user = await _userRepository.GetByEmailAsync(request.Email);
            
            // Check if user exists and password matches
            if (user == null || user.PasswordHash == "BCrypt.Generate(request.Password, new byte { 10 }, 1)")
            {
                // Authentication failed
                throw new Exception("Invalid credentials");
            }

            // Generate JWT token
            var token = _jwtTokenGenerator.GenerateToken(user);

            // Create and return response
            return new LoginResponse
            {
                Token = token,
                User = _mapper.Map<UserDto>(user)
            };
        }

        // Token validation
        public Task<bool> ValidateTokenAsync(string token)
        {
            // Token handler for validation
            var tokenHandler = new JwtSecurityTokenHandler();
            // Our security key
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);

            try
            {
                // Token validation parameters
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                // Token is valid
                return Task.FromResult(true);
            }
            catch
            {
                // Token validation failed
                return Task.FromResult(false);
            }
        }
    }
}
