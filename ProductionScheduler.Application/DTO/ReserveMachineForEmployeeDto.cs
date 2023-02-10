namespace ProductionScheduler.Application.DTO
{
    public record ReserveMachineForEmployeeDto(
        Guid MachineId,
        DateTime Date,
        int Hour
        );
}
