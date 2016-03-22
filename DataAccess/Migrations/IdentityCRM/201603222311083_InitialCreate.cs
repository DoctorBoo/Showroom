namespace DataAccess.Migrations.IdentityCRM
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Email = c.String(nullable: false, maxLength: 50),
                        Address = c.String(),
                        Zipcode = c.String(),
                        Name = c.String(),
                        Nick = c.String(),
                        Telephone = c.String(),
                        City = c.String(),
                        ChaimberOfCommerce = c.String(),
                        AFM = c.String(),
                        BankAccount = c.String(),
                    })
                .PrimaryKey(t => t.Email);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Employees");
        }
    }
}
