using OnlineCinema.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Security.Policy;

namespace OnlineCinema.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public OnlineCinemaApplicationUser User { get; set; }

        public virtual IEnumerable<TicketInOrder> ProductInOrders { get; set; }
    }
}
