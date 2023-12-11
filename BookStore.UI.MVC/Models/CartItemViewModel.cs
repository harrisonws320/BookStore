using BookStore.DATA.EF.Models;


namespace BookStore.UI.MVC.Models
{
    public class CartItemViewModel
    {
        public int Qty { get; set; }
        public Book Book { get; set; }
        public CartItemViewModel(int qty, Book book)
        {
            Qty = qty;
            Book = book;
        }
    }
}
