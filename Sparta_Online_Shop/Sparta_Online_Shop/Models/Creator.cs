namespace Sparta_Online_Shop
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Creator
    {
        public int CreatorID { get; set; }

        [StringLength(100)]
        public string CreatorName { get; set; }

        public string ProfileImage { get; set; }

        public string Description { get; set; }

        public string GitHubLink { get; set; }

        public string Website { get; set; }
    }
}
