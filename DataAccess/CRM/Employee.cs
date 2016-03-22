namespace DataAccess.Crm
{
    using DomainClasses.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Security.Principal;

    public partial class Employee : Identity, IEntity

    {
        [Key]
        [StringLength(50)]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Zipcode { get; set; }

        public string Name{ get; set; }
        public string Nick { get; set; }
        public string Telephone { get; set; }
        public string City { get; set; }
        public string ChaimberOfCommerce { get; set; }
        public string AFM { get; set; }
        public string BankAccount { get; set; }
    }
}
