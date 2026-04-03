CREATE TABLE [dbo].[Pagos] (
    [Id]         INT             IDENTITY (1, 1) NOT NULL,
    [FacturaId]  INT             NOT NULL,
    [Monto]      DECIMAL (10, 2) NOT NULL,
    [MetodoPago] NVARCHAR (50)   NULL,
    [Fecha]      DATETIME2 (7)   DEFAULT (sysdatetime()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([FacturaId]) REFERENCES [dbo].[Facturas] ([Id])
);

