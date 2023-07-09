using EShop.Domain.DomainModels;
using EShop.Domain.DTO;
using EShop.Repository.Interface;
using EShop.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EShop.Services.Implementation
{
    public class TicketService : ITicketService
    {
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<TickedInCard> _ticketInCardRepository;
        private readonly IUserRepository _userRepository;
        public TicketService(IRepository<Ticket> ticketRepository, IRepository<TickedInCard> ticketInCardRepository, IUserRepository userRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _ticketInCardRepository = ticketInCardRepository;
        }

        public bool AddToCart(AddToCardDto item, string userID)
        {
            var user = this._userRepository.Get(userID);

            var userCard = user.UserCard;

            if (item.TicketId != null && userCard != null)
            {
                var ticket = this.GetDetailsForTicket(item.TicketId);
                if (ticket != null)
                {
                    TickedInCard itemToAdd = new TickedInCard
                    {
                        Ticket = ticket,
                        TicketId = ticket.Id,
                        Card = userCard,
                        CardId = userCard.Id,
                        Quantity = item.Quantity
                    };
                    this._ticketInCardRepository.Insert(itemToAdd);
                    return true;
                }
                return false;
            }
            return false;
        }

        public void CreateNewTicket(Ticket t)
        {
            this._ticketRepository.Insert(t);
        }

        public void DeleteTicket(Guid id)
        {
            var ticket = this.GetDetailsForTicket(id);
            this._ticketRepository.Delete(ticket);
        }

        public List<Ticket> GetAllTickets()
        {
            return this._ticketRepository.GetAll().ToList();
        }

        public Ticket GetDetailsForTicket(Guid? id)
        {
            return this._ticketRepository.Get(id);
        }

        public AddToCardDto GetShoppingCartInfo(Guid? id)
        {
            var ticket = this.GetDetailsForTicket(id);
            AddToCardDto model = new AddToCardDto
            {
                SelectedTicket = ticket,
                TicketId = ticket.Id,
                Quantity = 1
            };
            return model;
        }

        public void UpdeteExistingTicket(Ticket t)
        {
            this._ticketRepository.Update(t);
        }
    }
}
