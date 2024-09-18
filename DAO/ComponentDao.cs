using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DAO
{
    public class ComponentDao : BaseDao<Component>
    {
        private static ComponentDao instance = null;
        private static readonly object instacelock = new object();

        private ComponentDao()
        {

        }

        public static ComponentDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ComponentDao();
                }
                return instance;
            }
        }

        public async Task<bool> IsComponentIdExisted(int id)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Components.AnyAsync(c => c.ComponentId == id);
            }
        }

        public async Task<bool> IsComponentNameExisted(string name)
        {
            if (name.IsNullOrEmpty())
            {
                return false;
            }

            using (var context = new MmrmsContext())
            {
                return await context.Components.AnyAsync(c => c.ComponentName.ToLower().Equals(name.ToLower()));
            }
        }

        public async Task<Component> CreateComponent(Component component)
        {
            using (var context = new MmrmsContext())
            {
                DbSet<Component> _dbSet = context.Set<Component>();
                _dbSet.Add(component);
                await context.SaveChangesAsync();

                return component;
            }
        }
    }
}
