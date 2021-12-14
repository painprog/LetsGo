using LetsGo.Core.Entities;
using LetsGo.Core.Repositories;

namespace LetsGo.DAL.Repositories
{
    public class EventTicketTypeRepository : Repository<EventTicketType>, IEventTicketTypeRepository
    {
        public EventTicketTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}