using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IronBarCode;
using LetsGo.Core.Entities;
using LetsGo.DAL;
using LetsGo.UI.Services;
using LetsGo.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LetsGo.UI.Controllers
{
    public class TicketController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EventsService _eventService;

        public TicketController(ApplicationDbContext goContext, EventsService eventsService)
        {
            _context = goContext;
            _eventService = eventsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(DetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                Event @event = _eventService.GetEvent(model.EventId).Result;
                string message = $"" +
                    $"<p style=\"text-indent: 20px;\">Здравствуйте, {model.Name}. <br />" +
                    $"Вы совершили покупки билетов на {@event.Name} с {@event.EventStart} до {@event.EventEnd} на сайте <a href=\"#\">ticketbox</a><br /><br />" +
                    $"</p>";

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
                        _context.PurchasedTickets.Add(ticket);
                        
                        GeneratedBarcode QR = QRCodeWriter.CreateQrCode(ticket.QR, 500, QRCodeWriter.QrErrorCorrectionLevel.Highest);
                        await EmailService.Send(
                            model.Email,
                            "Билет",
                            message + $"Тип блиета: {type.Name} <br />Ваш QR code: <br /> {QR.ToHtmlTag()} <br /> <br />Покажите его на входе"
                        );
                    }
                }


                _context.Events.Update(@event);
                await _context.SaveChangesAsync();
                return Json(new {success = true, redirectToUrl = Url.Action("Index", "Home")});
            }

            return Json(new {success = false});
        }
    }
}
