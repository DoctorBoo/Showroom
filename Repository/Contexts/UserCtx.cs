namespace Repository.Contexts
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class UserCtx : DbContext
    {
        public UserCtx()
			: base("name=DefaultConnection")
        {
        }

        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<User_Role> User_Role { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .Property(e => e.RoleID)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.AppID)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.VChar1)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.Text1)
                .IsUnicode(false);

            modelBuilder.Entity<User_Role>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<User_Role>()
                .Property(e => e.AppID)
                .IsUnicode(false);

            modelBuilder.Entity<User_Role>()
                .Property(e => e.VChar1)
                .IsUnicode(false);

            modelBuilder.Entity<User_Role>()
                .Property(e => e.VChar2)
                .IsUnicode(false);

            modelBuilder.Entity<User_Role>()
                .Property(e => e.Text1)
                .IsUnicode(false);

            modelBuilder.Entity<User_Role>()
                .Property(e => e.Text2)
                .IsUnicode(false);
        }

		public System.Data.Entity.DbSet<Repository.Contexts.User> Users { get; set; }
    }
}
