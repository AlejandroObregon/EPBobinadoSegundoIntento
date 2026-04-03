CREATE TABLE [dbo].[MovimientosInventario] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [ProductoId] INT           NOT NULL,
    [OrdenId]    INT           NULL,
    [Tipo]       NVARCHAR (10) NULL,
    [Cantidad]   INT           NOT NULL,
    [Fecha]      DATETIME2 (7) DEFAULT (sysdatetime()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CHECK ([Tipo]='SALIDA' OR [Tipo]='ENTRADA'),
    FOREIGN KEY ([OrdenId]) REFERENCES [dbo].[OrdenesServicio] ([Id]),
    FOREIGN KEY ([ProductoId]) REFERENCES [dbo].[Productos] ([Id])
);

