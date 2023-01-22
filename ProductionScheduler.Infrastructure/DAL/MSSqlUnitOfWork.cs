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
                await action(); // co by to nie bylo #refactor // to jest jakas czynnosc, bedziesz tu pewnie przekazywal zaraz
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(); // nie ma to wiekszego znaczenia ale jest...#31 11:20
                throw;
            }
        }
    }
}
