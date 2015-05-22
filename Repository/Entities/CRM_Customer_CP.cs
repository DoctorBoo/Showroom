namespace Repository.Contexts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CRM_Customer_CP
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string CustCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string Division { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string CPCode { get; set; }
    }
}
