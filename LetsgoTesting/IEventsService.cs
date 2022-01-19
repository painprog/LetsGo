using LetsGo.Core.Entities;
using LetsGo.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LetsgoTesting
{
    public interface IEventsService
    {
        Task<Event> AddEvent(AddEventViewModel eventView);
        Task<List<EventTicketType>> AddEventTicketTypes(int eventId, List<EventTicketType> ticketTypes);
        Task<List<EventTicketType>> UpdateEventTicketTypes(int eventId, List<EventTicketType> ticketTypes);
        Task DeleteEventTicketTypes(int[] IdsForDelete);
        Task<EditEventViewModel> MakeEditEventViewModel(int id);
        Task<Event> EditEvent(EditEventViewModel model);
        Task<List<Event>> GetEvents(int userId);
        Task<bool> ChangeStatus(string status, int eventId, string cause);
        Task<List<Event>> Events();
        Task<Event> GetEvent(int id);
        Task<EventTicketType> GetEventTicketType(int id);
        List<EventTicketType> EventTicketTypes(int eventId);
        Task<Location> GetLocation(int id);
        string GenerateCode();

    }
}
