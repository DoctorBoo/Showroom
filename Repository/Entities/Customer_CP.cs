namespace Repository.Contexts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Customer_CP
    {
        [Required]
        [StringLength(50)]
        public string CustCode { get; set; }

        [Key]
        [StringLength(50)]
        public string CPCode { get; set; }

        [StringLength(50)]
        public string MidleName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string Initial { get; set; }

        public int? Gender { get; set; }

        [StringLength(50)]
        public string CPFunction { get; set; }

        [StringLength(50)]
        public string CPEmail { get; set; }

        [StringLength(50)]
        public string CPEmail2 { get; set; }

        [StringLength(50)]
        public string CPPhone { get; set; }

        [StringLength(50)]
        public string CPPhone2 { get; set; }

        [StringLength(50)]
        public string CPFax1 { get; set; }

        [StringLength(50)]
        public string CPFax2 { get; set; }

        [StringLength(50)]
        public string CPMobile { get; set; }

        [StringLength(50)]
        public string CPMobile2 { get; set; }

        [Column(TypeName = "text")]
        public string CPNotes { get; set; }

        [StringLength(50)]
        public string VChar1 { get; set; }

        [StringLength(50)]
        public string VChar2 { get; set; }

        [StringLength(50)]
        public string VChar3 { get; set; }

        public int? Int1 { get; set; }

        public int? Int2 { get; set; }

        public int? Int3 { get; set; }

        public double? Float1 { get; set; }

        public double? Float2 { get; set; }

        [Column(TypeName = "text")]
        public string Text1 { get; set; }

        [Column(TypeName = "text")]
        public string Text2 { get; set; }

        [Column(TypeName = "text")]
        public string Text3 { get; set; }

        public DateTime? Date1 { get; set; }

        public DateTime? Date2 { get; set; }
    }
}
