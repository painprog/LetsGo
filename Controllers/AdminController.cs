using LetsGo.Models;
using LetsGo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.Controllers
{
    [Authorize(Roles ="superadmin, admin")]
    public class AdminController : Controller
    {
        private readonly LetsGoContext _goContext;

        public AdminController(LetsGoContext goContext)
        {
            _goContext = goContext;
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string adminId)
        {
            var admin = await _goContext.ContextUsers.FirstOrDefaultAsync(a => a.Id == adminId);
            if(admin != null)
            {
                ViewBag.Events = await _goContext.Events.OrderByDescending(e => e.CreatedAt).ToListAsync();
                return View(admin);
            }
            return BadRequest();
        }
    }
}
