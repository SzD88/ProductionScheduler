using ProductionScheduler.Core.ValueObjects;

namespace ProductionScheduler.Core.Entities;

public class Reservation
{
    public ReservationId Id { get; }
    public MachineId MachineId { get; }
    public EmployeeName EmployeeName { get; }
    public Hour Hour { get; private set; }
    public Date Date { get; }

    public Reservation(ReservationId id, MachineId machineId,
        EmployeeName employeeName, Hour hour, Date dateTime)
    {
        Id = id;
        MachineId = machineId;
        EmployeeName = employeeName;
        ChangeHourOfReservation(hour);
        Date = dateTime;
    }
    private Reservation()
    {

    }
    public void ChangeHourOfReservation(Hour hour)
    {
        Hour = hour;
    }
}

