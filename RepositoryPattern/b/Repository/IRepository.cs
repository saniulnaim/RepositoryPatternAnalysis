using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RepositoryPattern.b.Repository
{
    public interface IRepository : IDisposable
    {
        IUnitOfWork unitOfWork { get; }

        TEntity GetEntity<TEntity>(object keyValue) where TEntity : class;

        IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class;

        IQueryable<TEntity> GetQeury<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class;

        IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, bool>> criteria,
            Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize,
            SortOrder sortOrder = SortOrder.Ascending) where TEntity : class;

        TEntity Single<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;

        TEntity First<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        TEntity FindOne<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;

        int Count<TEntity>() where TEntity : class;

        int Count<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;

        void Add<TEntity>(TEntity entity) where TEntity : class;

        void Attach<TEntity>(TEntity entity) where TEntity : class;

        void Update<TEntity>(TEntity entity) where TEntity : class;

        void Delete<TEntity>(TEntity entity) where TEntity : class;

        void Delete<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;

        //(ISpecification<TEntity> criteria) where TEntity : class;

    }
}
