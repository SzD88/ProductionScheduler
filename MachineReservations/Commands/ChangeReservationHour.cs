using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineReservations.Api.Commands
{
    public record ChangeReservationHour( 

        Guid ReservationId, 
        string Hour   
        );
      
}
