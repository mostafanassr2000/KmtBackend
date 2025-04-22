using KmtBackend.DAL.Entities;
// Domain models
using KmtBackend.API.DTOs.Auth;
// Auth DTOs
using KmtBackend.API.DTOs.User;
// User DTOs
using Mapster;
// Mapster mapping library
using System.Globalization;
// Culture info for localization

namespace KmtBackend.API.Mapping
{
    // Mapping configuration for User entities
    public class UserMappingConfig : IRegister
    {
        // Register mappings with Mapster
        public void Register(TypeAdapterConfig config)
        {
            // User to UserResponse mapping
            config.NewConfig<User, UserResponse>()
                // Map basic properties
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Username, src => src.Username)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.Role, src => src.Role)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt)
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
                // Conditionally map department if exists
                .Map(dest => dest.Department, src => src.Department == null ? null : new DepartmentResponse
                {
                    Id = src.Department.Id,
                    // Use Arabic name if Arabic culture is current
                    Name = CultureInfo.CurrentCulture.Name.StartsWith("ar") && !string.IsNullOrEmpty(src.Department.NameAr)
                        ? src.Department.NameAr
                        : src.Department.Name
                });

            // User to UserDto mapping (for auth response)
            config.NewConfig<User, UserDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Username, src => src.Username)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.Role, src => src.Role)
                .Map(dest => dest.Title, src => src.Title)
                // Conditionally map department if exists
                .Map(dest => dest.Department, src => src.Department == null ? null : new DepartmentDto
                {
                    Id = src.Department.Id,
                    // Use Arabic name if Arabic culture is current
                    Name = CultureInfo.CurrentCulture.Name.StartsWith("ar") && !string.IsNullOrEmpty(src.Department.NameAr)
                        ? src.Department.NameAr
                        : src.Department.Name
                });

            // CreateUserRequest to User mapping
            config.NewConfig<CreateUserRequest, User>()
                .Map(dest => dest.Username, src => src.Username)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.Role, src => src.Role)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.DepartmentId, src => src.DepartmentId)
                // Don't map password directly - it's handled in service
                .Ignore(dest => dest.PasswordHash)
                // Set creation timestamp
                .Map(dest => dest.CreatedAt, src => DateTime.UtcNow);

            // Department to DepartmentResponse mapping
            config.NewConfig<Department, DepartmentResponse>()
                .Map(dest => dest.Id, src => src.Id)
                // Use Arabic name if Arabic culture is current
                .Map(dest => dest.Name, src => CultureInfo.CurrentCulture.Name.StartsWith("ar") && !string.IsNullOrEmpty(src.NameAr)
                    ? src.NameAr
                    : src.Name);
        }
    }
}

