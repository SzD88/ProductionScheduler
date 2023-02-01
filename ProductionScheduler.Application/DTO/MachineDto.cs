namespace ProductionScheduler.Application.DTO
{
    public class MachineDto
    { 
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public IEnumerable<ReservationDto> Reservations { get; set; }
    }
}
