
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server Compact Edition
-- --------------------------------------------------
-- Date Created: 01/25/2018 21:57:15
-- Generated from EDMX file: F:\Programmieren\Visual Studio 2017\Projects\MunchkinBot\MunchkinBot\DBModel.edmx
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- NOTE: if the constraint does not exist, an ignorable error will be reported.
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- NOTE: if the table does not exist, an ignorable error will be reported.
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'CardStock'
CREATE TABLE [CardStock] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(4000)  NOT NULL,
    [Type] int  NOT NULL,
    [Count] int  NOT NULL,
    [Level] int  NULL,
    [Bonus] nvarchar(4000)  NULL,
    [BadThings] nvarchar(4000)  NULL,
    [Loot] int  NULL,
    [LvUp] int  NULL,
    [Events] nvarchar(4000)  NULL,
    [Traits] nvarchar(4000)  NULL,
    [Restrictions] nvarchar(4000)  NULL,
    [React] bit  NULL
);
GO

-- Creating table 'DungeonStock'
CREATE TABLE [DungeonStock] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(4000)  NOT NULL,
    [Boni] nvarchar(4000)  NULL,
    [Events] nvarchar(4000)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'CardStock'
ALTER TABLE [CardStock]
ADD CONSTRAINT [PK_CardStock]
    PRIMARY KEY ([Id] );
GO

-- Creating primary key on [Id] in table 'DungeonStock'
ALTER TABLE [DungeonStock]
ADD CONSTRAINT [PK_DungeonStock]
    PRIMARY KEY ([Id] );
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------