﻿using DTOs.MaintenanceTicket;

namespace Repository.Interface
{
    public interface IMaintenanceTicketRepository
    {
        Task<MaintenanceTicketDto> CreateTicket(int staffId, CreateMaintenanceTicketDto createMaintenanceTicketDto);
        Task<MaintenanceTicketDto> GetTicket(string maintenanceTicketId);
        Task<IEnumerable<MaintenanceTicketDto>> GetTickets();
        Task<IEnumerable<MaintenanceTicketDto>> GetTicketsByCustomerId(int customerId);
    }
}
