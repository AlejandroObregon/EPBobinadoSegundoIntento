CREATE TABLE [dbo].[Proveedores] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Nombre]        NVARCHAR (100) NOT NULL,
    [Contacto]      NVARCHAR (100) NULL,
    [CreadoPor]     INT            NULL,
    [FechaCreacion] DATETIME2 (7)  DEFAULT (sysdatetime()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Proveedores_Usuarios] FOREIGN KEY ([CreadoPor]) REFERENCES [dbo].[Usuarios] ([Id])
);

