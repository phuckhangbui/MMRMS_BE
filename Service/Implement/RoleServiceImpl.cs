using DTOs.Role;
using Repository.Interface;
using Service.Interface;

namespace Service.Implement
{
    public class RoleServiceImpl : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleServiceImpl(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<RoleDto>> GetRoles()
        {
            return await _roleRepository.GetRoles();
        }
    }
}
