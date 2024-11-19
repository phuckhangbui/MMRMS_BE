using BusinessObject;
using Microsoft.EntityFrameworkCore;
using DeliveryTask = BusinessObject.DeliveryTask;

namespace DAO
{
    public class DeliveryTaskDao : BaseDao<DeliveryTask>
    {
        private static DeliveryTaskDao instance = null;
        private static readonly object instacelock = new object();

        private DeliveryTaskDao()
        {

        }

        public static DeliveryTaskDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DeliveryTaskDao();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<DeliveryTask>> GetDeliveries()
        {
            using (var context = new MmrmsContext())
            {
                return await context.Deliveries
                    .Include(d => d.Staff)
                    .Include(d => d.Manager)
                    .Include(d => d.ContractDeliveries)
                    .ThenInclude(d => d.Contract)
                    .ThenInclude(c => c.RentingRequest)
                    .ThenInclude(c => c.RentingRequestAddress)
                    .OrderByDescending(p => p.DateCreate).ToListAsync();
            }
        }

        public async Task<IEnumerable<DeliveryTask>> GetDeliveriesForStaff(int staffId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Deliveries
                    .Include(d => d.Staff)
                    .Include(d => d.Manager)
                    .Include(d => d.ContractDeliveries)
                    .ThenInclude(d => d.Contract)
                    .ThenInclude(c => c.RentingRequest)
                    .ThenInclude(c => c.RentingRequestAddress)
                    .OrderByDescending(p => p.DateCreate)
                    .Where(d => d.StaffId == staffId)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<DeliveryTask>> GetDeliveriesForCustomer(int customerId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Deliveries
                    .Include(d => d.Staff)
                    .Include(d => d.Manager)
                    .Include(d => d.ContractDeliveries)
                        .ThenInclude(cd => cd.Contract)
                        .ThenInclude(c => c.RentingRequest)
                        .ThenInclude(rr => rr.RentingRequestAddress)
                    .OrderByDescending(d => d.DateCreate)
                    .Where(d => d.ContractDeliveries.Any(cd => cd.Contract.AccountSignId == customerId))
                    .ToListAsync();
            }
        }

        public async Task<DeliveryTask> GetDeliveryTask(int DeliveryTaskId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Deliveries
                    .Include(d => d.Staff)
                    .Include(d => d.Manager)
                    .Include(d => d.ContractDeliveries)
                    .ThenInclude(d => d.Contract)
                    .ThenInclude(c => c.RentingRequest)
                    .ThenInclude(c => c.RentingRequestAddress)
                    .FirstOrDefaultAsync(d => d.DeliveryTaskId == DeliveryTaskId);
            }
        }

        public async Task<DeliveryTask> CreateDelivery(DeliveryTask deliveryTask, List<ContractDelivery> listContractDelivery, DeliveryTaskLog deliveryTaskLog)
        {
            using (var context = new MmrmsContext())
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        context.Deliveries.Add(deliveryTask);
                        await context.SaveChangesAsync();

                        foreach (var contractDelivery in listContractDelivery)
                        {
                            contractDelivery.DeliveryTaskId = deliveryTask.DeliveryTaskId;
                            context.ContractDeliveries.Add(contractDelivery);
                        }
                        await context.SaveChangesAsync();

                        deliveryTaskLog.DeliveryTaskId = deliveryTask.DeliveryTaskId;
                        context.DeliveryTaskLogs.Add(deliveryTaskLog);
                        await context.SaveChangesAsync();


                        await transaction.CommitAsync();

                        return deliveryTask;
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction on error
                        await transaction.RollbackAsync();
                        throw new Exception("Error occurred during transaction: " + ex.Message);
                    }
                }
            }
        }

        public async Task<DeliveryTask> GetDeliveryDetail(int deliveryTaskId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Deliveries
                    .Include(d => d.Staff)
                    .Include(d => d.Manager)
                    .Include(d => d.DeliveryTaskLogs.OrderByDescending(l => l.DateCreate))
                    .ThenInclude(l => l.AccountTrigger)
                    .Include(d => d.ContractDeliveries)
                    .ThenInclude(d => d.Contract)
                    .ThenInclude(c => c.RentingRequest)
                    .ThenInclude(c => c.RentingRequestAddress)
                    .FirstOrDefaultAsync(d => d.DeliveryTaskId == deliveryTaskId);
            }
        }

        public async Task UpdateDeliveryAndContractDelivery(DeliveryTask delivery, DeliveryTaskLog deliveryTaskLog)
        {
            using (var context = new MmrmsContext())
            {
                context.Deliveries.Update(delivery);

                foreach (var contractDelivery in delivery.ContractDeliveries)
                {
                    context.Entry(contractDelivery).State = EntityState.Modified;
                }
                await context.SaveChangesAsync();

                context.DeliveryTaskLogs.Add(deliveryTaskLog);

                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<DeliveryTask>> GetDeliverTaskFromNowForStaff(int staffId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Deliveries
                    .Where(d => d.StaffId == staffId
                    && DateOnly.FromDateTime((DateTime)d.DateShip) >= DateOnly.FromDateTime(DateTime.Now)).ToListAsync();
            }
        }

        public async Task<IEnumerable<DeliveryTask>> GetDeliveryTasksInADate(DateOnly date)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Deliveries
                        .Where(d =>
                                    DateOnly.FromDateTime((DateTime)d.DateShip) == (date)).ToListAsync();
            }
        }

        public async Task<IEnumerable<DeliveryTask>> GetDeliverTaskStaff(int staffId, DateOnly dateStart, DateOnly dateEnd)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Deliveries
                         .Where(d => d.StaffId == staffId
                                    && DateOnly.FromDateTime((DateTime)d.DateShip) >= dateStart
                                    && DateOnly.FromDateTime((DateTime)d.DateShip) <= dateEnd)
                         .ToListAsync();
            }
        }


    }
}
