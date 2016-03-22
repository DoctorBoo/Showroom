namespace DataAccess.Crm
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using DomainClasses.Helpers;

    public partial class Identity : DbContext
    {
        public Identity()
            : base("name=Identity")
        {
        }
        //public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        //public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        //public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        //public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        
        public virtual DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
        /// <summary>
        /// Add employees here.
        /// </summary>
        /// <returns></returns>
        public static Employee[] Init()
        {
            Employee[] list = new Employee[] { };
            try
            {
                string users = SubSystem.AppSettings[Constants.UsersFromAppSettings];
                var userList = users.Split(',');
                list = new Employee[userList.Length];
                for (int i = 0; i < userList.Length; i++)
                {
                    list[i] = new Employee();
                    list[i].Email = userList[i];
                }
            }
            catch (Exception ex)
            {
                Log<Identity>.Write.Fatal("No users for application found", ex);
            }
            return list;
        }
    }
}
