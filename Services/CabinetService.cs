using LetsGo.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.Services
{
    public class CabinetService
    {
        private readonly EventsService _service;
        private readonly LetsGoContext _context;
        private readonly UserManager<User> _userManager;

        public CabinetService(EventsService service, LetsGoContext context, UserManager<User> userManager)
        {
            _service = service;
            _context = context;
            _userManager = userManager;
        }
    }
}
