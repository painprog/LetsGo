using System;
using System.Data;
using System.Threading.Tasks;
using LetsGo.Core.Repositories;

namespace LetsGo.Core
{
    public interface IUnitOfWork : IDisposable
    {

        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        IEventCategoryRepository EventCategories { get; }
        IEventRepository Events { get; }
        IEventTicketTypeRepository EventTicketTypes { get; }
        ILocationCategoryRepository LocationCategories { get; }
        ILocationRepository Locations { get; }
        IPurchasedTicketRepository PurchasedTickets { get; }

        Task<int> CompleteAsync();
        void BeginTransaction();
        void BeginTransaction(IsolationLevel level);
        void RollbackTransaction();
        void CommitTransaction();

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}