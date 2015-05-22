Steps Migration
---------------

1. enable-migrations -contexttypename class1 -migrationsdirectory path\class1Migrations
2. add-migration -ConfigurationTypename namespace.class1Migrations.Configuration "InitialCreate"
3. update-database  -ConfigurationTypename namespace.class1Migrations.Configuration -verbose