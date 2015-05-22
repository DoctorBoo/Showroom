namespace Repository.Contexts
{    
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserLogin")]
    public partial class UserLogin : IUser
    {
        [Key]
        [Column(Order = 0)]
        public int UserLoginID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string LoginName { get; set; }

        [Required]
        [StringLength(4000)]
        public string PassWord { get; set; }

        [Required]
        [StringLength(4000)]
        public string Email { get; set; }

        public int RoleId { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ModifyDate { get; set; }

        [Required]
        [StringLength(6)]
        public string ModifiedBy { get; set; }
    }
}
