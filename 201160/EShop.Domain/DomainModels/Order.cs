using EShop.Domain.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Domain.DomainModels
{
    public class Order : BaseEntry
    {

        public string UserId { get; set; }
        public User User { get; set; }
        
        public IEnumerable<TicketInOrder> TicketInOrders { get; set; }

    }
}
