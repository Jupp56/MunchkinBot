
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/01/2018 07:37:39
-- Generated from EDMX file: C:\Users\Nick\documents\visual studio 2015\Projects\MunchkinBot\MunchkinBot\MunchkinDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [testdatenbank];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[CardStock]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CardStock];
GO
IF OBJECT_ID(N'[dbo].[DungeonStock]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DungeonStock];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'CardStock'
CREATE TABLE [dbo].[CardStock] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Type] int  NOT NULL,
    [Count] int  NOT NULL,
    [Level] int  NULL,
    [Bonus] nvarchar(max)  NULL,
    [BadThings] nvarchar(max)  NULL,
    [Loot] int  NULL,
    [LvUp] int  NULL,
    [Events] nvarchar(max)  NULL,
    [Traits] nvarchar(max)  NULL,
    [Restrictions] nvarchar(max)  NULL,
    [React] bit  NULL,
    [Description] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'DungeonStock'
CREATE TABLE [dbo].[DungeonStock] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Events] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'CardStock'
ALTER TABLE [dbo].[CardStock]
ADD CONSTRAINT [PK_CardStock]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DungeonStock'
ALTER TABLE [dbo].[DungeonStock]
ADD CONSTRAINT [PK_DungeonStock]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------