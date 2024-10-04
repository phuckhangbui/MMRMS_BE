﻿using DTOs.MaintenanceTicket;

namespace Repository.Interface
{
    public interface IMaintenanceTicketRepository
    {
        Task CreateTicket(int staffId, CreateMaintenanceTicketDto createMaintenanceTicketDto);
        Task<IEnumerable<MaintenanceTicketDto>> GetTickets();
        Task<IEnumerable<MaintenanceTicketDto>> GetTicketsByCustomerId(int customerId);
    }
}
