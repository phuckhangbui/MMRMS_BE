using AutoMapper;
using DAO;
using DTOs.Role;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;

namespace Repository.Implement
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IMapper _mapper;

        public RoleRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDto>> GetRoles()
        {
            var roles = await RoleDao.Instance.GetAllAsync();
            if (!roles.IsNullOrEmpty())
            {
                return _mapper.Map<IEnumerable<RoleDto>>(roles);
            }

            return [];
        }
    }
}
