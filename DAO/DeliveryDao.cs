﻿using BusinessObject;
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
                    .Include(d => d.ContractDeliveries)
                    .ThenInclude(d => d.Contract)
                    .ThenInclude(c => c.RentingRequest)
                    .ThenInclude(c => c.RentingRequestAddress)
                    .OrderByDescending(p => p.DateCreate)
                    .Where(d => d.StaffId == staffId)
                    .ToListAsync();
            }
        }

        public async Task<DeliveryTask> GetDeliveryTask(int DeliveryTaskId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Deliveries
                    .Include(d => d.Staff)
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
                    .Include(d => d.DeliveryTaskLogs)
                    .ThenInclude(l => l.AccountTrigger)
                    .Include(d => d.ContractDeliveries)
                    .ThenInclude(d => d.Contract)
                    .ThenInclude(c => c.RentingRequest)
                    .ThenInclude(c => c.RentingRequestAddress)
                    .FirstOrDefaultAsync(d => d.DeliveryTaskId == deliveryTaskId);
            }
        }

        public async Task UpdateDeliveryAndContractDelivery(DeliveryTask delivery)
        {
            using (var context = new MmrmsContext())
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        context.Deliveries.Update(delivery);

                        foreach (var contractDelivery in delivery.ContractDeliveries)
                        {
                            context.Entry(contractDelivery).State = EntityState.Modified;
                        }
                        await context.SaveChangesAsync();

                        var newDeliveryTaskLog = delivery.DeliveryTaskLogs.Last();
                        newDeliveryTaskLog.DeliveryTaskId = delivery.DeliveryTaskId;
                        context.DeliveryTaskLogs.Add(newDeliveryTaskLog);

                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return;
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



    }
}
