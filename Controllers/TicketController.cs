using LetsGo.Models;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.Controllers
{
    public class TicketController : Controller
    {
        private readonly LetsGoContext _goContext;
        public TicketController(LetsGoContext goContext)
        {
            _goContext = goContext;
        }

        [HttpPost]
        public IActionResult Create(DetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in model.EventTickets)
                {
                    var type = _goContext.EventTicketTypes.FirstOrDefault(t => t.Id == item.Id);
                    Event @event = _goContext.Events.FirstOrDefault(e => e.Id == type.EventId);

                    string message = $"" +
                        $"Здравствуйте, {model.Name}. " +
                        $"Вы совершили покупки билетов на {@event.Name} с {@event.EventStart} до {@event.EventEnd} на сайте <a href=\"#\">ticketbox</a><br /><br />" +
                        $"Вот ваши билеты:<br />";

                    type.Sold += item.Count;
                    for (int i = 0; i < item.Count; i++)
                    {
                        var ticket = new PurchasedTicket()
                        {
                            CustomerEmail = model.Email,
                            CustomerName = model.Name,
                            CustomerPhone = model.PhoneNumber,
                            EventTicketTypeId = item.Id,
                            Id = Guid.NewGuid().ToString(),
                            PurchaseDate = DateTime.Now,
                            Scanned = false
                        };
                        _goContext.PurchasedTickets.Add(ticket);
                    }
                }

                _goContext.SaveChanges();
                return Json(new {success = true, redirectToUrl = Url.Action("Index", "Home")});
            }

            return Json(new {success = false});
        }
    }
}
