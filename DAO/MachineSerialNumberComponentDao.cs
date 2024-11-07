using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class MachineSerialNumberComponentDao : BaseDao<MachineSerialNumberComponent>
    {
        private static MachineSerialNumberComponentDao instance = null;
        private static readonly object instacelock = new object();

        private MachineSerialNumberComponentDao()
        {

        }

        public static MachineSerialNumberComponentDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MachineSerialNumberComponentDao();
                }
                return instance;
            }
        }

        public async Task<MachineSerialNumberComponent> GetComponent(int machineSerialNumberComponentId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.MachineSerialNumberComponents
                    .Include(sc => sc.MachineComponent)
                    .ThenInclude(c => c.Component)
                    .FirstOrDefaultAsync(sc => sc.MachineSerialNumberComponentId == machineSerialNumberComponentId);

            }
        }
    }
}
