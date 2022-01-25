using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using MimeKit.Text;
using System.Security.Policy;
using LetsGo.Core.Entities;
using System.Collections.Generic;

namespace LetsGo.UI.Services
{
    public class EmailService
    {
        public static async Task Send(string emailTo, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Администрация сайта", "ticketboxkg@gmail.com"));
            email.To.Add(MailboxAddress.Parse(emailTo));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = message };
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 25, false);
                await client.AuthenticateAsync("ticketboxkg@gmail.com", "9x8Q3NB8uX");
                await client.SendAsync(email);
                await client.DisconnectAsync(true);
            }
        }

        public static async Task SendTickets(string emailTo, string subject, string message, IEnumerable<PurchasedTicket> tickets)
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 25, false);
                await client.AuthenticateAsync("ticketboxkg@gmail.com", "9x8Q3NB8uX");
                foreach (var item in tickets)
                {
                    var email = new MimeMessage();
                    email.From.Add(new MailboxAddress("Администрация сайта", "ticketboxkg@gmail.com"));
                    email.To.Add(MailboxAddress.Parse(emailTo));
                    email.Subject = subject;
                    email.Body = new TextPart(TextFormat.Html) {
                        Text = message + $"Ваш QR code: <br /> <img width=\"100\" height=\"100\" src=\"https://localhost:44377/Ticket/GetQR?QR={item.TicketIdentifier}\">" +
                        $"<img/> <br/> <br />покажите его на входе<br/><a href=\"https://localhost:44377/Ticket/GetPDF?QR={item.TicketIdentifier}\">Ваш билет в pdf формате.<a/>"
                    };
                    await client.SendAsync(email);
                }
                await client.DisconnectAsync(true);
            }
        }
    }
}
