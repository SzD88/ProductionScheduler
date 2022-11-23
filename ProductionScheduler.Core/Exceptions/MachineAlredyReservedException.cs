namespace ProductionScheduler.Core.Exceptions
{
    public sealed class MachineAlredyReservedException : CustomException
    {
        public string Name { get; }
        public DateTime Date { get; }

        public MachineAlredyReservedException(string name, DateTime date) :
            base($"Machine: {name} is alredy reserved at date: {date:d}")
        {
            Date = date;
        }

    }
}
