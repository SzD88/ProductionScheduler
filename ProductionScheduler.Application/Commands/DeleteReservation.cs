﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionScheduler.Application.Commands
{
    public record DeleteReservation(

        Guid ReservationId

          );

}
