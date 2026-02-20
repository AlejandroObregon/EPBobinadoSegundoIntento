CREATE TABLE [dbo].[Motores] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [ClienteId]   INT            NOT NULL,
    [ModeloId]    INT            NOT NULL,
    [NumeroSerie] NVARCHAR (100) NULL,
    [CreadoEn]    DATETIME2 (7)  DEFAULT (sysdatetime()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Motores_Clientes] FOREIGN KEY ([ClienteId]) REFERENCES [dbo].[Clientes] ([Id]),
    CONSTRAINT [FK_Motores_Modelos] FOREIGN KEY ([ModeloId]) REFERENCES [dbo].[ModelosMotor] ([Id])
);

