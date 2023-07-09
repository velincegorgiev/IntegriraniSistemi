using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShopAdminApplication.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public IEnumerable<TicketInOrder> TicketInOrders { get; set; }
    }
}
