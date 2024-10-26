using Microsoft.EntityFrameworkCore;
using MachineTask = BusinessObject.MachineTask;

namespace DAO
{
    public class MachineTaskDao : BaseDao<MachineTask>
    {
        private static MachineTaskDao instance = null;
        private static readonly object instacelock = new object();

        private MachineTaskDao()
        {

        }

        public static MachineTaskDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MachineTaskDao();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<MachineTask>> GetMachineTasks()
        {
            using (var context = new MmrmsContext())
            {
                return await context.MachineTasks.Include(d => d.Staff).Include(d => d.Manager).OrderByDescending(p => p.DateCreate).ToListAsync();
            }
        }

        public async Task<MachineTask> GetMachineTask(int MachineTaskId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.MachineTasks.Include(d => d.Staff).Include(d => d.Manager).FirstOrDefaultAsync(d => d.MachineTaskId == MachineTaskId);
            }
        }

        public async Task<MachineTask> GetMachineTaskDetail(int taskId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.MachineTasks.Include(d => d.Staff).Include(d => d.Manager)
                                                .Include(d => d.MachineTaskLogs)
                                                .ThenInclude(l => l.AccountTrigger)
                                                .Include(d => d.ComponentReplacementTicketsCreateFromTask)
                                                .FirstOrDefaultAsync(d => d.MachineTaskId == taskId);
            }
        }

        //public async Task CreateTaskWithRequest(MachineTask task, RequestResponse requestResponse)
        //{
        //    using (var context = new MmrmsContext())
        //    {

        //    }
        //}
    }
}
