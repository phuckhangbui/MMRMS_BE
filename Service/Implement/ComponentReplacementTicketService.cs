using Common;
using Common.Enum;
using DTOs.ComponentReplacementTicket;
using Microsoft.AspNetCore.SignalR;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using Service.SignalRHub;
using System.Transactions;

namespace Service.Implement
{
    public class ComponentReplacementTicketService : IComponentReplacementTicketService
    {
        private readonly IComponentReplacementTicketRepository _componentReplacementTicketRepository;
        private readonly IComponentRepository _componentRepository;
        private readonly IMachineSerialNumberLogRepository _machineSerialNumberLogRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IMachineTaskRepository _machineTaskRepository;
        private readonly IMachineSerialNumberComponentRepository _machineSerialNumberComponentRepository;
        private readonly IMachineCheckRequestService _machineCheckRequestService;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IHubContext<ComponentReplacementTicketHub> _ComponentReplacementTicketHub;
        private readonly INotificationService _notificationService;


        public ComponentReplacementTicketService(IComponentReplacementTicketRepository ComponentReplacementTicketRepository, IComponentRepository componentRepository, IContractRepository contractRepository, IHubContext<ComponentReplacementTicketHub> ComponentReplacementTicketHub, INotificationService notificationService, IMachineTaskRepository machineTaskRepository, IMachineSerialNumberComponentRepository machineSerialNumberComponentRepository, IMachineCheckRequestService machineCheckRequestService, IMachineSerialNumberLogRepository machineSerialNumberLogRepository)
        {
            _componentReplacementTicketRepository = ComponentReplacementTicketRepository;
            _componentRepository = componentRepository;
            _contractRepository = contractRepository;
            _ComponentReplacementTicketHub = ComponentReplacementTicketHub;
            _notificationService = notificationService;
            _machineTaskRepository = machineTaskRepository;
            _machineSerialNumberComponentRepository = machineSerialNumberComponentRepository;
            _machineCheckRequestService = machineCheckRequestService;
            _machineSerialNumberLogRepository = machineSerialNumberLogRepository;
        }

        //private async Task UpdateMachineTaskAndMachineCheckRequestBaseOnNewTicketStatus(int machineTaskId, int activatorId)
        //{
        //    var machineTaskDetail = await _machineTaskRepository.GetMachineTaskDetail(machineTaskId);

        //    if (machineTaskDetail == null)
        //    {
        //        throw new ServiceException(MessageConstant.MachineTask.TaskNotFound);
        //    }

        //    if (machineTaskDetail.ComponentReplacementTicketCreateFromTaskList.Count() == 1)
        //    {
        //        await this.UpdateTaskAndRequestStatusToCompleted(machineTaskDetail.MachineTaskId, machineTaskDetail.MachineCheckRequestId, activatorId);
        //    }
        //    else
        //    {
        //        var isAllTicketCompleted = machineTaskDetail.ComponentReplacementTicketCreateFromTaskList.All(componentReplacementTicket =>
        //                                                    componentReplacementTicket.Status == ComponentReplacementTicketStatusEnum.Completed.ToString() ||
        //                                                    componentReplacementTicket.Status == ComponentReplacementTicketStatusEnum.Canceled.ToString());

        //        if (isAllTicketCompleted)
        //        {
        //            await this.UpdateTaskAndRequestStatusToCompleted(machineTaskDetail.MachineTaskId, machineTaskDetail.MachineCheckRequestId, activatorId);
        //        }
        //    }
        //}

        //private async Task UpdateTaskAndRequestStatusToCompleted(int machineTaskId, string machineCheckRequestId, int activatorId)
        //{
        //    await _machineTaskRepository.UpdateTaskStatus(machineTaskId,
        //                                                  MachineTaskEnum.Completed.ToString(),
        //                                                  activatorId,
        //                                                  null);

        //    await _machineCheckRequestService.UpdateRequestStatus(machineCheckRequestId,
        //                                                          MachineCheckRequestStatusEnum.Completed.ToString(),
        //                                                          null);
        //}

