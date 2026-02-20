CREATE TABLE [dbo].[Productos] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [Nombre]      NVARCHAR (100)  NOT NULL,
    [Categoria]   NVARCHAR (50)   NULL,
    [Stock]       INT             DEFAULT ((0)) NULL,
    [StockMinimo] INT             DEFAULT ((0)) NULL,
    [Precio]      DECIMAL (10, 2) NULL,
    [Activo]      BIT             DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

