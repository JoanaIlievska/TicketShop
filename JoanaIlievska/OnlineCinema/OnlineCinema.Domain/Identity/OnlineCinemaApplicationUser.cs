using OnlineCinema.Domain.DomainModels;

using System.Collections.Generic;

namespace OnlineCinema.Domain.Identity
{
    public class OnlineCinemaApplicationUser: Microsoft.AspNetCore.Identity.IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }

        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }

        public virtual ShoppingCart UserCart { get; set; }

        public virtual ICollection<Order> Orders { get; set; } 

    }
}
