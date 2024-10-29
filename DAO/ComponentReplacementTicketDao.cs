﻿using BusinessObject;
using Microsoft.EntityFrameworkCore;
using ComponentReplacementTicket = BusinessObject.ComponentReplacementTicket;

namespace DAO
{
    public class ComponentReplacementTicketDao : BaseDao<ComponentReplacementTicket>
    {
        private static ComponentReplacementTicketDao instance = null;
        private static readonly object instacelock = new object();

        private ComponentReplacementTicketDao()
        {

        }

        public static ComponentReplacementTicketDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ComponentReplacementTicketDao();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<ComponentReplacementTicket>> GetComponentReplacementTickets()
        {
            using (var context = new MmrmsContext())
            {
                return await context.ComponentReplacementTickets
                    .Include(c => c.EmployeeCreate).Include(c => c.Component).Include(c => c.Contract)
                    .ToListAsync();
            }
        }

        public async Task CreateTicket(ComponentReplacementTicket componentTicket, ComponentReplacementTicketLog ticketLog, Invoice invoice)
        {
            using (var context = new MmrmsContext())
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        context.ComponentReplacementTickets.Add(componentTicket);
                        await context.SaveChangesAsync();

                        invoice.ComponentReplacementTicketId = componentTicket.ComponentReplacementTicketId;
                        context.Invoices.Add(invoice);
                        await context.SaveChangesAsync();

                        componentTicket.InvoiceId = invoice.InvoiceId;
                        context.ComponentReplacementTickets.Update(componentTicket);
                        await context.SaveChangesAsync();


                        ticketLog.ComponentReplacementTicketId = componentTicket.ComponentReplacementTicketId;
                        context.ComponentReplacementTicketLogs.Add(ticketLog);
                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();
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
