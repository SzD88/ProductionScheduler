using ProductionScheduler.Core.Abstractions;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.Policies;
using ProductionScheduler.Core.ValueObjects;
using ProductionScheduler.Core.Exceptions;

namespace ProductionScheduler.Core.DomainServices
{
    internal sealed class MachineReservationService : IMachineReservationService
    {
        // tu przypisano wwzystkie 3 polityki #refactor usun
        private readonly IEnumerable<IReservationPolicy> _policies;
        private readonly IClock _clock;

        public MachineReservationService(IEnumerable<IReservationPolicy> policies, IClock clock)
        {
            _policies = policies;
            _clock = clock;
        }

        public void ReserveMachineForService(IEnumerable<Machine> allMachines, Date date, Hour hour)
        {
            foreach (var machine in allMachines)
            {
                var reservationsForSameDate = machine.Reservations
                    .Where(x => x.Date == date)
                    .Where(y => y.Hour == hour);

                //docelowo to ma robic co innego, sprubj to potem przerobic , ma to 
                // 1. przeszukac wszystkie maszyny i w nich rezerwacje
                // 2. Sprawdzic date i godzine
                //3. Nadpisac zadana godzine i date nowa rezerwacja for service 

                //tutaj bedzie pozmieniane pozniej obecnie tak to zosdtawie, to ma byc na zasadzie serwisu calej hali

                machine.RemoveReservations(reservationsForSameDate);

                var serviceReservation = new ServiceReservation( ReservationId.Create() , machine.Id, date, hour);
                machine.AddReservation(serviceReservation, new Date( _clock.Current()));
            }
        }

        // wzorzec #refactor wzorzec strategii - if do pojedynczej klasy #tu koniec 16 min

        public void ReserveMachineForUser(IEnumerable<Machine> allMachineReservations,
            EmplooyeeRank rank, Machine periodiMachineReservation, MachineReservation reservation)
        {

            // #refactor for future - mozesz na podstawie reservation sprawdzac w policy np godziny dla danej rangi pracownika, po prostu przekaz 
            // reservation to metody can reserve - mozna to rozbudować potem i bedzie w pytke 


            var machineToReserveId = periodiMachineReservation.Id;
            // przypisz te 1 polityke ktora zwroci true bo rank == jobtitle #refactor 
            var policy = _policies.SingleOrDefault(x => x.CanBeApplied(rank));
            if (policy is null)
            {
                throw new NoReservationPolicyFoundException(rank);
            } //#refactor

            //var totalEmployeeReservations = allMachineReservations
            //    .SelectMany(x => x.Reservations)
            //    .OfType<MachineReservation>()
            //    .Count(x => x.EmployeeName ==  );
            if (policy.CanReserve(allMachineReservations, reservation.EmployeeName) is false)
            {
                throw new CannotReserveMachineException(machineToReserveId   );
            }

            periodiMachineReservation.AddReservation(reservation, new Date( _clock.Current()));
        }
    }
}
