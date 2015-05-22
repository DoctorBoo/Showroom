namespace Repository.Contexts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContactMoment")]
    public partial class ContactMoment
    {
        public DateTime ContactDate { get; set; }

        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string CPCode { get; set; }

        public DateTime RegDate { get; set; }

        [Required]
        [StringLength(50)]
        public string CustCode { get; set; }

        [Required]
        [StringLength(50)]
        public string ContactType { get; set; }

        [StringLength(4000)]
        public string Conversation { get; set; }

        [Key]
        [Column(Order = 1)]
        public long UID { get; set; }

        public DateTime? NextAction { get; set; }
    }
}
