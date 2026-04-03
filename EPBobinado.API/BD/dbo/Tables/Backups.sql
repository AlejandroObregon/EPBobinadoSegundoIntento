CREATE TABLE [dbo].[Backups] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Archivo]      NVARCHAR (255) NULL,
    [Fecha]        DATETIME2 (7)  DEFAULT (sysdatetime()) NULL,
    [RealizadoPor] INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([RealizadoPor]) REFERENCES [dbo].[Usuarios] ([Id])
);

