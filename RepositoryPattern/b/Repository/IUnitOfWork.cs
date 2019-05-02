using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryPattern.b.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
    }


}
