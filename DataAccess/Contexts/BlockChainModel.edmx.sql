
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/22/2016 21:30:15
-- Generated from EDMX file: C:\Users\Modiwirijo\Source\Repos\Showroom\DataAccess\Contexts\BlockChainModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [AdventureWorks2014];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Mine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Mine];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Mine'
CREATE TABLE [dbo].[Mine] (
    [Id] decimal(38,0) IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(10)  NOT NULL,
    [Value] decimal(18,4)  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [Origin] nvarchar(50)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Mine'
ALTER TABLE [dbo].[Mine]
ADD CONSTRAINT [PK_Mine]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------