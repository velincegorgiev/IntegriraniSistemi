using EShop.Domain.DomainModels;
using EShop.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EShop.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Order> entities;
        string errorMessage = string.Empty;

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Order>();
        }
        public IEnumerable<Order> GetAll()
        {
            return entities.AsEnumerable();
        }

        public List<Order> GetAllOrders()
        {
            return entities
                .Include(z => z.TicketInOrders)
                .Include(z => z.User)
                .Include("TicketInOrders.OrderTicket")
                .ToListAsync().Result;
        }

        public Order GetOrderDetails(BaseEntry model)
        {
            return entities
               .Include(z => z.TicketInOrders)
               .Include(z => z.User)
               .Include("TicketInOrders.OrderTicket")
               .SingleOrDefaultAsync(z => z.Id == model.Id).Result;
        }
    }
}
