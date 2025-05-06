using KmtBackend.DAL.Entities;
using KmtBackend.API.DTOs.Auth;
using KmtBackend.API.DTOs.User;
using Mapster;
using System.Globalization;
using KmtBackend.Models.DTOs.User;
using KmtBackend.Models.DTOs.Title;
using KmtBackend.Models.DTOs.Department;

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
                .Map(dest => dest.Department, src => src.Department == null ? null : new DepartmentResponse
                {
                    Id = src.Department.Id,
                    // Use Arabic name if Arabic culture is current
                    Name = CultureInfo.CurrentCulture.Name.StartsWith("ar") && !string.IsNullOrEmpty(src.Department.NameAr)
                        ? src.Department.NameAr
                        : src.Department.Name,
                    Description = CultureInfo.CurrentCulture.Name.StartsWith("ar") && !string.IsNullOrEmpty(src.Department.DescriptionAr)
                        ? src.Department.DescriptionAr
                        : src.Department.Description,
                    CreatedAt = src.Department.CreatedAt,
                    UpdatedAt = src.Department.UpdatedAt,
                    UserCount = src.Department.Users.Count
                })
                .Map(dest => dest.Title, src => src.Title == null ? null : new TitleResponse
                {
                    Id = src.Title.Id,
                    // Use Arabic name if Arabic culture is current
                    Name = CultureInfo.CurrentCulture.Name.StartsWith("ar") && !string.IsNullOrEmpty(src.Title.NameAr)
                        ? src.Title.NameAr
                        : src.Title.Name,
                    Description = CultureInfo.CurrentCulture.Name.StartsWith("ar") && !string.IsNullOrEmpty(src.Title.DescriptionAr)
                        ? src.Title.DescriptionAr
                        : src.Title.Description,
                    CreatedAt = src.Title.CreatedAt,
                    UpdatedAt = src.Title.UpdatedAt,
                    UserCount = src.Title.Users.Count
                });

            // User to UserDto mapping (for auth response)
            config.NewConfig<User, UserDto>()
                // Conditionally map department if exists
                .Map(dest => dest.Department, src => src.Department == null ? null : new DepartmentDto
                {
                    Id = src.Department.Id,
                    // Use Arabic name if Arabic culture is current
                    Name = CultureInfo.CurrentCulture.Name.StartsWith("ar") && !string.IsNullOrEmpty(src.Department.NameAr)
                        ? src.Department.NameAr
                        : src.Department.Name,
                    Description = CultureInfo.CurrentCulture.Name.StartsWith("ar") && !string.IsNullOrEmpty(src.Department.DescriptionAr)
                        ? src.Department.DescriptionAr
                        : src.Department.Description
                })
                .Map(dest => dest.Title, src => src.Title == null
                ? null
                : CultureInfo.CurrentCulture.Name.StartsWith("ar") && !string.IsNullOrEmpty(src.Department.NameAr)
                        ? src.Department.NameAr
                        : src.Department.Name);

            // CreateUserRequest to User mapping
            config.NewConfig<CreateUserRequest, User>()
                // Don't map password directly - it's handled in service
                .Ignore(dest => dest.PasswordHash)
                // Set creation timestamp
                .Map(dest => dest.CreatedAt, src => DateTime.UtcNow);
        }
    }
}

