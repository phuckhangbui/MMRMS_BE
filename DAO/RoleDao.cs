using BusinessObject;
using Common.Enum;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class RoleDao : BaseDao<Role>
    {
        private static RoleDao instance = null;
        private static readonly object instacelock = new object();

        private RoleDao()
        {

        }

        public static RoleDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RoleDao();
                }
                return instance;
            }
        }

        public async Task<Role?> GetRoleByRoleName(AccountRoleEnum accountRoleEnum)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Roles
                    .FirstOrDefaultAsync(r => r.RoleName.Equals(accountRoleEnum.ToString()));
            }
        }
    }
}
