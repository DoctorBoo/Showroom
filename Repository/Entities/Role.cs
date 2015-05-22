namespace Repository.Contexts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Role")]
    public partial class Role
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string RoleID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string AppID { get; set; }

        [StringLength(50)]
        public string VChar1 { get; set; }

        public int? Int1 { get; set; }

        [Column(TypeName = "text")]
        public string Text1 { get; set; }

        public DateTime? Date1 { get; set; }
    }
}
