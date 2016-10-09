namespace yFabric.Migrations.ApplicationDbContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MyUsers",
                c => new
                    {
                        email = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.email);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MyUsers");
        }
    }
}
