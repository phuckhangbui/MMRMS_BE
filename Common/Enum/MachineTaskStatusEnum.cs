namespace Common.Enum
{
    public enum MachineTaskStatusEnum
    {
        Assigned,
        Completed,
        Failed,
        ReAssigned,
        CreateMaintenceTicket,
        Canceled
    }

    public enum MachineTaskTypeEnum
    {
        MachineryCheck,
        ComponentReplacement
    }
}
