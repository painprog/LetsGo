using LetsGo.Core.Entities;
using LetsGo.UI.Services;
using LetsGo.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsgoTesting
{
    public class TestingTicketController: Controller
    {
            private readonly IDbContext _context;
            private readonly IEventsService _eventService;

            public TestingTicketController(IDbContext goContext, IEventsService eventsService)
            {
                _context = goContext;
                _eventService = eventsService;
            }

        [HttpPost]
        public async Task<IActionResult> Create(DetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<PurchasedTicket> purchasedTickets = new List<PurchasedTicket>();
                Event @event = _eventService.GetEvent(model.EventId).Result;

                foreach (var item in model.EventTickets)
                {
                    var type = _eventService.GetEventTicketType(item.Id).Result;

                    type.Sold += item.Count;
                    @event.Sold += item.Count;
                    for (int i = 0; i < item.Count; i++)
                    {
                        var ticket = new PurchasedTicket()
                        {
                            TicketIdentifier = Guid.NewGuid().ToString(),
                            CustomerEmail = model.Email,
                            CustomerName = model.Name,
                            CustomerPhone = model.PhoneNumber,
                            EventTicketType = type,
                            EventTicketTypeId = type.Id,
                            PurchaseDate = DateTime.Now,
                            Scanned = false
                        };
                        ticket.QR = "https://localhost:44377/api/ticketcheck/details/" + ticket.TicketIdentifier;
                        _context.Add(ticket);
                        purchasedTickets.Add(ticket);
                    }
                }
                string message = $"" +
                    $"<p style=\"text-indent: 20px;\">Здравствуйте, {model.Name}. <br />" +
                    $"Вы совершили покупки билетов на \"{@event.Name}\" с {@event.EventStart} до {@event.EventEnd} на сайте <a href=\"#\">ticketbox</a><br /><br />" +
                    $"</p>";

                _context.Update(@event);
                await _context.SaveChangesAsync();
                await EmailService.SendTickets(model.Email, "Билет", message, purchasedTickets);

                return Json(new { success = true, redirectToUrl = Url.Action("Index", "Home") });
            }

            return Json(new { success = false });
        }
    }
}
