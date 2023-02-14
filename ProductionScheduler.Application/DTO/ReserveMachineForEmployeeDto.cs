namespace ProductionScheduler.Application.DTO
{
    public record ReserveMachineForEmployeeDto(

        Guid MachineId,
        Guid UserId,
        DateTime Date,
        int Hour,
        string employeeName
        );
}
