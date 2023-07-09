using System;
namespace OnlineCinema.Domain.DomainModels
{
    public class TicketsInShoppingCarts : BaseEntity
    {
        public Guid ProductId { get; set; }
        public virtual Ticket Product { get; set; }

        public Guid ShoppingCartId { get; set; }
        public virtual ShoppingCart UserCart{ get; set; }

        public int Quantity { get; set; }
    }
}
