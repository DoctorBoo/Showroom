namespace yFabric.Migrations
{
	using Microsoft.AspNet.Identity;
	using Microsoft.AspNet.Identity.EntityFramework;
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;
	using yFabric.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<yFabric.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
			//AutomaticMigrationDataLossAllowed = true;
            ContextKey = "yFabric.Models.ApplicationDbContext";
        }

        protected override void Seed(yFabric.Models.ApplicationDbContext context)
        {
			var manager = new UserManager<ApplicationUser>(
				new UserStore<ApplicationUser>(
					new ApplicationDbContext()));

			for (int i = 0; i < 4; i++)
			{
				var user = new ApplicationUser()
				{
					UserName = string.Format("User{0}", i.ToString())
				};
				manager.Create(user, string.Format("Password{0}", i.ToString()));
			}
        }
    }
}
