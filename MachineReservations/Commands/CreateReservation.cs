using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineReservations.Api.Commands
{
    public record CreateReservation(

        Guid MachineId, 
        Guid ReservationId,
        DateTime Date, 
        string EmployeeName, 
        string Hour   
        
        );
     
}
