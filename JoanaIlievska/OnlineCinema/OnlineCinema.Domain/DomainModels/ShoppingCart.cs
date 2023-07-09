using OnlineCinema.Domain.Identity;
using System;
using System.Collections.Generic;

namespace OnlineCinema.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public string OwnerId { get; set; }
        public virtual OnlineCinemaApplicationUser Owner { get; set; }

        public virtual ICollection<TicketsInShoppingCarts> TicketsInShoppingCarts { get; set; }

    }
}
