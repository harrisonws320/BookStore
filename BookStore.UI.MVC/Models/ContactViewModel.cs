using Microsoft.EntityFrameworkCore; //Grants access to [Keyless]
using System.ComponentModel.DataAnnotations; //Grants access to annotations for validation

namespace BookStore.UI.MVC.Models
{
    //If you receive an error when generating a View with this model
    //that requires a primary key, you can add the using statement above
    //and annotates the class with the [Keyless] 
    [Keyless]
    public class ContactViewModel
    {
        //We can use Data Annotations to add validation to our model.
        //This is useful when we have required fields or need certain kinds of information.

        [Required(ErrorMessage = "*Name is required")] //Makes the field required
        public string Name { get; set; }

        [Required(ErrorMessage = "*Email is required")]
        [DataType(DataType.EmailAddress)] //Certain formatting is expected (@ symbol, .com, etc.)
        public string Email { get; set; }

        [Required(ErrorMessage = "*Subject is required")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "*Message is required")]
        [DataType(DataType.MultilineText)] //Denotes this field is larger than a standard textbox (<input> => <textarea>)
        public string Message { get; set; }

        //Mini-Lab:
        //Create the ContactViewModel in your PersonalSite MVC Solution
        //You can copy and paste the properties and data annotations here
        //into that solution. Don't forget the using statement and annotations.
    }
}
