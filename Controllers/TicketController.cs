using IronBarCode;
using LetsGo.Models;
using LetsGo.Services;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetsGo.Core.Entities;
using LetsGo.DAL;

namespace LetsGo.Controllers
{
    public class TicketController : Controller
    {
        private readonly ApplicationDbContext _goContext;
        public TicketController(ApplicationDbContext goContext)
        {
            _goContext = goContext;
        }

        [HttpPost]
        public async Task<IActionResult> Create(DetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in model.EventTickets)
                {
                    var type = _goContext.EventTicketTypes.FirstOrDefault(t => t.Id == item.Id);
                    Event @event = _goContext.Events.FirstOrDefault(e => e.Id == type.EventId);

                    string message = $"" +
                        $"<p style=\"text-indent: 20px;\">Здравствуйте, {model.Name}. <br />" +
                        $"Вы совершили покупки билетов на {@event.Name} с {@event.EventStart} до {@event.EventEnd} на сайте <a href=\"#\">ticketbox</a><br /><br />" +
                        $"</p>";

                    type.Sold += item.Count;
                    for (int i = 0; i < item.Count; i++)
                    {
                        var ticket = new PurchasedTicket()
                        {
                            CustomerEmail = model.Email,
                            CustomerName = model.Name,
                            CustomerPhone = model.PhoneNumber,
                            EventTicketTypeId = item.Id,
                            //Id = Guid.NewGuid().ToString(),
                            PurchaseDate = DateTime.Now,
                            Scanned = false
                        };
                        _goContext.PurchasedTickets.Add(ticket);
                        GeneratedBarcode QR = QRCodeWriter.CreateQrCode("https://localhost:44377/Home/Index/" + ticket.Id, 500, QRCodeWriter.QrErrorCorrectionLevel.Highest);
                        EmailService emailService = new EmailService();
                        await emailService.Send(
                            model.Email,
                            "Билет",
                            message + $"Ваш QR code: <br /> {QR.ToHtmlTag()} <br/> <br />покажите его на входе"
                        );
                    }
                }

                _goContext.SaveChanges();
                return Json(new {success = true, redirectToUrl = Url.Action("Index", "Home")});
            }

            return Json(new {success = false});
        }
    }
}
