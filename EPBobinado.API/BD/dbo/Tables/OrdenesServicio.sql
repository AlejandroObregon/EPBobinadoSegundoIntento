CREATE TABLE [dbo].[OrdenesServicio] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [MotorId]   INT           NULL,
    [Estado]    NVARCHAR (50) NOT NULL,
    [TecnicoId] INT           NULL,
    [CreadoEn]  DATETIME2 (7) DEFAULT (sysdatetime()) NULL,
    [UsuarioId] INT NULL, 
    [Descripcion] NVARCHAR(MAX) NULL, 
    [Costo] DECIMAL(10, 2) NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Ordenes_Motores] FOREIGN KEY ([MotorId]) REFERENCES [dbo].[Motores] ([Id]),
    CONSTRAINT [FK_Ordenes_Tecnicos] FOREIGN KEY ([TecnicoId]) REFERENCES [dbo].[Usuarios] ([Id])
);

