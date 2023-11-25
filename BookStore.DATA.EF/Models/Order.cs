using System;
using System.Collections.Generic;

namespace BookStore.DATA.EF.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderBooks = new HashSet<OrderBook>();
        }

        public int OrderId { get; set; }
        public string BuyerId { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public string ShipToName { get; set; } = null!;
        public string ShipCity { get; set; } = null!;
        public string? ShipState { get; set; }
        public string ShipPostalCode { get; set; } = null!;

        public virtual Buyer Buyer { get; set; } = null!;
        public virtual ICollection<OrderBook> OrderBooks { get; set; }
    }
}
