﻿namespace ProductionScheduler.Application.Commands
{
    public record ChangeReservationHour(

        Guid ReservationId,
        short Hour
        );

}
