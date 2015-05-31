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

Steps EF
-------------------
1. Install-Package EntityFramework -Version 6.1.3