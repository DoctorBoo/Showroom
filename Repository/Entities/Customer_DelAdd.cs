namespace Repository.Contexts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Customer_DelAdd
    {
        [Required]
        [StringLength(50)]
        public string CustCode { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DelCode { get; set; }

        [StringLength(50)]
        public string DefaultAdd { get; set; }

        [StringLength(50)]
        public string DelName { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Number { get; set; }

        [StringLength(50)]
        public string ZipCode { get; set; }

        [StringLength(50)]
        public string ZipExt { get; set; }

        [StringLength(50)]
        public string PostBus { get; set; }

        [StringLength(50)]
        public string ZipCode2 { get; set; }

        [StringLength(50)]
        public string ZipExt2 { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(50)]
        public string Telp { get; set; }

        [StringLength(50)]
        public string Telp2 { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }

        [StringLength(50)]
        public string Fax2 { get; set; }

        [Column(TypeName = "text")]
        public string Notes { get; set; }

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
