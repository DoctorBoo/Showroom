namespace Repository.Contexts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Customer")]
    public partial class Customer
    {
        [Key]
        [StringLength(50)]
        public string CustCode { get; set; }

        [StringLength(50)]
        public string CustType { get; set; }

        [StringLength(100)]
        public string CustName { get; set; }

        public int? DefDelNo { get; set; }

        public int? DefPriceList { get; set; }

        [StringLength(50)]
        public string Chamber { get; set; }

        [StringLength(50)]
        public string TaxNo { get; set; }

        public double? Tax { get; set; }

        public double? CreditLimit { get; set; }

        public int? CreditTerms { get; set; }

        [StringLength(50)]
        public string WebSite { get; set; }

        [Column(TypeName = "text")]
        public string Notes { get; set; }

        [StringLength(50)]
        public string VChar1 { get; set; }

        [StringLength(50)]
        public string VChar2 { get; set; }

        [StringLength(50)]
        public string VChar3 { get; set; }

        [StringLength(50)]
        public string VChar4 { get; set; }

        public int? Int1 { get; set; }

        public int? Int2 { get; set; }

        public int? Int3 { get; set; }

        public double? Float1 { get; set; }

        public double? Float2 { get; set; }

        public double? Float3 { get; set; }

        [Column(TypeName = "text")]
        public string Text1 { get; set; }

        [Column(TypeName = "text")]
        public string Text2 { get; set; }

        [Column(TypeName = "text")]
        public string Text3 { get; set; }

        public DateTime? Date1 { get; set; }

        public DateTime? Date2 { get; set; }

        public DateTime? Date3 { get; set; }

        public bool? Bit1 { get; set; }

        public bool? Bit2 { get; set; }

        public bool? Bit3 { get; set; }
    }
}
