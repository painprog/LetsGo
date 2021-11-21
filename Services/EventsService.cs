using LetsGo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.Services
{
    public class EventsService
    {
        private readonly LetsGoContext _goContext;
        private IWebHostEnvironment _appEnvironment;
        private IMemoryCache _cache;

        public EventsService(LetsGoContext goContext, IWebHostEnvironment appEnvironment, IMemoryCache cache)
        {
            _goContext = goContext;
            _appEnvironment = appEnvironment;
            _cache = cache;
        }

        public async Task<Event> GetEvent(string id)
        {
            Event Event = null;
            if (!_cache.TryGetValue(id, out Event))
            {
                Event = await _goContext.Events.Include(e => e.Location).FirstOrDefaultAsync(p => p.Id == id);
                if (Event != null)
                {
                    _cache.Set(Event.Id, Event, 
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return Event;
        }
    }
}
