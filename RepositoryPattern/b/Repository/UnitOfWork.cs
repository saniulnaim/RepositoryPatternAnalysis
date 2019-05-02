using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryPattern.b.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private bool _dispose;

        public UnitOfWork(DbContext context)
        {
            _dbContext = context;
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            if (_dispose)
                return;
            _dispose = true;
        }
        #endregion Implementation of IDisposable

        public void SaveChanges()
        {
            //((IObjectContextAdapter)_dbContext).ObjectContext.SaveChanges();
            //not working so

            _dbContext.SaveChanges();
        }
    }
}
