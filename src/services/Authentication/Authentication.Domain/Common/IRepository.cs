namespace SwapSquare.Authentication.Domain.Common;

public interface IRepository<T> where T : AggregateRoot
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    T Update(T entity, CancellationToken cancellationToken = default);
    T Delete(T entity, CancellationToken cancellationToken = default);
}