namespace Sparta_Online_Shop
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Review
    {
        public int ReviewID { get; set; }

        public int? UserID { get; set; }

        public int? ProductID { get; set; }

        public int Rating { get; set; }

        public string ReviewText { get; set; }

        public DateTime DateOfReview { get; set; }
        
        public bool? Flagged { get; set; }

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
}
