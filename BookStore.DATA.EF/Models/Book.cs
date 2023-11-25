using System;
using System.Collections.Generic;

namespace BookStore.DATA.EF.Models
{
    public partial class Book
    {
        public Book()
        {
            OrderBooks = new HashSet<OrderBook>();
        }

        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public int? AuthorId { get; set; }
        public int PublicationDate { get; set; }
        public int GenreId { get; set; }
        public bool IsFiction { get; set; }
        public int TypeId { get; set; }
        public int Pages { get; set; }
        public int? PublisherId { get; set; }
        public int ConditionId { get; set; }
        public decimal? BookPrice { get; set; }
        public int? Isbn { get; set; }
        public string? BookImage { get; set; }
        public short? UnitsInStock { get; set; }

        public virtual Author? Author { get; set; }
        public virtual Condition Condition { get; set; } = null!;
        public virtual Genre Genre { get; set; } = null!;
        public virtual Publisher? Publisher { get; set; }
        public virtual Type Type { get; set; } = null!;
        public virtual ICollection<OrderBook> OrderBooks { get; set; }
    }
}
