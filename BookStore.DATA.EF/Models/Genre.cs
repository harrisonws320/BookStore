using System;
using System.Collections.Generic;

namespace BookStore.DATA.EF.Models
{
    public partial class Genre
    {
        public Genre()
        {
            Books = new HashSet<Book>();
        }

        public int GenreId { get; set; }
        public string GenreName { get; set; } = null!;

        public virtual ICollection<Book> Books { get; set; }
    }
}
