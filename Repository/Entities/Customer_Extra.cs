namespace Repository.Contexts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Customer_Extra
    {
        [Key]
        [StringLength(50)]
        public string CustCode { get; set; }

        public int? NumEmployers { get; set; }

        [Column(TypeName = "text")]
        public string Competitor1 { get; set; }

        [Column(TypeName = "text")]
        public string Competitor2 { get; set; }

        [Column(TypeName = "text")]
        public string Competitor3 { get; set; }

        [Column(TypeName = "text")]
        public string Customers { get; set; }

        [StringLength(50)]
        public string DecisionmakerCPcode { get; set; }

        [StringLength(50)]
        public string CoreProduct1 { get; set; }

        [StringLength(50)]
        public string CoreProduct2 { get; set; }

        [StringLength(50)]
        public string CoreProduct3 { get; set; }

        [StringLength(50)]
        public string CoreProduct4 { get; set; }

        [StringLength(50)]
        public string CoreProduct5 { get; set; }

        [Column(TypeName = "money")]
        public decimal? YearRevenue { get; set; }

        [StringLength(2083)]
        public string GoogleStreetView { get; set; }

        public int? NumLocations { get; set; }

        [StringLength(50)]
        public string Area { get; set; }

        [Column(TypeName = "text")]
        public string Whatwecando { get; set; }

        [Column(TypeName = "text")]
        public string Actualnews { get; set; }

        [StringLength(50)]
        public string Tags { get; set; }

        [StringLength(50)]
        public string CampaignLink2Table { get; set; }

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

        [StringLength(100)]
        public string GUID { get; set; }

        [StringLength(50)]
        public string UserID { get; set; }
    }
}
