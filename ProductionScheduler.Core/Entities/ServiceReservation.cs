﻿using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.Entities
{
    public sealed class ServiceReservation : Reservation
    {
        public ServiceReservation(ReservationId id, MachineId machineId, 
          Date date, Hour hour) 
            : base (id, machineId, hour, date)
        {
        }

       
     
    }
}
