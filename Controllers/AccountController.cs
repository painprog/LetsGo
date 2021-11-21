using LetsGo.Models;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LetsGo.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private LetsGoContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, LetsGoContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public IActionResult Profile()
        {
            Event @event = new Event
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Концерт «Интерстеллар. Simple Music Ensemble»",
                Description = "Музыка из культового фильма «Интерстеллар» среди растений – 26 сентября! Автор завораживающих саундтреков к фантастическому эпосу Кристофера Нолана про задыхающуюся Землю, космические полеты и парадоксы времени – один из величайших кинокомпозиторов современности Ханс Циммер, лауреат премии «Оскар», двукратный лауреат «Золотого глобуса», трехкратный лауреат «Грэмми».",
                CreatedAt = DateTime.Now,
                EventStart = new DateTime(2021, 11, 19),
                EventEnd = new DateTime(2021, 11, 19),
                PosterImage = "10101010011001.jpeg",
                Categories = JsonSerializer.Serialize(new List<EventCategory> {
                    new EventCategory { Name = "классика" },
                    new EventCategory { Name = "инструментальная музыка" }
                }),
                //Categories = "{ Name: \"классика\", Name: \"инструментальная музыка\" }",
                AgeLimit = 0,
                TicketLimit = 450,
                LocationId = "local",
                Location = new Location
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "ВДНХ (Выставка достижений народного хозяйства)",
                    Address = "пр-т Мира, 119, Москва, Россия, 129223",
                    Phones = "",
                    Description = "Вы́ставка достиже́ний наро́дного хозя́йства — выставочный комплекс в Останкинском районе Северо-Восточного административного округа города Москвы, второй по величине выставочный комплекс в городе. Входит в 50 крупнейших выставочных центров мира.",
                    Categories = JsonSerializer.Serialize(new List<LocationCategory> {
                        new LocationCategory{ Name = "выставочный центр" }
                    })
                }
            };
            var tickets = new List<EventTicketType> {
                    new EventTicketType { Name = "vip", EventId = @event.Id, Count = 50, Sold = 0, Price = 800, Id = Guid.NewGuid().ToString() },
                    new EventTicketType { Name = "classic", EventId = @event.Id, Count = 80, Sold = 0, Price = 400, Id = Guid.NewGuid().ToString() }
                };
            var user = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
            ProfileViewModel viewModel = new ProfileViewModel
            {
                Events = new List<Event> { @event },
                User = user
            };
            return View(viewModel);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                User user = await _userManager.FindByEmailAsync(model.LoginOrEmail) ?? await _userManager.FindByNameAsync(model.LoginOrEmail);
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(
                  user,
                  model.Password,
                  model.RememberMe,
                  false
                  );
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
