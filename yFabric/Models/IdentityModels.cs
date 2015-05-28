using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Repository.Contexts;
using System.Security.Principal;
using System.Linq;

namespace yFabric.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Hometown { get; set; }
		
		public string Nick { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("UserCtx", throwIfV1Schema: false)
        {            
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

	public static class Extensions
	{
		/// <summary>
		/// Fallback is User.Identity.Name
		/// </summary>
		/// <param name="User"></param>
		/// <returns></returns>
		public static string GetNick (this IPrincipal User)
		{
			string nickName = string.Empty;
			if (User.Identity.IsAuthenticated)
			{
				var usersContext = new ApplicationDbContext();
				var appUser = (from u in usersContext.Users
								where u.UserName.Equals(User.Identity.Name)
								select new { Name = u.Nick!= null && u.Nick.Trim() != "" ? u.Nick : User.Identity.Name}).FirstOrDefault();								
				nickName = appUser.Name;
			}
			
			return nickName;
		}
	}
}