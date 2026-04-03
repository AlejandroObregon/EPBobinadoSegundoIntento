CREATE TABLE [dbo].[Motores] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [UsuarioId]   INT            NOT NULL,
    [ModeloId]    INT            NOT NULL,
    [NumeroSerie] NVARCHAR (100) NULL,
    [CreadoEn]    DATETIME2 (7)  DEFAULT (sysdatetime()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ModeloId]) REFERENCES [dbo].[ModelosMotor] ([Id]),
    FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuarios] ([Id])
);

