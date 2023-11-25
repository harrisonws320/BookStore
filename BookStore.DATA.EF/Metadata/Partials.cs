using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


namespace BookStore.DATA.EF.Models/*.Metadata*/
{
    // internal class Partials
    //{
    //}

    [ModelMetadataType(typeof(AuthorMetadata))]
    public partial class Author { /* custom properties here */ }

    [ModelMetadataType(typeof(BookMetadata))]
    public partial class Book 
    {
        [NotMapped]
        public IFormFile? Image { get; set; }

    }

    [ModelMetadataType(typeof(BuyerMetadata))]
    public partial class Buyer { }

    [ModelMetadataType(typeof(ConditionMetadata))]
    public partial class Condition { }

    [ModelMetadataType(typeof(GenreMetadata))]
    public partial class Genre { } 
    
    [ModelMetadataType(typeof(OrderMetadata))]
    public partial class Order { }
    
    [ModelMetadataType(typeof(PublisherMetadata))]
    public partial class Publisher { }

    [ModelMetadataType(typeof(TypeMetadata))]
    public partial class Type { }
}

//Models -> Entity Models : Don't make changes here
//Metadata -> Metadata classes : Data annotation only
//            Partial classes : bind metadata to the entity models - also custom props
