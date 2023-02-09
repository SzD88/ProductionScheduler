using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.Entities
{
    public sealed class ReservationForService : Reservation
    {
        private ReservationForService()
        { 
        }

        public ReservationForService(ReservationId id, MachineId machineId, 
          Date date, Hour hour) 
            : base (id, machineId, hour, date)
        {
        }
    }
}
