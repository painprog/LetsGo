using LetsGo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LetsGo.Services
{
    public class EventsService
    {
        private readonly LetsGoContext _goContext;
        private IWebHostEnvironment _appEnvironment;

        public EventsService(LetsGoContext goContext, IWebHostEnvironment appEnvironment)
        {
            _goContext = goContext;
            _appEnvironment = appEnvironment;
        }

        public async Task<Event> GetEvent(IFormFile uploadImage, Event _event,
            string[] categories, string AgeLimit)
        {
            string pathImage = "/posters/" + uploadImage.FileName;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + pathImage, FileMode.Create))
                await uploadImage.CopyToAsync(fileStream);

            if (categories.Length > 0)
            {
                string jsonCateg = JsonSerializer.Serialize<string[]>(categories);
            }
            _event.PosterImage = pathImage;
            _event.CreatedAt = DateTime.Now;
            _event.AgeLimit = Convert.ToInt32(AgeLimit);
            await _goContext.Events.AddAsync(_event);
            await _goContext.SaveChangesAsync();
            return _event;
        }
    }
}
