using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Text;
using System.Threading.Tasks;

namespace LetsgoTesting
{
    public interface IDbContext: IDisposable
    {
        IDbSet<T> Set<T>() where T : class;
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
        IDbSet<T> Add<T>(T entity) where T : class;
        IDbSet<T> Update<T>(T entity) where T : class;
    }
}
