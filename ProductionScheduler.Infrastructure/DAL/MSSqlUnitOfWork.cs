namespace ProductionScheduler.Infrastructure.DAL
{
    internal sealed class MSSqlUnitOfWork : IUnitOfWork
    {
        public MSSqlUnitOfWork(ProductionSchedulerDbContext dbContext)
        {
            _dbContext = dbContext;
        } 
        public ProductionSchedulerDbContext _dbContext { get; }

        public async Task ExecuteAsync(Func<Task> action)
        {
           await using var transaction =  await _dbContext.Database.BeginTransactionAsync(); 
            try
            {
                await action(); 
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();  
                throw;
            }
        }
    }
}
