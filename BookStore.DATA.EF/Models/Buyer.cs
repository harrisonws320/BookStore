using System;
using System.Collections.Generic;

namespace BookStore.DATA.EF.Models
{
    public partial class Buyer
    {
        public Buyer()
        {
            Orders = new HashSet<Order>();
        }

        public string BuyerId { get; set; } = null!;
        public string BuyerFname { get; set; } = null!;
        public string BuyerLname { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? State { get; set; }
        public string PostalCode { get; set; } = null!;
        public string? Country { get; set; }
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
