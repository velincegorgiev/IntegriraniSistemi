using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Domain.DomainModels
{
    public class TickedInCard : BaseEntry
    {
        public Guid TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public Guid CardId { get; set; }
        public Card Card { get; set; }
        public int Quantity { get; set; }
    }
}
