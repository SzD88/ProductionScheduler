namespace ProductionScheduler.Infrastructure.DAL
{
    public class MSSqlOptions
    {
        public string ConnectionString { get; set; }
        public bool ApplyMigration { get; set; }
    }
}
