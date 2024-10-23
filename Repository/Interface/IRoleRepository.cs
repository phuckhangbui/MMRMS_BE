using DTOs.Role;

namespace Repository.Interface
{
    public interface IRoleRepository
    {
        Task<IEnumerable<RoleDto>> GetRoles();
    }
}
