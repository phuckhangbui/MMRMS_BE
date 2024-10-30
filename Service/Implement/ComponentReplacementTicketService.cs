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
        private readonly IMachineSerialNumberRepository _machineSerialNumberRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IMachineTaskRepository _machineTaskRepository;
        private readonly IMachineSerialNumberComponentRepository _machineSerialNumberComponentRepository;
        private readonly IMachineCheckRequestService _machineCheckRequestService;
        private readonly IHubContext<ComponentReplacementTicketHub> _ComponentReplacementTicketHub;
        private readonly INotificationService _notificationService;


        public ComponentReplacementTicketService(IComponentReplacementTicketRepository ComponentReplacementTicketRepository, IMachineSerialNumberRepository machineSerialNumberRepository, IComponentRepository componentRepository, IContractRepository contractRepository, IHubContext<ComponentReplacementTicketHub> ComponentReplacementTicketHub, INotificationService notificationService, IMachineTaskRepository machineTaskRepository, IMachineSerialNumberComponentRepository machineSerialNumberComponentRepository, IMachineCheckRequestService machineCheckRequestService)
        {
            _componentReplacementTicketRepository = ComponentReplacementTicketRepository;
            _machineSerialNumberRepository = machineSerialNumberRepository;
            _componentRepository = componentRepository;
            _contractRepository = contractRepository;
            _ComponentReplacementTicketHub = ComponentReplacementTicketHub;
            _notificationService = notificationService;
            _machineTaskRepository = machineTaskRepository;
            _machineSerialNumberComponentRepository = machineSerialNumberComponentRepository;
            _machineCheckRequestService = machineCheckRequestService;
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

            var machineTaskDetail = await _machineTaskRepository.GetMachineTaskDetail((int)ticket.MachineTaskCreateId);

            if (machineTaskDetail == null)
            {
                throw new ServiceException(MessageConstant.MachineTask.TaskNotFound);
            }



            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _componentReplacementTicketRepository.UpdateTicketStatusToComplete(componentReplacementTicketId, staffId);

                    await _componentRepository.RemoveOnHoldQuantity((int)ticket.ComponentId, (int)ticket.Quantity);

                    if (machineTaskDetail.ComponentReplacementTicketCreateFromTaskList.Count() > 1)
                    {
                        bool isTaskComplete = true;
                        foreach (var taskTicket in machineTaskDetail.ComponentReplacementTicketCreateFromTaskList)
                        {
                            if (taskTicket.Status != MachineTaskStatusEnum.Completed.ToString())
                            {
                                isTaskComplete = false;
                                break;
                            }
                        }

                        if (isTaskComplete)
                        {
                            await _machineTaskRepository.UpdateTaskStatus(machineTaskDetail.MachineTaskId,
                                                                      MachineTaskStatusEnum.Completed.ToString(),
                                                                      staffId,
                                                                      null);

                            await _machineCheckRequestService.UpdateRequestStatus(machineTaskDetail.MachineCheckRequestId, MachineCheckRequestStatusEnum.Completed.ToString());
                        }
                    }
                    else
                    {
                        await _machineTaskRepository.UpdateTaskStatus(machineTaskDetail.MachineTaskId,
                                                                      MachineTaskStatusEnum.Completed.ToString(),
                                                                      staffId,
                                                                      null);

                        await _machineCheckRequestService.UpdateRequestStatus(machineTaskDetail.MachineCheckRequestId, MachineCheckRequestStatusEnum.Completed.ToString());
                    }



                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw new ServiceException(MessageConstant.ComponentReplacementTicket.CompleteFail);
                }
            }

        }

        public async Task CreateComponentReplacementTicketWhenCheckMachineRenting(int staffId, CreateComponentReplacementTicketDto createComponentReplacementTicketDto)
        {
            var machineTask = await _machineTaskRepository.GetMachineTask(createComponentReplacementTicketDto.MachineTaskCreateId);

            if (machineTask == null)
            {
                throw new ServiceException(MessageConstant.MachineTask.TaskNotFound);
            }

            if (machineTask.Status != MachineTaskStatusEnum.Created.ToString() || machineTask.Status != MachineTaskStatusEnum.Reparing.ToString())
            {
                throw new ServiceException(MessageConstant.MachineTask.CannotCreateTicketWithThisTask);
            }

            var serialComponent = await _machineSerialNumberComponentRepository.GetComponent(createComponentReplacementTicketDto.MachineSerialNumberComponentId);

            if (serialComponent == null)
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.ComponentIdNotFound);
            }

            //if (serialComponent.Status != MachineSerialNumberComponentStatusEnum.Broken.ToString())
            //{
            //    throw new ServiceException(MessageConstant.MachineSerialNumber.ComponentIsNotBrokenToCreateTicket);
            //}

            if (serialComponent.Quantity < createComponentReplacementTicketDto.Quantity)
            {
                throw new ServiceException(MessageConstant.ComponentReplacementTicket.BiggerQuantityThanMachine);
            }

            var component = await _componentRepository.GetComponent((int)serialComponent.ComponentId);
            if (component == null)
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.ComponentIdNotFound);
            }

            if (component.Status != ComponentStatusEnum.Active.ToString() || (component?.AvailableQuantity) < createComponentReplacementTicketDto.Quantity)
            {
                throw new ServiceException(MessageConstant.ComponentReplacementTicket.NotEnoughQuantity);
            }

            var now = DateTime.Now;

            var replacementTicket = new ComponentReplacementTicketDto
            {
                ComponentReplacementTicketId = GlobalConstant.ComponentReplacementTicketIdPrefixPattern + now.ToString(GlobalConstant.DateTimeFormatPattern),
                EmployeeCreateId = staffId,
                MachineTaskCreateId = createComponentReplacementTicketDto.MachineTaskCreateId,
                ContractId = machineTask.ContractId,
                ComponentId = serialComponent.ComponentId,
                MachineSerialNumberComponentId = createComponentReplacementTicketDto.MachineSerialNumberComponentId,
                ComponentPrice = createComponentReplacementTicketDto.ComponentPrice,
                AdditionalFee = createComponentReplacementTicketDto.AdditionalFee,
                Quantity = createComponentReplacementTicketDto.Quantity,
                TotalAmount = createComponentReplacementTicketDto.ComponentPrice * createComponentReplacementTicketDto.Quantity + createComponentReplacementTicketDto.AdditionalFee,
                DateCreate = now,
                Status = ComponentReplacementTicketStatusEnum.Unpaid.ToString(),
                Note = createComponentReplacementTicketDto.Note,
            };

            var contract = await _contractRepository.GetContractById(machineTask.ContractId);

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
                        await _machineSerialNumberComponentRepository.UpdateComponentStatus(serialComponent.MachineSerialNumberComponentId, MachineSerialNumberComponentStatusEnum.Broken.ToString(), staffId);
                    }

                    var newComponentTicket = await _componentReplacementTicketRepository.CreateTicket(staffId, replacementTicket, contract.AccountSignId);

                    await _machineTaskRepository.UpdateTaskStatus(machineTask.MachineTaskId, MachineTaskStatusEnum.Reparing.ToString(), staffId, null);

                    await _componentRepository.MoveComponentQuanityFromAvailableToOnHold(component.ComponentId, createComponentReplacementTicketDto.Quantity);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw new ServiceException(MessageConstant.ComponentReplacementTicket.CreateFail);
                }
            }

            if (contract != null)
            {
                await _notificationService.SendNotificationToCustomerWhenCreateComponentReplacementTicket((int)contract.AccountSignId, (double)replacementTicket.TotalAmount, replacementTicket.ComponentName);
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
