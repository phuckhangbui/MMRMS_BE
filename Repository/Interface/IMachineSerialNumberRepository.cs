﻿using DTOs.Machine;
using DTOs.MachineSerialNumber;
using DTOs.RentingRequest;

namespace Repository.Interface
{
    public interface IMachineSerialNumberRepository
    {
        Task CreateMachineSerialNumber(MachineSerialNumberDto serialMachineNumberDto, IEnumerable<MachineComponentDto> componentMachineList, int accountId);
        Task Delete(string serialNumber);
        Task<bool> IsSerialNumberExist(string serialNumber);
        Task<bool> IsMachineSerialNumberHasContract(string serialNumber);
        Task UpdateRentDaysCounterMachineSerialNumber(MachineSerialNumberDto machineSerialNumberDto, int accountId);
        Task UpdateMachineSerialNumber(string serialNumber, MachineSerialNumberUpdateDto machineSerialNumberUpdateDto, int accountId);
        Task<bool> CheckMachineSerialNumberValidToRent(List<RentingRequestSerialNumberDto> rentingRequestSerialNumbers);
        Task<MachineSerialNumberDto> GetMachineSerialNumber(string serialNumber);
        Task<IEnumerable<MachineSerialNumberLogDto>> GetMachineSerialNumberLog(string serialNumber);
        Task<IEnumerable<MachineSerialNumberComponentDto>> GetMachineComponent(string serialNumber);
        Task UpdateStatus(string serialNumber, string status, int staffId);
        Task UpdateStatus(string serialNumber, string status, int staffId, string note);
        Task UpdateStatus(string serialNumber, string status, int accountId, string? note, DateTime? expectedAvailableDate);
    }
}
