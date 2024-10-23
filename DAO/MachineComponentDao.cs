using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class MachineComponentDao : BaseDao<MachineComponent>
    {
        private static MachineComponentDao instance = null;
        private static readonly object instacelock = new object();

        private MachineComponentDao()
        {

        }

        public static MachineComponentDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MachineComponentDao();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<MachineComponent>> GetMachineComponentBaseOnComponentId(int componentId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.MachineComponents.Where(c => c.ComponentId == componentId).ToListAsync();
            }
        }
    }
}
