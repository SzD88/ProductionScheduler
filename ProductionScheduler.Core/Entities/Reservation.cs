using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.Entities;

public abstract class Reservation
{
    public ReservationId Id { get; }
    public MachineId MachineId { get; }
    public Hour Hour { get; private set; }
    public Date Date { get; }

    public Reservation(ReservationId id, MachineId machineId,
          Hour hour, Date date)
    {
        Id = id;
        MachineId = machineId;
        ChangeHourOfReservation(hour);
        Date = date;
    }
    private Reservation()
    {

    }
    public void ChangeHourOfReservation(Hour hour)
    {
        Hour = hour;
    }
}

