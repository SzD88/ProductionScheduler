using ProductionScheduler.Application.Services;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProductionScheduler.Infrastructure.DAL.Repositories
{
	internal class MSSqlPeriodMachineReservationRepository : IPeriodMachineReservationRepository
	{

		private readonly ProductionSchedulerDbContext _periodMachineReservations;
		public MSSqlPeriodMachineReservationRepository(ProductionSchedulerDbContext dbContext)
		{
			_periodMachineReservations = dbContext;
		}
		public PeriodMachineReservation Get(MachineId id)
		{
			return _periodMachineReservations.PeriodMachineReservations
				.Include(x => x.Reservations ) // eager loading a nie lazy loading 
				.SingleOrDefault(x => x.Id == id);
		}

		public IEnumerable<PeriodMachineReservation> GetAll()
		{
			return _periodMachineReservations.PeriodMachineReservations
				.Include(x => x.Reservations) // eager loading a nie lazy loading 
				.ToList();
		}


		public void Create(PeriodMachineReservation command)
		{
			_periodMachineReservations.Add(command);
			_periodMachineReservations.SaveChanges();
		}
		public void Update(PeriodMachineReservation command)
		{
			_periodMachineReservations.Update(command);
			_periodMachineReservations.SaveChanges();
		}
		public void Delete(PeriodMachineReservation command)
		{
			_periodMachineReservations.Remove(command);
			_periodMachineReservations.SaveChanges();
		}


	}
}
