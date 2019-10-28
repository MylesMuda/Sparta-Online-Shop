using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sparta_Online_Shop
{
    public class ProductReview
    {
        public Product product { get; set; }
        public List<Review> reviews { get; set; }    
    }
}