        public async Task CancelComponentReplacementTicket(int customerId, string componentReplacementTicketId)
        {
            var ticket = await _componentReplacementTicketRepository.GetTicket(componentReplacementTicketId);

            if (ticket == null)
            {
                throw new ServiceException(MessageConstant.ComponentReplacementTicket.TicketNotFound);
            }

            if (ticket.Status != ComponentReplacementTicketStatusEnum.Unpaid.ToString())
            {
                throw new ServiceException(MessageConstant.ComponentReplacementTicket.CannotCancelAlreadyPaid);
            }

            var contract = await _contractRepository.GetContractById(ticket.ContractId);

            if (contract?.AccountSignId != customerId)
            {
                throw new ServiceException(MessageConstant.ComponentReplacementTicket.NotCorrectCustomerId);
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _componentReplacementTicketRepository.UpdateTicketStatus(componentReplacementTicketId, ComponentReplacementTicketStatusEnum.Canceled.ToString(), customerId);

                    await _componentRepository.MoveComponentQuanityFromOnHoldToAvailable((int)ticket.ComponentId, (int)ticket.Quantity);

                    //update invoice status
                    await _invoiceRepository.UpdateInvoiceStatus(ticket.InvoiceId, InvoiceStatusEnum.Canceled.ToString());

                    //update task status and request status
                    //await this.UpdateMachineTaskAndMachineCheckRequestBaseOnNewTicketStatus((int)ticket.MachineTaskCreateId, customerId);

                    string action = $"Ticket thay thế bộ phận [{ticket.ComponentName}] đã bị hủy";

                    await _machineSerialNumberLogRepository.WriteComponentLog(ticket.SerialNumber, (int)ticket.MachineSerialNumberComponentId, action, customerId);

                    //notify staff
                    await _notificationService.SendNotificationToStaffWhenCustomerCancelTicket(ticket, ticket.ComponentReplacementTicketId);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw new ServiceException(MessageConstant.ComponentReplacementTicket.ErrorWhenCancel);
                }
            }

            await _ComponentReplacementTicketHub.Clients.All.SendAsync("OnUpdateComponenentReplacementTicket", componentReplacementTicketId);

        }

