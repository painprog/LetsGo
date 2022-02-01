using LetsGo.Core.Entities;
using LetsGo.DAL;
using LetsGo.DAL.Contracts;
using LetsGo.UI.Controllers;
using LetsGo.UI.Services;
using LetsGo.UI.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LetsgoTesting
{
    public class TicketTest
    {
        public Mock<IEventsService> mock = new Mock<IEventsService>();
        public Mock<IDbContext> mockDb = new Mock<IDbContext>();

        [Fact]
        public async void CreateReturnsJsonResultWithSuccesFalse()
        {
            // Arrange
            TestingTicketController ticketController = new TestingTicketController(mockDb.Object, mock.Object);
            ticketController.ModelState.AddModelError("Email", "Почта обязательна для заполнения");
            DetailsViewModel detailsView = new DetailsViewModel();

            // Act
            var result = await ticketController.Create(detailsView);

            //Assert
            JsonResult jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(new { success = false }, jsonResult.Value);
        }

        [Fact]
        public async void CreateReturnsJsonResultWithSuccesTrue()
        {
            // Arrange
            DetailsViewModel detailsView = new DetailsViewModel 
            { EventId = 1,
              EventTickets = GetTestEventTicketTypes()
            };
            mock.Setup(r => r.GetEvent(detailsView.EventId).Result).Returns(GetTestEvents().
                FirstOrDefault(e => e.Id == detailsView.EventId));
            foreach (var item in detailsView.EventTickets)
            {
                mock.Setup(r => r.GetEventTicketType(item.Id).Result).Returns(GetTestEventTicketTypes().
                                FirstOrDefault(e => e.Id == item.Id));
            }
     
            TestingTicketController ticketController = new TestingTicketController(mockDb.Object, mock.Object);
        
            // Act
            var result = await ticketController.Create(detailsView);
        
            //Assert
            JsonResult jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(new { success = true }, jsonResult.Value);
        }

        public List<Event> GetTestEvents()
        {
            var events = new List<Event>
           {
             new Event { Id=1, Name="Sensation"},
             new Event { Id=2, Name="Kazantip"},
             new Event { Id=3, Name="OctoberFest"},
           };
            return events;
        }

        public List<EventTicketType> GetTestEventTicketTypes()
        {
            var eventsTicTypes = new List<EventTicketType>
           {
             new EventTicketType { Id=1, Name="vip", Count=3},
             new EventTicketType { Id=2, Name="bronze", Count=4},
             new EventTicketType { Id=3, Name="gold", Count=5},
           };
            return eventsTicTypes;
        }



    }


}

