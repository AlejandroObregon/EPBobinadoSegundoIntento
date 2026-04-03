CREATE TABLE [dbo].[Bitacora] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [UsuarioId]     INT            NULL,
    [Accion]        NVARCHAR (100) NOT NULL,
    [TablaAfectada] NVARCHAR (100) NULL,
    [RegistroId]    INT            NULL,
    [Fecha]         DATETIME2 (7)  DEFAULT (sysdatetime()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuarios] ([Id])
);

