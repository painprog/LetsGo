using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using IronBarCode;
using LetsGo.Core.Entities;
using LetsGo.DAL;
using LetsGo.UI.Services;
using LetsGo.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace LetsGo.UI.Controllers
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
                Event @event = _goContext.Events.FirstOrDefault(e => e.Id == model.EventId);
                foreach (var item in model.EventTickets)
                {
                    var type = _goContext.EventTicketTypes.FirstOrDefault(t => t.Id == item.Id);
                    string message = $"" +
                        $"<p style=\"text-indent: 20px;\">Здравствуйте, {model.Name}. <br />" +
                        $"Вы совершили покупки билетов на {@event.Name} с {@event.EventStart} до {@event.EventEnd} на сайте <a href=\"#\">ticketbox</a><br /><br />" +
                        $"</p>";

                    type.Sold += item.Count;
                    @event.Sold += item.Count;
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
                        EmailService emailService = new EmailService();
                        await emailService.Send(
                            model.Email,
                            "Билет",
                            message + $"Ваш QR code: <br /> <img width=\"100\" height=\"100\" src=\"https://localhost:44377/Ticket/GetQR?{ticket.Id}\"><img/> <br/> <br />покажите его на входе"
                        );
                    }
                }
                _goContext.Events.Update(@event);
                await _goContext.SaveChangesAsync();
                return Json(new {success = true, redirectToUrl = Url.Action("Index", "Home")});
            }

            return Json(new {success = false});
        }
        public IActionResult GetQR(int id)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(id.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            byte[] data = default(byte[]);
            using (System.IO.MemoryStream sampleStream = new System.IO.MemoryStream())
            {
                qrCodeImage.Save(sampleStream, System.Drawing.Imaging.ImageFormat.Bmp);
                data = sampleStream.ToArray();
            }
            return File(data, "image/jpeg");
        }
    }
}
