namespace DataAccess.Migrations.IdentityCRM
{
    using DataAccess.Crm;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DataAccess.Crm.Identity>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\IdentityCRM";
        }

        protected override void Seed(DataAccess.Crm.Identity context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Employees.AddOrUpdate(
              p => p.Name,
              new Employee { Name = "Dwight", Email = "sneakyboo@gmail.com" }
            );
            //
        }
    }
}
