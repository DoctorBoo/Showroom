Steps SignalR
---------------
1. install-package Microsoft.AspNet.SignalR  
    [Howto](http://www.asp.net/signalr/overview/getting-started/tutorial-getting-started-with-signalr)
2. Install-Package jQuery.UI.Combined

Steps Migration
---------------

1. enable-migrations -contexttypename class1 -migrationsdirectory path\class1Migrations
2. add-migration -ConfigurationTypename namespace.class1Migrations.Configuration "InitialCreate"
3. update-database  -ConfigurationTypename namespace.class1Migrations.Configuration -verbose

Change identityUser
-------------------

1. Enable-Migrations –EnableAutomaticMigrations
2. Add-Migration Init 
3. Update-Database