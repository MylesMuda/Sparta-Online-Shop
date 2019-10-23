namespace Sparta_Online_Shop
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BasketItem
    {
        public int BasketItemID { get; set; }

        public int BasketID { get; set; }

        public int ProductID { get; set; }

        public int? Quantity { get; set; }

        public virtual Basket Basket { get; set; }

        public virtual Product Product { get; set; }
    }
}
