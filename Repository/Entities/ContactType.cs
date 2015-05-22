namespace Repository.Contexts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ContactType
    {
        [Key]
        [Column("ContactType")]
        [StringLength(50)]
        public string ContactType1 { get; set; }
    }
}
