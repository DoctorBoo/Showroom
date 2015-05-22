namespace Repository.Contexts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [StringLength(50)]
        public string UserID { get; set; }

        [StringLength(50)]
        public string UserGroup { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string MidleName { get; set; }

        [StringLength(50)]
        public string Initial { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        public int? AccessLevel { get; set; }

        [StringLength(50)]
        public string Role { get; set; }

        [StringLength(3)]
        public string Lang { get; set; }

        [StringLength(50)]
        public string Func { get; set; }

        [Column(TypeName = "text")]
        public string Notes { get; set; }

        public int? NotifTimerInterval { get; set; }

        public int? DueDateDay { get; set; }

        public int? DueDateHrs { get; set; }

        public int? DueDateMin { get; set; }

        public int? ReminderDay { get; set; }

        public int? ReminderHrs { get; set; }

        public int? ReminderMin { get; set; }

        public int? SnoozeDay { get; set; }

        public int? SnoozeHrs { get; set; }

        public int? SnoozeMin { get; set; }

        public DateTime? LoginTime { get; set; }

        public DateTime? LogoutTime { get; set; }

        public int? LogCount { get; set; }

        public int? LogFlag { get; set; }

        public int? LogErrCount { get; set; }

        public int? LogRelCount { get; set; }

        [StringLength(50)]
        public string PictureFile { get; set; }

        [Column(TypeName = "text")]
        public string LocalDir { get; set; }

        [Column(TypeName = "text")]
        public string wavFile { get; set; }

        [Column(TypeName = "text")]
        public string wallpaper { get; set; }

        [StringLength(50)]
        public string VChar1 { get; set; }

        [StringLength(50)]
        public string VChar2 { get; set; }

        [StringLength(50)]
        public string VChar3 { get; set; }

        [StringLength(50)]
        public string VChar4 { get; set; }

        [StringLength(50)]
        public string VChar5 { get; set; }

        public int? Int1 { get; set; }

        public int? Int2 { get; set; }

        public int? Int3 { get; set; }

        public int? Int4 { get; set; }

        public int? Int5 { get; set; }

        public double? Float1 { get; set; }

        public double? Float2 { get; set; }

        [Column(TypeName = "text")]
        public string Text1 { get; set; }

        [Column(TypeName = "text")]
        public string Text2 { get; set; }

        [Column(TypeName = "text")]
        public string Text3 { get; set; }

        [Column(TypeName = "text")]
        public string Text4 { get; set; }

        [Column(TypeName = "text")]
        public string Text5 { get; set; }

        public bool? isDriver { get; set; }

        public bool? isWorker { get; set; }

        public bool? isDeliveryMan { get; set; }
    }
}
