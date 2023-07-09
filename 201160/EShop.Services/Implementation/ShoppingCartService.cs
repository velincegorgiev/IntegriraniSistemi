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
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<Card> _cartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<EmailMessage> _mailRepository;
        private readonly IRepository<TicketInOrder> _ticketInOrderRepository;
        private readonly IUserRepository _userRepository;
        public ShoppingCartService(IRepository<Card> cartRepository, IRepository<EmailMessage> mailRepository, IUserRepository userRepository, IRepository<Order> orderRepository, IRepository<TicketInOrder> ticketInOrderRepository)
        {
            _cartRepository = cartRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _ticketInOrderRepository = ticketInOrderRepository;
            _mailRepository = mailRepository;
        }

        public bool deleteProductFromSoppingCart(Guid id, string userId)
        {
            if (!string.IsNullOrEmpty(userId) && id != null)
            {
                var loggedInUser = this._userRepository.Get(userId);

                var userCard = loggedInUser.UserCard;
                var itemToDelete = userCard.TickedInCards.Where(z => z.Ticket.Id.Equals(id)).FirstOrDefault();
                userCard.TickedInCards.Remove(itemToDelete);
                this._cartRepository.Update(userCard);
                return true;
            }
            return false;
        }

        public CardDto getShoppingCartInfo(string userId)
        {

            var loggedInUser = this._userRepository.Get(userId);

            var userCard = loggedInUser.UserCard;
            var AllTicket = userCard.TickedInCards.ToList();
            var AllTicketPrice = AllTicket.Select(z => new
            {
                TicketPrice = z.Ticket.Price,
                Quantity = z.Quantity
            }).ToList();

            var totalPrice = 0;
            foreach (var item in AllTicketPrice)
            {
                totalPrice = item.Quantity * item.TicketPrice;
            }
            CardDto scDto = new CardDto
            {
                Tickeds = AllTicket,
                TotalPrice = totalPrice
            };
            return scDto;
        }

        public bool order(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var loggedInUser = this._userRepository.Get(userId);

                var userCard = loggedInUser.UserCard;

                EmailMessage message = new EmailMessage();
                message.MailTo = loggedInUser.Email;
                message.Subject = "successfully created order";
                message.Status = false;

                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    User = loggedInUser,
                    UserId = userId
                };
                this._orderRepository.Insert(order);
                List<TicketInOrder> ticketInOrders = new List<TicketInOrder>();
                var result = userCard.TickedInCards.Select(z => new TicketInOrder
                {
                    TicketId = z.Ticket.Id,
                    OrderTicket = z.Ticket,
                    OrderId = order.Id,
                    UserOrder = order,
                    Quantity=z.Quantity
                }).ToList();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Your order is completed. The order conatins: ");
                var totalPrice = 0.0;
                for (int i = 1; i <= result.Count(); i++)
                {
                    var currentItem = result[i - 1];
                    totalPrice += currentItem.Quantity * currentItem.OrderTicket.Price;
                    sb.AppendLine(i.ToString() + ". " + currentItem.OrderTicket.MovieName + " with quantity of: " + currentItem.Quantity + " and price of: $" + currentItem.OrderTicket.Price);
                }
                sb.AppendLine("Total price for your order: " + totalPrice.ToString());

                message.Content = sb.ToString();
                this._mailRepository.Insert(message);

                ticketInOrders.AddRange(result);
                foreach (var item in ticketInOrders)
                {
                    this._ticketInOrderRepository.Insert(item);
                }
                

                loggedInUser.UserCard.TickedInCards.Clear();
                this._userRepository.Update(loggedInUser);
                return true;
            }
            return false;
        }
    }
}
