namespace ProductionScheduler.Application.DTO
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public Guid MachineId { get; set; }
        public string EmployeeName { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public int Hour { get; set; }
    }
}
