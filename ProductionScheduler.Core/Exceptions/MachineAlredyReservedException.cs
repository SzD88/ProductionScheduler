namespace ProductionScheduler.Core.Exceptions
{
    public sealed class MachineAlredyReservedException : CustomException
    {
        public string Name { get; }
        public DateTime Date { get; }

        public MachineAlredyReservedException(string name, DateTime date, short hour) :
            base($"Machine: {name} is alredy reserved at date: {date:d} and hour: {hour}")
        {
            Date = date;
        }

    }
}
