using System;
using System.Collections.Generic;

namespace OnlineCinema.Domain.DomainModels
{
    public class TicketInOrder : BaseEntity
    {
        public Guid ProductId { get; set; }

        public Ticket OrderedProduct { get; set; }

        public Guid OrderId { get; set; }

        public Order UserOrder { get; set; }

        public int Quantity { get; set; }

    }
}
