CREATE TABLE [dbo].[Cotizaciones] (
    [Id]       INT             IDENTITY (1, 1) NOT NULL,
    [OrdenId]  INT             NOT NULL,
    [Total]    DECIMAL (10, 2) NULL,
    [Aprobada] BIT             DEFAULT ((0)) NULL,
    [CreadoEn] DATETIME2 (7)   DEFAULT (sysdatetime()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([OrdenId]) REFERENCES [dbo].[OrdenesServicio] ([Id])
);

