using EShop.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Domain.DomainModels
{
    public class Card : BaseEntry
    {
        public string OwnerId { get; set; }
        public virtual User Owner { get; set; }
        public virtual ICollection<TickedInCard> TickedInCards { get; set; }
    }
}
