using System.Threading.Tasks;
using LetsGo.Core;
using LetsGo.Core.Entities;
using LetsGo.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LetsGo.UI.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketCheckController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWorkFactory _uowFactory;

        public TicketCheckController(ApplicationDbContext context, IUnitOfWorkFactory uowFactory)
        {
            _context = context;
            _uowFactory = uowFactory;
        }

        //GET api/ticketcheck/status/{id}
        [HttpGet("status/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "apiclient")]
        public async Task<ActionResult<PurchasedTicket>> CheckStatus(string id)
        {
            using (var uow = _uowFactory.MakeUnitOfWork())
            {
                PurchasedTicket purchasedTicket = await uow.PurchasedTickets
                    .Find(t => t.TicketIdentifier == id)
                    .FirstOrDefaultAsync();

                if (purchasedTicket == null)
                    return NotFound();

                return new JsonResult(purchasedTicket.Scanned);
            }
        }

        //GET api/ticketcheck/skip/{id}
        [HttpGet("skip/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "apiclient")]
        public async Task<ActionResult<PurchasedTicket>> Skip(string id)
        {
            using (var uow = _uowFactory.MakeUnitOfWork())
            {
                PurchasedTicket purchasedTicket = await uow.PurchasedTickets.Find(t => t.TicketIdentifier == id).FirstOrDefaultAsync();
                
                if (purchasedTicket == null)
                    return NotFound();

                purchasedTicket.Scanned = true;
                uow.PurchasedTickets.Update(purchasedTicket);
                await uow.CompleteAsync();
                return new JsonResult($"Status has been changed to {purchasedTicket.Scanned}");
            }
        }

        //GET api/ticketcheck/rollback/{id}
        [HttpGet("rollback/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "apiclient")]
        public async Task<ActionResult<PurchasedTicket>> RollBack(string id)
        {
            using (var uow = _uowFactory.MakeUnitOfWork())
            {
                PurchasedTicket purchasedTicket = await uow.PurchasedTickets.Find(t => t.TicketIdentifier == id).FirstOrDefaultAsync();
                
                if (purchasedTicket == null)
                    return NotFound();

                if (purchasedTicket.Scanned)
                {
                    purchasedTicket.Scanned = false;
                    uow.PurchasedTickets.Update(purchasedTicket);
                    await uow.CompleteAsync();
                    return new JsonResult($"Status has been changed to {purchasedTicket.Scanned}");

                }
                return new JsonResult("Status is already False");
            }
        }
    }
}
