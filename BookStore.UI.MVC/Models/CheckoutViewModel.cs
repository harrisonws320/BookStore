namespace BookStore.UI.MVC.Models
{
    public class CheckoutViewModel
    {
        public string BuyerId { get; set; } = null!;
        public string BuyerFname { get; set; } = null!;
        public string BuyerLname { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? State { get; set; }
        public string PostalCode { get; set; } = null!;
    }
}
