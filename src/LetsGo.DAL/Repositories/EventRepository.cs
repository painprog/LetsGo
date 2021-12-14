using LetsGo.Core.Entities;
using LetsGo.Core.Repositories;

namespace LetsGo.DAL.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}