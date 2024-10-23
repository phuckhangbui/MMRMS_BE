using DTOs.Role;

namespace Service.Interface
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetRoles();
    }
}
