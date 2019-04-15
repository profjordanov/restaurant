using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Restaurant.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime RegistrationDate { get; set; }

        public virtual ICollection<Rating> GivenRatings { get; set; } = new HashSet<Rating>();

        public virtual ICollection<Order> MadeOrders { get; set; } = new HashSet<Order>();

        public virtual ICollection<Restaurant> OwnedRestaurants { get; set; } = new HashSet<Restaurant>();
    }
}
