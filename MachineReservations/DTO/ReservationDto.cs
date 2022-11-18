using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineReservations.Api.DTO
{
    public class ReservationDto
    {
        public Guid Id { get; set; } = Guid.NewGuid ();
        public Guid MachineId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime Date { get; set; }
    }
}
