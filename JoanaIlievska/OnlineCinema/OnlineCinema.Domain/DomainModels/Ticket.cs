using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineCinema.Domain.DomainModels
{
    public class Ticket : BaseEntity
    {

        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        
        public string ProductDescription { get; set; }
        public int ProductPrice { get; set; }
        public double ProductRating { get; set; }

        public DateTime movietime { get; set; }
        public virtual ICollection<TicketsInShoppingCarts> TicketsInShoppingCarts { get; set; }

        public virtual IEnumerable<TicketInOrder> ProductInOrders { get; set; }

    }
}
