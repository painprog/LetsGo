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
            TestingTicketController ticketController = new TestingTicketController(mockDb.Object,mock.Object);
            ticketController.ModelState.AddModelError("Email", "Почта обязательна для заполнения");
            DetailsViewModel detailsView = new DetailsViewModel();

            // Act
            var result = await ticketController.Create(detailsView);

            //Assert
            JsonResult jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(new { success = false }, jsonResult.Value);
        }
    }
}
