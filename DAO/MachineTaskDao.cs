using BusinessObject;
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
                return await context.MachineTasks.Include(d => d.Staff)
                                                 .Include(d => d.Manager)
                                                 .OrderByDescending(p => p.DateCreate).ToListAsync();
            }
        }

        public async Task<MachineTask> GetMachineTask(int machineTaskId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.MachineTasks.Include(d => d.Staff)
                                                 .Include(d => d.Manager)
                                                 .FirstOrDefaultAsync(d => d.MachineTaskId == machineTaskId);
            }
        }

        public async Task<MachineTask> GetMachineTaskDetail(int taskId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.MachineTasks.Include(d => d.Staff)
                                                 .Include(d => d.Manager)
                                                 .Include(d => d.MachineTaskLogs.OrderByDescending(l => l.DateCreate))
                                                 .ThenInclude(l => l.AccountTrigger)
                                                 .Include(d => d.ComponentReplacementTicketsCreateFromTask)
                                                 .Include(d => d.MachineCheckRequest)
                                                 .Include(d => d.Contract)
                                                 .FirstOrDefaultAsync(d => d.MachineTaskId == taskId);
            }
        }

        public async Task<MachineTask> CreateMachineTaskBaseOnRequest(MachineTask task, MachineTaskLog taskLog)
        {
            using (var context = new MmrmsContext())
            {

                context.MachineTasks.Add(task);
                await context.SaveChangesAsync();

                taskLog.MachineTaskId = task.MachineTaskId;
                context.MachineTaskLogs.Add(taskLog);
                await context.SaveChangesAsync();

                return task;

            }
        }

        public async Task<IEnumerable<MachineTask>> GetMachineTaskFromNowForStaff(int staffId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.MachineTasks
                    .Where(d => d.StaffId == staffId
                    && DateOnly.FromDateTime((DateTime)d.DateStart) >= DateOnly.FromDateTime(DateTime.Now)).ToListAsync();
            }
        }

        public async Task<IEnumerable<MachineTask>> GetMachineTasksInADate(DateOnly date)
        {
            using (var context = new MmrmsContext())
            {
                return await context.MachineTasks
                        .Where(d =>
                                    DateOnly.FromDateTime((DateTime)d.DateStart) == (date)).ToListAsync();
            }
        }
    }

}
