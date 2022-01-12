using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LetsGo.Core.Entities;
using LetsGo.DAL;
using LetsGo.UI.Services;
using LetsGo.UI.Services.Contracts;
using LetsGo.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LetsGo.UI.Controllers
{
    public class TicketController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IBackgroundTaskQueue _backgroundTaskQueue;

        private readonly EventsService _eventService;

        public TicketController(ApplicationDbContext goContext, EventsService eventsService, IBackgroundTaskQueue backgroundTaskQueue)
        {
            _context = goContext;
            _backgroundTaskQueue = backgroundTaskQueue;
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
                        ticket.QR = $"{Request.Scheme}://{Request.Host}/api/ticketcheck/details/" + ticket.TicketIdentifier;
                        _context.PurchasedTickets.Add(ticket);
                        purchasedTickets.Add(ticket);
                    }
                }
                string message = $"" +
                    $"<p style=\"text-indent: 20px;\">Здравствуйте, {model.Name}. <br />" +
                    $"Вы совершили покупки билетов на \"{@event.Name}\" с {@event.EventStart} до {@event.EventEnd} на сайте <a href=\"#\">ticketbox</a><br /><br />" +
                    $"</p>";

                _context.Events.Update(@event);
                await _context.SaveChangesAsync();

                _backgroundTaskQueue.QueueBackgroundWorkItem(async token =>
                {
                    await EmailService.SendTickets(model.Email, "Билет", message, purchasedTickets);
                });

                return Json(new {success = true, redirectToUrl = Url.Action("Index", "Home")});
            }

            return Json(new {success = false});
        }
    }
}
