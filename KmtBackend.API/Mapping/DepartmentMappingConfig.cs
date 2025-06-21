using KmtBackend.DAL.Entities;
using KmtBackend.Models.DTOs.Department;
using Mapster;

namespace KmtBackend.API.Mapping
{
    public class DepartmentMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Department, DepartmentResponse>()
                .Map(dest => dest.HeadOfDepartmentId, src => src.HeadOfDepartmentId);
        }
    }
}