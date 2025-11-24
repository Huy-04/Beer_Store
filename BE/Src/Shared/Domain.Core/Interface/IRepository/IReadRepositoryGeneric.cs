using Domain.Core.Base;
using System.Linq.Expressions;

namespace Domain.Core.Interface.IRepository
{
    public interface IReadRepositoryGeneric<TEntity> where TEntity : Entity
    {
        public Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default);

        public Task<TEntity?> GetByIdAsync(Guid id, CancellationToken token = default);

        public Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);

        public Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> condition,
            CancellationToken token = default);

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> condition, CancellationToken token = default);

        // Child
        public Task<IEnumerable<TChild>> GetAllChildAsync<TChild>(
            Expression<Func<TEntity, IEnumerable<TChild>>> navigation,
            CancellationToken token = default);

        public Task<TChild?> GetChildById<TChild>(
           Expression<Func<TEntity, IEnumerable<TChild>>> navigation,
           Expression<Func<TChild, bool>> childCondition,
           CancellationToken token = default);

        public Task<bool> ExistsChildById<TChild>(
            Expression<Func<TEntity, IEnumerable<TChild>>> navigation,
            Expression<Func<TChild, bool>> childCondition,
            CancellationToken token);

        public Task<IEnumerable<TChild>> FindChildAsync<TChild>(
            Expression<Func<TEntity, IEnumerable<TChild>>> navigation,
            Expression<Func<TChild, bool>> childCondition,
            CancellationToken token = default);
    }
}