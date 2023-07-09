using System;
using OnlineCinema.Domain.DomainModels;


namespace OnlineCinema.Domain.DTO
{
    public class AddToShoppingCartDto
    {
        public Ticket SelectedProduct { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