        public async Task CompleteComponentReplacementTicket(int staffId, string componentReplacementTicketId)
        {
            var ticket = await _componentReplacementTicketRepository.GetTicket(componentReplacementTicketId);

            if (ticket == null)
            {
                throw new ServiceException(MessageConstant.ComponentReplacementTicket.TicketNotFound);
            }

            if (ticket.Status != ComponentReplacementTicketStatusEnum.Paid.ToString())
            {
                throw new ServiceException(MessageConstant.ComponentReplacementTicket.NotReadyToBeCompletedWhenNotPaid);
            }

            if (staffId != ticket.EmployeeCreateId)
            {
                throw new ServiceException(MessageConstant.ComponentReplacementTicket.NotCorrectStaffId);
            }

            var component = await _componentRepository.GetComponent((int)ticket.ComponentId);
            if (component == null)
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.ComponentIdNotFound);
            }


            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var serialComponent = await _machineSerialNumberComponentRepository.GetComponent((int)ticket.MachineSerialNumberComponentId);

                    if (serialComponent == null)
                    {
                        throw new ServiceException(MessageConstant.MachineSerialNumber.ComponentIdNotFound);
                    }

                    string action = $"Ticket thay thế bộ phận [{ticket.ComponentName}] đã được hoàn tất, bộ phận đã được thay thế";

                    await _machineSerialNumberLogRepository.WriteComponentLog(ticket.SerialNumber, (int)ticket.MachineSerialNumberComponentId, action, staffId);

                    await _machineSerialNumberComponentRepository.UpdateComponentStatus(serialComponent.MachineSerialNumberComponentId, MachineSerialNumberComponentStatusEnum.Normal.ToString(), staffId);

                    await _componentReplacementTicketRepository.UpdateTicketStatusToComplete(componentReplacementTicketId, staffId);

                    await _componentRepository.RemoveOnHoldQuantity((int)ticket.ComponentId, (int)ticket.Quantity);

                    //await this.UpdateMachineTaskAndMachineCheckRequestBaseOnNewTicketStatus((int)ticket.MachineTaskCreateId, staffId);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw new ServiceException(MessageConstant.ComponentReplacementTicket.CompleteFail);
                }
            }

            await _ComponentReplacementTicketHub.Clients.All.SendAsync("OnUpdateComponenentReplacementTicket", componentReplacementTicketId);
        }


        //private async Task CreateComponentReplacementTicketInternal(
        //            int staffId,
        //            CreateComponentReplacementTicketBaseDto createComponentReplacementTicketDto,
        //            int? machineTaskId = null,
        //            string? contractId = null)
        //{
        //    MachineTaskDto machineTask = null;

        //    if (machineTask.Type == MachineTaskTypeEnum.MachineryCheckRequest.ToString() )
        //    {
        //        machineTask = await _machineTaskRepository.GetMachineTask(machineTaskId.Value);
        //        if (machineTask == null)
        //        {
        //            throw new ServiceException(MessageConstant.MachineTask.TaskNotFound);
        //        }

        //        if (machineTask.Status != MachineTaskStatusEnum.Created.ToString() &&
        //            machineTask.Status != MachineTaskStatusEnum.Reparing.ToString())
        //        {
        //            throw new ServiceException(MessageConstant.MachineTask.CannotCreateTicketWithThisTask);
        //        }

        //        contractId = machineTask.ContractId;
        //    }

        //    var serialComponent = await _machineSerialNumberComponentRepository
        //                                 .GetComponent(createComponentReplacementTicketDto.MachineSerialNumberComponentId);
        //    if (serialComponent == null)
        //    {
        //        throw new ServiceException(MessageConstant.MachineSerialNumber.ComponentIdNotFound);
        //    }

        //    if (serialComponent.Quantity < createComponentReplacementTicketDto.Quantity)
        //    {
        //        throw new ServiceException(MessageConstant.ComponentReplacementTicket.BiggerQuantityThanMachine);
        //    }

        //    var component = await _componentRepository.GetComponent((int)serialComponent.ComponentId);
        //    if (component == null ||
        //        component.Status != ComponentStatusEnum.Active.ToString() ||
        //        component.AvailableQuantity < createComponentReplacementTicketDto.Quantity)
        //    {
        //        throw new ServiceException(MessageConstant.ComponentReplacementTicket.NotEnoughQuantity);
        //    }

        //    if (machineTask.Type == MachineTaskTypeEnum.MachineryCheckRequest.ToString() )
        //    {
        //        var machineTaskDetail = await _machineTaskRepository.GetMachineTaskDetail(machineTaskId.Value);
        //        if (machineTaskDetail.ComponentReplacementTicketCreateFromTaskList.Any(ticket =>
        //            ticket.ComponentId == component.ComponentId))
        //        {
        //            throw new ServiceException(MessageConstant.ComponentReplacementTicket.DuplicateComponentTicketForThisTask);
        //        }
        //    }
        //    else
        //    {
        //        var ticketListFromContract = await _componentReplacementTicketRepository.GetTicketListFromContract(contractId);
        //        if (ticketListFromContract.Any(ticket =>
        //           ticket.ComponentId == component.ComponentId && ticket.Type == ComponentReplacementTicketTypeEnum.ContractTermniationTicket.ToString()))
        //        {
        //            throw new ServiceException(MessageConstant.ComponentReplacementTicket.DuplicateComponentTicketForThisContractWhenTerminate);
        //        }
        //    }

        //    var now = DateTime.Now;
        //    var replacementTicket = new ComponentReplacementTicketDto
        //    {
        //        EmployeeCreateId = staffId,
        //        MachineTaskCreateId = machineTaskId,
        //        ContractId = contractId,
        //        ComponentId = serialComponent.ComponentId,
        //        MachineSerialNumberComponentId = createComponentReplacementTicketDto.MachineSerialNumberComponentId,
        //        ComponentPrice = createComponentReplacementTicketDto.ComponentPrice,
        //        AdditionalFee = createComponentReplacementTicketDto.AdditionalFee,
        //        Quantity = createComponentReplacementTicketDto.Quantity,
        //        TotalAmount = createComponentReplacementTicketDto.ComponentPrice * createComponentReplacementTicketDto.Quantity
        //                      + createComponentReplacementTicketDto.AdditionalFee,
        //        DateCreate = now,
        //        Note = createComponentReplacementTicketDto.Note
        //    };

        //    if (machineTask.Type == MachineTaskTypeEnum.MachineryCheckRequest.ToString() )
        //    {
        //        replacementTicket.Type = ComponentReplacementTicketTypeEnum.RentingTicket.ToString();
        //        replacementTicket.Status = ComponentReplacementTicketStatusEnum.Unpaid.ToString();
        //    }
        //    else
        //    {
        //        replacementTicket.Type = ComponentReplacementTicketTypeEnum.ContractTermniationTicket.ToString();
        //        replacementTicket.Status = ComponentReplacementTicketStatusEnum.Paid.ToString();
        //    }

        //    var contract = await _contractRepository.GetContractById(contractId);
        //    if (contract == null)
        //    {
        //        throw new ServiceException(MessageConstant.Contract.ContractNotFound);
        //    }

        //    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //    {
        //        try
        //        {
        //            if (serialComponent.Status != MachineSerialNumberComponentStatusEnum.Broken.ToString())
        //            {
        //                await _machineSerialNumberComponentRepository.UpdateComponentStatus(
        //                    serialComponent.MachineSerialNumberComponentId,
        //                    MachineSerialNumberComponentStatusEnum.Broken.ToString(),
        //                    staffId);
        //            }

        //            var newComponentTicket = await _componentReplacementTicketRepository
        //                                              .CreateTicket(staffId, replacementTicket, contract.AccountSignId);

        //            if (machineTask.Type == MachineTaskTypeEnum.MachineryCheckRequest.ToString() )
        //            {
        //                await _machineTaskRepository.UpdateTaskStatus(machineTask.MachineTaskId, MachineTaskStatusEnum.Reparing.ToString(), staffId, null);
        //                await _machineCheckRequestService.UpdateRequestStatus(
        //                   machineTask?.MachineCheckRequestId,
        //                   MachineCheckRequestStatusEnum.Processing.ToString(),
        //                   null);

        //                await _componentRepository.MoveComponentQuanityFromAvailableToOnHold(
        //                    component.ComponentId, createComponentReplacementTicketDto.Quantity);
        //            }

        //            string componentLogMessage = $"Tạo ticket thay thế bộ phận [{serialComponent.ComponentName}]";
        //            if (machineTask.Type == MachineTaskTypeEnum.MachineryCheckRequest.ToString() )
        //            {
        //                componentLogMessage += " khi đang thuê máy";
        //            }
        //            else
        //            {
        //                componentLogMessage += " khi đáo hạn hợp đồng";
        //            }

        //            await _machineSerialNumberLogRepository.WriteComponentLog(
        //                serialComponent.SerialNumber,
        //                createComponentReplacementTicketDto.MachineSerialNumberComponentId,
        //               componentLogMessage,
        //                staffId);

        //            scope.Complete();
        //        }
        //        catch
        //        {
        //            throw new ServiceException(MessageConstant.ComponentReplacementTicket.CreateFail);
        //        }
        //    }

        //    if (contract != null)
        //    {
        //        await _notificationService.SendNotificationToCustomerWhenCreateComponentReplacementTicket(
        //            (int)contract.AccountSignId, (double)replacementTicket.TotalAmount, replacementTicket.ComponentName);
        //    }
        //    await _ComponentReplacementTicketHub.Clients.All.SendAsync("OnCreateComponentReplacementTicket");
        //}
        public async Task CreateComponentReplacementTicket(int staffId, CreateComponentReplacementTicketDto createComponentReplacementTicketDto)
        {
            var machineTaskDetail = await _machineTaskRepository.GetMachineTaskDetail(createComponentReplacementTicketDto.MachineTaskCreateId);

            if (machineTaskDetail == null)
            {
                throw new ServiceException(MessageConstant.MachineTask.TaskNotFound);
            }

            if (machineTaskDetail.Status != MachineTaskEnum.Created.ToString() &&
                machineTaskDetail.Status != MachineTaskEnum.Reparing.ToString())
            {
                throw new ServiceException(MessageConstant.MachineTask.CannotCreateTicketWithThisTask);
            }

            var contractId = machineTaskDetail.ContractId;

            var serialComponent = await _machineSerialNumberComponentRepository
                                         .GetComponent(createComponentReplacementTicketDto.MachineSerialNumberComponentId);
            if (serialComponent == null)
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.ComponentIdNotFound);
            }

            if (serialComponent.AvailableQuantity < createComponentReplacementTicketDto.Quantity)
            {
                throw new ServiceException(MessageConstant.ComponentReplacementTicket.BiggerQuantityThanMachine);
            }

            var component = await _componentRepository.GetComponent((int)serialComponent.ComponentId);
            if (machineTaskDetail.Type == MachineTaskTypeEnum.MachineryCheckRequest.ToString())
            {
                if (component == null ||
                component.Status != ComponentStatusEnum.Active.ToString() ||
                component.AvailableQuantity < createComponentReplacementTicketDto.Quantity)
                {
                    throw new ServiceException(MessageConstant.ComponentReplacementTicket.NotEnoughQuantity);
                }
            }


            if (machineTaskDetail.ComponentReplacementTicketCreateFromTaskList.Any(ticket =>
                ticket.ComponentId == component.ComponentId))
            {
                throw new ServiceException(MessageConstant.ComponentReplacementTicket.DuplicateComponentTicketForThisTask);
            }


            var now = DateTime.Now;
            var replacementTicket = new ComponentReplacementTicketDto
            {
                EmployeeCreateId = staffId,
                MachineTaskCreateId = createComponentReplacementTicketDto.MachineTaskCreateId,
                ContractId = contractId,
                ComponentId = serialComponent.ComponentId,
                MachineSerialNumberComponentId = createComponentReplacementTicketDto.MachineSerialNumberComponentId,
                ComponentPrice = createComponentReplacementTicketDto.ComponentPrice,
                AdditionalFee = createComponentReplacementTicketDto.AdditionalFee,
                Quantity = createComponentReplacementTicketDto.Quantity,
                TotalAmount = createComponentReplacementTicketDto.ComponentPrice * createComponentReplacementTicketDto.Quantity
                              + createComponentReplacementTicketDto.AdditionalFee,
                DateCreate = now,
                Note = createComponentReplacementTicketDto.Note
            };

            if (machineTaskDetail.Type == MachineTaskTypeEnum.MachineryCheckRequest.ToString())
            {
                replacementTicket.Type = ComponentReplacementTicketTypeEnum.RentingTicket.ToString();
                replacementTicket.Status = ComponentReplacementTicketStatusEnum.Unpaid.ToString();
            }
            else if (machineTaskDetail.Type == MachineTaskTypeEnum.ContractTerminationCheck.ToString())
            {
                replacementTicket.Type = ComponentReplacementTicketTypeEnum.ContractTerminationTicket.ToString();
                replacementTicket.Status = ComponentReplacementTicketStatusEnum.Completed.ToString();
            }

            var contract = await _contractRepository.GetContractById(contractId);
            if (contract == null)
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotFound);
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (serialComponent.Status != MachineSerialNumberComponentStatusEnum.Broken.ToString())
                    {
                        await _machineSerialNumberComponentRepository.UpdateComponentStatus(
                            serialComponent.MachineSerialNumberComponentId,
                            MachineSerialNumberComponentStatusEnum.Broken.ToString(),
                            staffId);
                    }

                    var newComponentTicket = await _componentReplacementTicketRepository
                                                      .CreateTicket(staffId, replacementTicket, contract.AccountSignId);

                    if (machineTaskDetail.Type == MachineTaskTypeEnum.MachineryCheckRequest.ToString())
                    {
                        await _machineTaskRepository.UpdateTaskStatus(machineTaskDetail.MachineTaskId, MachineTaskEnum.Reparing.ToString(), staffId, null);
                        await _machineCheckRequestService.UpdateRequestStatus(
                           machineTaskDetail?.MachineCheckRequestId,
                           MachineCheckRequestStatusEnum.Processing.ToString(),
                           null);

                        await _componentRepository.MoveComponentQuanityFromAvailableToOnHold(
                            component.ComponentId, createComponentReplacementTicketDto.Quantity);
                    }

                    string componentLogMessage = $"Tạo ticket thay thế bộ phận [{serialComponent.ComponentName}]";
                    if (machineTaskDetail.Type == MachineTaskTypeEnum.MachineryCheckRequest.ToString())
                    {
                        componentLogMessage += " khi đang thuê máy";
                    }
                    else if (machineTaskDetail.Type == MachineTaskTypeEnum.ContractTerminationCheck.ToString())
                    {
                        componentLogMessage += " khi đáo hạn hợp đồng";
                    }

                    await _machineSerialNumberLogRepository.WriteComponentLog(
                        serialComponent.SerialNumber,
                        createComponentReplacementTicketDto.MachineSerialNumberComponentId,
                       componentLogMessage,
                        staffId);
                    if (contract != null)
                    {
                        await _notificationService.SendNotificationToCustomerWhenCreateComponentReplacementTicket(
                            (int)contract.AccountSignId, (double)replacementTicket.TotalAmount, replacementTicket.ComponentName, newComponentTicket.ComponentReplacementTicketId);
                    }
                    scope.Complete();
                }
                catch
                {
                    throw new ServiceException(MessageConstant.ComponentReplacementTicket.CreateFail);
                }
            }


            await _ComponentReplacementTicketHub.Clients.All.SendAsync("OnCreateComponentReplacementTicket");
        }



        public async Task<ComponentReplacementTicketDetailDto> GetComponentReplacementTicket(string replacementTicketId)
        {
            return await _componentReplacementTicketRepository.GetComponentReplacementTicketDetail(replacementTicketId);
        }

        public async Task<IEnumerable<ComponentReplacementTicketDto>> GetComponentReplacementTickets()
        {
            return await _componentReplacementTicketRepository.GetTickets();
        }

        public async Task<IEnumerable<ComponentReplacementTicketDto>> GetComponentReplacementTickets(int customerId)
        {
            return await _componentReplacementTicketRepository.GetTicketsByCustomerId(customerId);
        }

        public async Task<IEnumerable<ComponentReplacementTicketDto>> GetComponentReplacementTicketsForStaff(int staffId)
        {
            var list = await _componentReplacementTicketRepository.GetTickets();

            var result = list.Where(t => t.EmployeeCreateId == staffId).ToList();

            return result;
        }


    }
}
