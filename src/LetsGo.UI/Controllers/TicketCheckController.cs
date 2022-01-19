using LetsGo.Core.Entities;
using LetsGo.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketCheckController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TicketCheckController(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET api/ticketcheck/status/{id}
        [HttpGet("status/{id}")]
        public async Task<ActionResult<PurchasedTicket>> CheckStatus(string id)
        {
            PurchasedTicket purchasedTicket = await _context.PurchasedTickets.FirstOrDefaultAsync(t => t.TicketIdentifier == id);
            if (purchasedTicket == null)
                return NotFound();
            return new JsonResult(purchasedTicket.Scanned);
        }

        //GET api/ticketcheck/skip/{id}
        [HttpGet("skip/{id}")]
        public async Task<ActionResult<PurchasedTicket>> Skip(string id)
        {
            PurchasedTicket purchasedTicket = await _context.PurchasedTickets.FirstOrDefaultAsync(t => t.TicketIdentifier == id);
            if (purchasedTicket == null)
                return NotFound();

            purchasedTicket.Scanned = true;
            _context.PurchasedTickets.Update(purchasedTicket);
            await _context.SaveChangesAsync();
            return new JsonResult($"Status has been changed to {purchasedTicket.Scanned}");
        }

        //GET api/ticketcheck/rollback/{id}
        [HttpGet("rollback/{id}")]
        public async Task<ActionResult<PurchasedTicket>> RollBack(string id)
        {
            PurchasedTicket purchasedTicket = await _context.PurchasedTickets.FirstOrDefaultAsync(t => t.TicketIdentifier == id);
            if (purchasedTicket == null)
                return NotFound();

            if(purchasedTicket.Scanned == true)
            {
                purchasedTicket.Scanned = false;
                _context.PurchasedTickets.Update(purchasedTicket);
                await _context.SaveChangesAsync();
                return new JsonResult($"Status has been changed to {purchasedTicket.Scanned}");

            }
            return new JsonResult("Status is already False");
        }
    }
}
