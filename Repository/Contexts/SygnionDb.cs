namespace Repository.Contexts
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SygnionDb : DbContext
    {
        public SygnionDb()
			: base("name=DefaultConnection")
        {
        }

        public virtual DbSet<AddressBook> AddressBooks { get; set; }
        public virtual DbSet<ContactMoment> ContactMoments { get; set; }
        public virtual DbSet<CRM_CP> CRM_CP { get; set; }
        public virtual DbSet<CRM_Customer_CP> CRM_Customer_CP { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Customer_Bank> Customer_Bank { get; set; }
        public virtual DbSet<Customer_CP> Customer_CP { get; set; }
        public virtual DbSet<Customer_DelAdd> Customer_DelAdd { get; set; }
        public virtual DbSet<Customer_Extra> Customer_Extra { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<User_Email> User_Email { get; set; }
        public virtual DbSet<UserLogin> UserLogins { get; set; }
        public virtual DbSet<AccountMgr> AccountMgrs { get; set; }
        public virtual DbSet<ContactType> ContactTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddressBook>()
                .Property(e => e.ContactID)
                .IsUnicode(false);

            modelBuilder.Entity<AddressBook>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<AddressBook>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<AddressBook>()
                .Property(e => e.MidleName)
                .IsUnicode(false);

            modelBuilder.Entity<AddressBook>()
                .Property(e => e.Initial)
                .IsUnicode(false);

            modelBuilder.Entity<AddressBook>()
                .Property(e => e.CompanyID)
                .IsUnicode(false);

            modelBuilder.Entity<AddressBook>()
                .Property(e => e.CompanyName)
                .IsUnicode(false);

            modelBuilder.Entity<AddressBook>()
                .Property(e => e.Tel1)
                .IsUnicode(false);

            modelBuilder.Entity<AddressBook>()
                .Property(e => e.Tel2)
                .IsUnicode(false);

            modelBuilder.Entity<AddressBook>()
                .Property(e => e.Fax1)
                .IsUnicode(false);

            modelBuilder.Entity<AddressBook>()
                .Property(e => e.Fax2)
                .IsUnicode(false);

            modelBuilder.Entity<AddressBook>()
                .Property(e => e.Mob1)
                .IsUnicode(false);

            modelBuilder.Entity<AddressBook>()
                .Property(e => e.Mob2)
                .IsUnicode(false);

            modelBuilder.Entity<AddressBook>()
                .Property(e => e.Email1)
                .IsUnicode(false);

            modelBuilder.Entity<AddressBook>()
                .Property(e => e.Email2)
                .IsUnicode(false);

            modelBuilder.Entity<AddressBook>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<AddressBook>()
                .Property(e => e.VChar1)
                .IsUnicode(false);

            modelBuilder.Entity<AddressBook>()
                .Property(e => e.VChar2)
                .IsUnicode(false);

            modelBuilder.Entity<AddressBook>()
                .Property(e => e.Text1)
                .IsUnicode(false);

            modelBuilder.Entity<AddressBook>()
                .Property(e => e.Text2)
                .IsUnicode(false);

            modelBuilder.Entity<ContactMoment>()
                .Property(e => e.ContactType)
                .IsFixedLength();

            modelBuilder.Entity<CRM_CP>()
                .Property(e => e.CPCode)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_CP>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_CP>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_CP>()
                .Property(e => e.Initial)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_CP>()
                .Property(e => e.MidleName)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_CP>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_CP>()
                .Property(e => e.Characteristic)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_CP>()
                .Property(e => e.CPFunction)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_CP>()
                .Property(e => e.CPEmail)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_CP>()
                .Property(e => e.CPEmail2)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_CP>()
                .Property(e => e.CPPhone)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_CP>()
                .Property(e => e.CPPhone2)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_CP>()
                .Property(e => e.CPFax1)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_CP>()
                .Property(e => e.CPFax2)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_CP>()
                .Property(e => e.CPMobile)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_CP>()
                .Property(e => e.CPMobile2)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_CP>()
                .Property(e => e.CPNotes)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_CP>()
                .Property(e => e.PhotoFile)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_Customer_CP>()
                .Property(e => e.CustCode)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_Customer_CP>()
                .Property(e => e.Division)
                .IsUnicode(false);

            modelBuilder.Entity<CRM_Customer_CP>()
                .Property(e => e.CPCode)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.CustCode)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.CustType)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.CustName)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Chamber)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.TaxNo)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.WebSite)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.VChar1)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.VChar2)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.VChar3)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.VChar4)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Text1)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Text2)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Text3)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Bank>()
                .Property(e => e.CustCode)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Bank>()
                .Property(e => e.BankAcc)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Bank>()
                .Property(e => e.BankName)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Bank>()
                .Property(e => e.BankCity)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Bank>()
                .Property(e => e.BankDefault)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Bank>()
                .Property(e => e.VChar1)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Bank>()
                .Property(e => e.VChar2)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Bank>()
                .Property(e => e.Text1)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Bank>()
                .Property(e => e.Text2)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_CP>()
                .Property(e => e.CustCode)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_CP>()
                .Property(e => e.CPCode)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_CP>()
                .Property(e => e.MidleName)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_CP>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_CP>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_CP>()
                .Property(e => e.Initial)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_CP>()
                .Property(e => e.CPFunction)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_CP>()
                .Property(e => e.CPEmail)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_CP>()
                .Property(e => e.CPEmail2)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_CP>()
                .Property(e => e.CPPhone)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_CP>()
                .Property(e => e.CPPhone2)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_CP>()
                .Property(e => e.CPFax1)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_CP>()
                .Property(e => e.CPFax2)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_CP>()
                .Property(e => e.CPMobile)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_CP>()
                .Property(e => e.CPMobile2)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_CP>()
                .Property(e => e.CPNotes)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_CP>()
                .Property(e => e.VChar1)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_CP>()
                .Property(e => e.VChar2)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_CP>()
                .Property(e => e.VChar3)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_CP>()
                .Property(e => e.Text1)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_CP>()
                .Property(e => e.Text2)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_CP>()
                .Property(e => e.Text3)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.CustCode)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.DefaultAdd)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.DelName)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.Number)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.ZipCode)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.ZipExt)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.PostBus)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.ZipCode2)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.ZipExt2)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.Telp)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.Telp2)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.Fax)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.Fax2)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.VChar1)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.VChar2)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.VChar3)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.Text1)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.Text2)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_DelAdd>()
                .Property(e => e.Text3)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.CustCode)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.Competitor1)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.Competitor2)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.Competitor3)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.Customers)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.DecisionmakerCPcode)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.CoreProduct1)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.CoreProduct2)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.CoreProduct3)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.CoreProduct4)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.CoreProduct5)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.YearRevenue)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.Area)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.Whatwecando)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.Actualnews)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.Tags)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.CampaignLink2Table)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.VChar1)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.VChar2)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.VChar3)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.Text1)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.Text2)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.Text3)
                .IsUnicode(false);

            modelBuilder.Entity<Customer_Extra>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserGroup)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.MidleName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Initial)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Role)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Lang)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Func)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.PictureFile)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.LocalDir)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.wavFile)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.wallpaper)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.VChar1)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.VChar2)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.VChar3)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.VChar4)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.VChar5)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Text1)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Text2)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Text3)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Text4)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Text5)
                .IsUnicode(false);

            modelBuilder.Entity<User_Email>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<User_Email>()
                .Property(e => e.Profile)
                .IsUnicode(false);

            modelBuilder.Entity<User_Email>()
                .Property(e => e.EmailAccount)
                .IsUnicode(false);

            modelBuilder.Entity<User_Email>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User_Email>()
                .Property(e => e.EmailAddr)
                .IsUnicode(false);

            modelBuilder.Entity<User_Email>()
                .Property(e => e.SMTP)
                .IsUnicode(false);

            modelBuilder.Entity<User_Email>()
                .Property(e => e.VChar1)
                .IsUnicode(false);

            modelBuilder.Entity<User_Email>()
                .Property(e => e.VChar2)
                .IsUnicode(false);

            modelBuilder.Entity<User_Email>()
                .Property(e => e.Text1)
                .IsUnicode(false);

            modelBuilder.Entity<User_Email>()
                .Property(e => e.Text2)
                .IsUnicode(false);

            modelBuilder.Entity<UserLogin>()
                .Property(e => e.LoginName)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<UserLogin>()
                .Property(e => e.ModifiedBy)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AccountMgr>()
                .Property(e => e.CustName)
                .IsUnicode(false);

            modelBuilder.Entity<AccountMgr>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<ContactType>()
                .Property(e => e.ContactType1)
                .IsFixedLength();
        }
    }
}
