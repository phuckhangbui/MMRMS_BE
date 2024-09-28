using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class ComponentProductDao : BaseDao<ComponentProduct>
    {
        private static ComponentProductDao instance = null;
        private static readonly object instacelock = new object();

        private ComponentProductDao()
        {

        }

        public static ComponentProductDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ComponentProductDao();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<ComponentProduct>> GetComponentProductBaseOnComponentId(int componentId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.ComponentProducts.Where(c => c.ComponentId == componentId).ToListAsync();
            }
        }
    }
}
