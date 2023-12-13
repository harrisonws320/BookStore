using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Step 1:
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using BookStore.DATA.EF.Models;
//Step 2:

namespace BookStore.DATA.EF.Models/*.Metadata*/
{
    public class AuthorMetadata
    {
        [Key]
        public int AuthorId { get; set; }

        [MaxLength(15, ErrorMessage = "First Name cannot exceed 50 characters")]
        [Display(Name = "First Name")]
        public string? AuthorFname { get; set; } 

        [Required(ErrorMessage = "Last Name is required")]
        [MaxLength(15, ErrorMessage = "Last Name cannot exceed 50 characters")]
        [Display(Name = "Last Name")]
        public string? AuthorLname { get; set; } = null!;

        [Display(Name = "Born")]
        public int? Born { get; set; }

        [Display(Name = "Died")]
        public int? Died { get; set; }

        [Display(Name = "City")]
        [MaxLength(15, ErrorMessage = "City cannot exceed 15 characters")]
        public string? City { get; set; }

        [StringLength(2, MinimumLength = 2)]
        [Display(Name = "State")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        public string? State { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [MaxLength(20, ErrorMessage = "Country cannot exceed 20 characters")]
        public string Country { get; set; } = null!;

        [Display(Name = "Belief System")]
        [MaxLength(30, ErrorMessage = "Religion cannot exceed 30 characters")]
        public string? Religion { get; set; }

        [Display(Name = "Ascribed Movement")]
        [MaxLength(30, ErrorMessage = "Movement cannot exceed 30 characters")]
        public string? Movement { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }

    public class BookMetadata
    {
        [Key]

        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = null!;

        [Display(Name = "Author")]
        public int? AuthorId { get; set; }

        [Display(Name = "Published")]
        public int PublicationDate { get; set; }

        [Display(Name = "Genre ID")]
        public int GenreId { get; set; }

        [Display(Name = "Is Fiction")]
        public bool IsFiction { get; set; }

        [Display(Name = "Type ID")]
        public int TypeId { get; set; }

        [Display(Name = "Number of Pages")]
        public int Pages { get; set; }

        [Display(Name = "Publisher ID")]
        public int? PublisherId { get; set; }

        [Display(Name = "Condition ID")]
        public int ConditionId { get; set; }

        [Required]
        [Range(0, (double)decimal.MaxValue)]
        [DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = false)]
        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        [DefaultValue(0)]
        public decimal? BookPrice { get; set; }

        [Display(Name = "ISBN")]
        public int? Isbn { get; set; }


        [StringLength(75)]
        [Display(Name = "Image")]
        public string? Image { get; set; }

        [Display(Name = "Units in Stock")]
        public short? UnitsInStock { get; set; }

        [ForeignKey("AuthorId")]
       
        public virtual Author? Author { get; set; }

        [ForeignKey("ConditionId")]
        public virtual Condition Condition { get; set; } = null!;

        [ForeignKey("GenreId")]
        public virtual Genre Genre { get; set; } = null!;

        [ForeignKey("PublisherId")]
        public virtual Publisher? Publisher { get; set; }

        [ForeignKey("TypeId")]
        public virtual Type Type { get; set; } = null!;

        public virtual ICollection<OrderBook> OrderBooks { get; set; }
    }

    public class BuyerMetadata
    {
        [Key]
        [Required(ErrorMessage = "Buyer ID is required")]
        public string BuyerId { get; set; } = null!;

        [Required]
        [Display(Name = "First Name")]
        [StringLength(20)]
        public string BuyerFname { get; set; } = null!;

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(20)]
        public string BuyerLname { get; set; } = null!;

        //[Required(ErrorMessage = "First Name is required")]
        //[MaxLength(20, ErrorMessage = "First Name cannot exceed 20 characters")]
        //[Display(Name = "Buyer First Name")]
        //public string BuyerFname { get; set; } = null!;

        //[Required(ErrorMessage = "Last Name is required")]
        //[MaxLength(20, ErrorMessage = "Last Name cannot exceed 20 characters")]
        //[Display(Name = "Buyer Last Name")]
        //public string BuyerLname { get; set; } = null!;

        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string? City { get; set; }


        [StringLength(2, MinimumLength = 2)]
        [Display(Name = "State")]
        public string? State { get; set; }

        [Required(ErrorMessage = "Postal Code is required")]
        [DataType(DataType.PostalCode)]
        public string? PostalCode { get; set; } 

        public string? Country { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(NullDisplayText = "[None]")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }

    public class ConditionMetadata
    {
        [Key]
        public int ConditionId { get; set; }

        [Required(ErrorMessage = "Condition Description is required")]
        [MaxLength(15, ErrorMessage = "Condition Description cannot exceed 15 characters")]
        [Display(Name = "Book Condition")]
        public string ConditionDescription { get; set; } = null!;

        public virtual ICollection<Book> Books { get; set; }
    }

    public class GenreMetadata
    {
        [Key]
        public int GenreId { get; set; }

        [Required(ErrorMessage = "Genre Name is required")]
        [MaxLength(15, ErrorMessage = "Genre Name cannot exceed 15 characters")]
        [Display(Name = "Genre Name")]
        public string GenreName { get; set; } = null!;

        public virtual ICollection<Book> Books { get; set; }
    }

    public class OrderMetadata
    {
        [Key]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Buyer ID is required")]
        public string BuyerId { get; set; } = null!;

        [Required(ErrorMessage = "Order Date is required")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Ship To Name is required")]
        [MaxLength(100, ErrorMessage = "Ship To Name cannot exceed 100 characters")]
        [Display(Name = "Ship To Name")]
        public string ShipToName { get; set; } = null!;

        [Required(ErrorMessage = "Ship City is required")]
        [MaxLength(50, ErrorMessage = "Ship City cannot exceed 50 characters")]
        [Display(Name = "Ship City")]
        public string ShipCity { get; set; } = null!;

        [StringLength(2, MinimumLength = 2)]
        [Display(Name = "Ship State")]
        public string? ShipState { get; set; }

        [Required(ErrorMessage = "Ship Zip Code is required")]
        [Display(Name = "Ship Zip Code")]
        [DataType(DataType.PostalCode)]
        public string ShipPostalCode { get; set; } = null!;

        [ForeignKey("BuyerId")]
        public virtual Buyer Buyer { get; set; } = null!;

        public virtual ICollection<OrderBook> OrderBooks { get; set; }
    }

    public class PublisherMetadata
    {
        [Key]
        public int PublisherId { get; set; }

        [Required(ErrorMessage = "Publisher Name is required")]
        [MaxLength(30, ErrorMessage = "Publisher Name cannot exceed 30 characters")]
        [Display(Name = "Publisher Name")]
        public string PublisherName { get; set; } = null!;

        public string? Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [MaxLength(15, ErrorMessage = "City cannot exceed 15 characters")]
        public string City { get; set; } = null!;

        [StringLength(2, MinimumLength = 2)]
        public string? State { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [MaxLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
        public string Country { get; set; } = null!;

        [Display(Name = "Zip Code")]
        [DataType(DataType.PostalCode)]
        public string? PostalCode { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [Url(ErrorMessage = "Invalid URL")]
        public string? Website { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }

    public class TypeMetadata
    {
        [Key]
        public int TypeId { get; set; }

        [Required(ErrorMessage = "Type Description is required")]
        [MaxLength(15, ErrorMessage = "Type Description cannot exceed 15 characters")]
        [Display(Name = "Type Description")]
        public string TypeDescription { get; set; } = null!;

        public virtual ICollection<Book> Books { get; set; }
    }
}
