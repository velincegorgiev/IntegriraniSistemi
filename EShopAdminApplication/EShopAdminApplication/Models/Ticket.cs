using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShopAdminApplication.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string MovieName { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public int Rating { get; set; }
    }
}
