using System;
using System.Collections.Generic;

namespace BookStore.DATA.EF.Models
{
    public partial class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }

        public int AuthorId { get; set; }
        public string AuthorFname { get; set; } = null!;
        public string? AuthorLname { get; set; }
        public int? Born { get; set; }
        public int? Died { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string Country { get; set; } = null!;
        public string? Religion { get; set; }
        public string? Movement { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
