using System;
using System.Collections.Concurrent;
using System.Data;
using System.Threading.Tasks;
using LetsGo.Core;
using LetsGo.Core.Repositories;
using LetsGo.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LetsGo.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly ConcurrentDictionary<Type, object> _repositories;

        private IDbContextTransaction _transaction;

        private bool _disposed;

        public IUserRepository Users { get; }
        public IRoleRepository Roles { get; }
        public IEventCategoryRepository EventCategories { get; }
        public IEventRepository Events { get; }
        public IEventTicketTypeRepository EventTicketTypes { get; }
        public ILocationCategoryRepository LocationCategories { get; }
        public ILocationRepository Locations { get; }
        public IPurchasedTicketRepository PurchasedTickets { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _repositories = new ConcurrentDictionary<Type, object>();

            Users = new UserRepository(context);
            Roles = new RoleRepository(context);
            EventCategories = new EventCategoryRepository(context);
            Events = new EventRepository(context);
            EventTicketTypes = new EventTicketTypeRepository(context);
            LocationCategories = new LocationCategoryRepository(context);
            Locations = new LocationRepository(context);
            PurchasedTickets = new PurchasedTicketRepository(context);
        }

        public Task<int> CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void BeginTransaction(IsolationLevel level)
        {
            _transaction = _context.Database.BeginTransaction(level);
        }

        public void CommitTransaction()
        {
            if (_transaction == null) return;

            _transaction.Commit();
            _transaction.Dispose();

            _transaction = null;
        }

        public void RollbackTransaction()
        {
            if (_transaction == null) return;

            _transaction.Rollback();
            _transaction.Dispose();

            _transaction = null;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return _repositories.GetOrAdd(typeof(TEntity), new Repository<TEntity>(_context)) as IRepository<TEntity>;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _context.Dispose();

            _disposed = true;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}