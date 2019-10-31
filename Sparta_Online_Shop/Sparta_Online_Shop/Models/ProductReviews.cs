using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sparta_Online_Shop
{
    public class ProductReviews
    {
        public Product Product { get; set; }
        public List<Review> Reviews { get; set; }    
    }
}