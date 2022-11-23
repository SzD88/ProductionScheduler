﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionScheduler.Application.DTO
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public Guid MachineId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime Date { get; set; }
        public short Hour { get; set; }
    }
}