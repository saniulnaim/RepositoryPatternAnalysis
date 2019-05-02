using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;

namespace RepositoryPattern.b.Repository
{
    public class Repository : IRepository, IDisposable
    {
        private bool bDisposed;
        private DbContext context;
        private IUnitOfWork unitOfWork;

        public Repository()
        {

        }

        public Repository(DbContext contextObj)
        {
            if (contextObj == null)
                throw new ArgumentNullException("context");
            this.context = contextObj;
        }

        public Repository(ObjectContent contextObj)
        {
            if (contextObj == null)
                throw new ArgumentNullException("context");
            context = new DbContext(contextObj, true);
        }

        public void Dispose()
        {
            Close();
        }

        public void Close()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool bDisposing)
        {
            if (!bDisposed)
            {
                if (bDisposing)
                {
                    if(null != context)
                    {
                        context.Dispose();
                    }
                }
                bDisposed = true;
            }
        }

        protected DbContext DbContext
        {
            get
            {
                if (context == null)
                    throw new ArgumentNullException("context");
                return context;
            }
        }

        public IUnitOfWork UnitOfWork
        {
            get
            {
                if(unitOfWork == null)
                {
                    unitOfWork = new UnitOfWork(DbContext);
                }
                return unitOfWork;
            }
        }

        public TEntity GetByKey<TEntity>(object keyValue) where TEntity : class
        {
            EntityKey key = GetEntityKey<TEntity>(keyValue);

            object originalItem;
            if(((IObjectContextAdapter)DbContext).ObjectContext.TryGetObjectByKey(key, out originalItem)){
                return (TEntity)originalItem;
            }

            return default(TEntity);
        }

        public IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class
        {
            string entityName = GetEntityName(TEntity)();
            return ((IObjectContextAdapter)DbContext).ObjectContext.CreateQuery<TEntity>(entityName);
        }

        public IQueryable<TEntity> GetQuery<TEntity>
            (Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return GetQuery<TEntity>().Where(predicate);
        }

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return GetQuery<TEntity>().AsEnumerable();
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>
            (Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex,
            int pageSize, SortOrder sortOrder = SortOrder.Ascending)
            where TEntity : class
        {
           if(sortOrder == SortOrder.Ascending)
            {
                return GetQuery<TEntity>()
                    .OrderBy(orderBy)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .AsEnumerable();
            }
            return GetQuery<TEntity>()
                   .OrderByDescending(orderBy)
                   .Skip((pageIndex - 1) * pageSize)
                   .Take(pageSize)
                   .AsEnumerable();
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>
            (Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, TOrderBy>> orderBy, 
            int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class
        {
            if(sortOrder == SortOrder.Ascending)
            {
                return GetQuery(criteria)
                       .OrderBy(orderBy)
                       .Skip((pageIndex - 1) * pageSize)
                       .Take(pageSize)
                       .AsEnumerable();
            }
            return GetQuery(criteria)
                   .OrderByDescending(orderBy)
                   .Skip((pageIndex - 1) * pageSize)
                   .Take(pageSize)
                   .AsEnumerable();
        }


    }
}
