namespace ProductionScheduler.Application.Abstractions
{

    //                                                   Musi byc typem referencyjnym (class) i musi implementowac interface iquery od tresult     
    public interface IQueryHandler<in TQuery, TResult> where TQuery : class, IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
