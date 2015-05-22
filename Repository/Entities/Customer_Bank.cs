namespace Repository.Contexts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Customer_Bank
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string CustCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string BankAcc { get; set; }

        [StringLength(50)]
        public string BankName { get; set; }

        [StringLength(50)]
        public string BankCity { get; set; }

        [StringLength(50)]
        public string BankDefault { get; set; }

        [StringLength(50)]
        public string VChar1 { get; set; }

        [StringLength(50)]
        public string VChar2 { get; set; }

        public int? Int1 { get; set; }

        public int? Int2 { get; set; }

        public double? Float1 { get; set; }

        public double? Float2 { get; set; }

        [Column(TypeName = "text")]
        public string Text1 { get; set; }

        [Column(TypeName = "text")]
        public string Text2 { get; set; }

        public DateTime? Date1 { get; set; }

        public DateTime? Date2 { get; set; }
    }
}
