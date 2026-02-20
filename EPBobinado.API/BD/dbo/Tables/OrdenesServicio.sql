CREATE TABLE [dbo].[OrdenesServicio] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [MotorId]   INT           NOT NULL,
    [Estado]    NVARCHAR (50) NOT NULL,
    [TecnicoId] INT           NULL,
    [CreadoEn]  DATETIME2 (7) DEFAULT (sysdatetime()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Ordenes_Motores] FOREIGN KEY ([MotorId]) REFERENCES [dbo].[Motores] ([Id]),
    CONSTRAINT [FK_Ordenes_Tecnicos] FOREIGN KEY ([TecnicoId]) REFERENCES [dbo].[Usuarios] ([Id])
);

