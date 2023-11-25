using System;
using System.Collections.Generic;

namespace BookStore.DATA.EF.Models
{
    public partial class Condition
    {
        public Condition()
        {
            Books = new HashSet<Book>();
        }

        public int ConditionId { get; set; }
        public string ConditionDescription { get; set; } = null!;

        public virtual ICollection<Book> Books { get; set; }
    }
}
