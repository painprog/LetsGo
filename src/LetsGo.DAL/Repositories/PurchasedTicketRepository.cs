using LetsGo.Core.Entities;
using LetsGo.Core.Repositories;

namespace LetsGo.DAL.Repositories
{
    public class PurchasedTicketRepository : Repository<PurchasedTicket>, IPurchasedTicketRepository
    {
        public PurchasedTicketRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}