using System;
using System.Collections.Generic;

namespace BookStore.DATA.EF.Models
{
    public partial class Type
    {
        public Type()
        {
            Books = new HashSet<Book>();
        }

        public int TypeId { get; set; }
        public string TypeDescription { get; set; } = null!;

        public virtual ICollection<Book> Books { get; set; }
    }
}
