using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using LetsGo.Core.Entities;
using LetsGo.DAL;
using LetsGo.UI.Services;
using LetsGo.UI.Services.Contracts;
using LetsGo.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using Aspose.Pdf;
using Aspose.Pdf.Text;

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
                //Event @event = _eventService.GetEvent(model.EventId).Result;
                Event @event = _context.Events.FirstOrDefault(e => e.Id == model.EventId);

                foreach (var item in model.EventTickets)
                {
                    //var type = _eventService.GetEventTicketType(item.Id).Result;
                    var type = _context.EventTicketTypes.FirstOrDefault(t => t.Id == item.Id);

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
                _context.SaveChanges();

                _backgroundTaskQueue.QueueBackgroundWorkItem(async token =>
                {
                    await EmailService.SendTickets(model.Email, "Билет", message, purchasedTickets);
                });

                return Json(new {success = true, redirectToUrl = Url.Action("Index", "Home")});
            }

            return Json(new {success = false});
        }
        public OptimizedMemoryStream GetQRStream(string QR)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(QR, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            byte[] data = default(byte[]);
            OptimizedMemoryStream sampleStream = new OptimizedMemoryStream();
            qrCodeImage.Save(sampleStream, System.Drawing.Imaging.ImageFormat.Bmp);
            return sampleStream;
        }
        public IActionResult GetQR(string QR)
        {
            var data = GetQRStream(QR).ToArray();
            return File(data, "image/jpeg");

        }
        public IActionResult GetPDF(string QR)
        {
            Document document = new Document();
            Page page = document.Pages.Add();
            page.AddImage(GetQRStream(QR), new Aspose.Pdf.Rectangle(20, 730, 120, 830));
            var descriptionText = "Show this qr code to the ticket controller.";
            var description = new TextFragment(descriptionText);
            description.TextState.Font = FontRepository.FindFont("Times New Roman");
            description.TextState.FontSize = 14;
            description.HorizontalAlignment = HorizontalAlignment.Right;
            page.Paragraphs.Add(description);
            byte[] data = default(byte[]);
            using (System.IO.MemoryStream sampleStream = new System.IO.MemoryStream())
            {
                document.Save(sampleStream);
                data = sampleStream.ToArray();
            }
            return File(data, "application/pdf");
        }
    }
}
