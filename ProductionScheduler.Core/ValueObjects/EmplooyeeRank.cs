namespace ProductionScheduler.Core.ValueObjects
{
    public sealed record EmplooyeeRank
    {
        public string Value { get;}
        public const string Employee = nameof(Employee); 
        public const string Manager = nameof(Manager);
        public const string Admin = nameof(Admin);

        private EmplooyeeRank( string value)
        {
            Value = value;
        }
        public static implicit operator string(EmplooyeeRank rank)
        {
            return rank.Value;
        }

        public static implicit operator EmplooyeeRank(string rank)
        => new EmplooyeeRank(rank);
    }
}
