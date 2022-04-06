CREATE TABLE [EmailTemplate] (

   [Id] int IDENTITY(1,1) NOT NULL,
   [Code] nvarchar(50) NOT NULL,
   [Name] nvarchar(50) NOT NULL,
   [Translate] nvarchar(max) NULL,
   [CustomTemplate] nvarchar(max) NOT NULL

   CONSTRAINT [PK_EmailTemplate] PRIMARY KEY CLUSTERED ([Id] ASC)
);
