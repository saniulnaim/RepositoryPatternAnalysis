using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryPattern.a
{
    public class GenericRepository<T> : IGenericRepository<T>, IDisposable where T : class
    {
        private DbSet<T> _entities;
        private string _errorMessage = string.Empty;
        private bool _isDisposed;
        public EmployeeDBContext Context { get; set; }

        public GenericRepository(IUnitOfWork<EmployeeDBContext> unitOfWork)
                : this(unitOfWork.context)
        {

        }
        public GenericRepository(EmployeeDBContext context)
        {
            _isDisposed = false;
            Context = context;
        }

        public virtual IQueryable<T> Table
        {
            get { return _entities; }
        }

        protected virtual DbSet<T> Entities
        {
            get { return _entities ?? (_entities = Context.Set<T>()); }
        }

        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
            _isDisposed = true;
        }

        public IEnumerable<T> GetAll()
        {
            return Entities.ToList();
        }

        public T GetById(object id)
        {
            return Entities.Find(id);
        }

        public void Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                Entities.Add(entity);

                if (Context == null || _isDisposed)
                    Context = new EmployeeDBContext();
            }
            catch(Exception ex)
            {
                throw new Exception();
            }
        }

        public void BulkInsert(IEnumerable<T> entities)
        {
            try
            {
                if(entities == null)
                {
                    throw new ArgumentNullException("entities");
                }

                //Context.Configuration.AutoDetectChangesEnabled = false;
                Context.Set<T>().AddRange(entities);
                Context.SaveChanges();
            }
            catch(Exception ex)
            {

            }
        }

        public virtual void Update(T entity)
        {
            try
            {
                if(entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                if (Context == null || _isDisposed)
                    Context = new EmployeeDBContext();
                SetEntryModified(entity);
            }
            catch(Exception ex)
            {

            }
        }
        private void SetEntryModified(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                if (Context == null || _isDisposed)
                    Context = new EmployeeDBContext();
                Entities.Remove(entity);
            }
            catch
            {

            }
        }
    }
}
