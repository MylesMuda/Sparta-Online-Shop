namespace Sparta_Online_Shop
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PayPalTransaction
    {
        public int PayPalTransactionID { get; set; }

        public int? PaymentTypeID { get; set; }

        public int? OrderID { get; set; }

        [Required]
        [StringLength(100)]
        public string PayPalOrderID { get; set; }

        public decimal Amount { get; set; }

        [Required]
        [StringLength(100)]
        public string CaptureID { get; set; }

        public virtual Order Order { get; set; }

        public virtual PaymentType PaymentType { get; set; }
    }
}
