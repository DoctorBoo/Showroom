namespace Repository.Contexts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AddressBook")]
    public partial class AddressBook
    {
        [Key]
        [StringLength(50)]
        public string ContactID { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string MidleName { get; set; }

        [StringLength(50)]
        public string Initial { get; set; }

        [StringLength(50)]
        public string CompanyID { get; set; }

        [StringLength(50)]
        public string CompanyName { get; set; }

        [StringLength(50)]
        public string Tel1 { get; set; }

        [StringLength(50)]
        public string Tel2 { get; set; }

        [StringLength(50)]
        public string Fax1 { get; set; }

        [StringLength(50)]
        public string Fax2 { get; set; }

        [StringLength(50)]
        public string Mob1 { get; set; }

        [StringLength(50)]
        public string Mob2 { get; set; }

        [StringLength(50)]
        public string Email1 { get; set; }

        [StringLength(50)]
        public string Email2 { get; set; }

        [Column(TypeName = "text")]
        public string Notes { get; set; }

        public int? Int1 { get; set; }

        public int? Int2 { get; set; }

        [StringLength(50)]
        public string VChar1 { get; set; }

        [StringLength(50)]
        public string VChar2 { get; set; }

        [Column(TypeName = "text")]
        public string Text1 { get; set; }

        [Column(TypeName = "text")]
        public string Text2 { get; set; }
    }
}
