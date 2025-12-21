using Domain.Core.Base;
using Domain.Core.ValueObjects;
using System.Linq.Expressions;

namespace Domain.Core.Interface.IRepository
{
    public interface IReadRepositoryGeneric<TEntity> where TEntity : Entity
    {
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default);

        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken token = default);

        Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);

        Task<IEnumerable<TEntity>> FindAsync(
           Expression<Func<TEntity, bool>> condition,
           CancellationToken token = default);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> condition, CancellationToken token = default);
    }
}