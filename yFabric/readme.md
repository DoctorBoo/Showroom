Steps SignalR
---------------
1. install-package Microsoft.AspNet.SignalR  
    [Howto](http://www.asp.net/signalr/overview/getting-started/tutorial-getting-started-with-signalr)
2. Install-Package jQuery.UI.Combined

Steps Migration (recreate local database)
---------------
0. remove in sql server explorer database && in app_data the mdf && migrations folder
1. update all DbContext base string values
2. enable-migrations -contexttypename class1 -migrationsdirectory path\class1Migrations   
   i.e. enable-migrations -contexttypename yFabric.Models.ApplicationDbContext -migrationsdirectory Migrations\ApplicationDbContextMigrations
3. add-migration -ConfigurationTypename namespace.class1Migrations.Configuration "InitialCreate"   
   i.e. add-migration -ConfigurationTypename yFabric.Migrations.ApplicationDbContextMigrations.Configuration "InitialCreate"
4. update-database  -ConfigurationTypename namespace.class1Migrations.Configuration -verbose   
   i.e. update-database  -ConfigurationTypename yFabric.Migrations.ApplicationDbContextMigrations.Configuration -verbose

Change identityUser
-------------------

1. Enable-Migrations –EnableAutomaticMigrations
2. Add-Migration Init 
3. Update-Database