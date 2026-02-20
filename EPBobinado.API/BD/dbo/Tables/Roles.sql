CREATE TABLE [dbo].[Roles] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Nombre]      NVARCHAR (50)  NOT NULL,
    [Descripcion] NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Nombre] ASC)
);

