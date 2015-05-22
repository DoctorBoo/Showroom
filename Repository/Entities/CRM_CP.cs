namespace Repository.Contexts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CRM_CP
    {
        [Key]
        [StringLength(50)]
        public string CPCode { get; set; }

        [StringLength(10)]
        public string Title { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string Initial { get; set; }

        [StringLength(50)]
        public string MidleName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public int? Gender { get; set; }

        [StringLength(5)]
        public string Characteristic { get; set; }

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

        [Column(TypeName = "text")]
        public string PhotoFile { get; set; }
    }
}
