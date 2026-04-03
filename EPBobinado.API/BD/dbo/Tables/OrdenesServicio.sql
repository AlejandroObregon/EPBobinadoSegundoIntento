CREATE TABLE [dbo].[OrdenesServicio] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [MotorId]     INT             NULL,
    [Estado]      NVARCHAR (50)   NOT NULL,
    [TecnicoId]   INT             NULL,
    [CreadoEn]    DATETIME2 (7)   DEFAULT (sysdatetime()) NULL,
    [UsuarioId]   INT             NULL,
    [Descripcion] NVARCHAR (MAX)  NULL,
    [Costo]       DECIMAL (10, 2) NULL,
    [FechaCita]   DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([MotorId]) REFERENCES [dbo].[Motores] ([Id]),
    FOREIGN KEY ([TecnicoId]) REFERENCES [dbo].[Usuarios] ([Id]),
    FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuarios] ([Id])
);

