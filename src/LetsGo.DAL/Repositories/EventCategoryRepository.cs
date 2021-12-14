using LetsGo.Core.Entities;
using LetsGo.Core.Repositories;

namespace LetsGo.DAL.Repositories
{
    public class EventCategoryRepository : Repository<EventCategory>, IEventCategoryRepository
    {
        public EventCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}