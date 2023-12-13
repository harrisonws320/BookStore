using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public string? Address { get; set; } 
        public string? City { get; set; } 
        public string? State { get; set; }
        public string? PostalCode { get; set; } 
        public string? Country { get; set; }
        public string? Phone { get; set; } 
        public string? Email { get; set; } 

        public virtual ICollection<Order> Orders { get; set; }
    }
}
