using  OnlineCinema.Domain.DomainModels;
using System.Collections.Generic;

namespace OnlineCinema.Domain.DTO
{
    public class ShoppingCartDto
    {
        public List<TicketsInShoppingCarts> Products { get; set; }

        public double TotalPrice { get; set; }
    }
}
