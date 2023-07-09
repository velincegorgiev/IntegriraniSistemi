using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Domain.DomainModels
{
    public class Ticket : BaseEntry
    {

        [Required]
        public string MovieName { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public int Rating { get; set; }
        public virtual ICollection<TickedInCard> TickedInCards { get; set; }
        public IEnumerable<TicketInOrder> TicketInOrders { get; set; }
    }
}
