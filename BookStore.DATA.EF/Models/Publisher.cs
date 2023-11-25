using System;
using System.Collections.Generic;

namespace BookStore.DATA.EF.Models
{
    public partial class Publisher
    {
        public Publisher()
        {
            Books = new HashSet<Book>();
        }

        public int PublisherId { get; set; }
        public string PublisherName { get; set; } = null!;
        public string? Address { get; set; }
        public string City { get; set; } = null!;
        public string? State { get; set; }
        public string Country { get; set; } = null!;
        public string? PostalCode { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
