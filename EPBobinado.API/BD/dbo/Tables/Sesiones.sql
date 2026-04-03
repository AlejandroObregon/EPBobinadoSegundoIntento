CREATE TABLE [dbo].[Sesiones] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [UsuarioId]       INT            NOT NULL,
    [Token]           NVARCHAR (255) NOT NULL,
    [Inicio]          DATETIME2 (7)  DEFAULT (sysdatetime()) NULL,
    [UltimaActividad] DATETIME2 (7)  NULL,
    [Activa]          BIT            DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuarios] ([Id]),
    UNIQUE NONCLUSTERED ([Token] ASC)
);

