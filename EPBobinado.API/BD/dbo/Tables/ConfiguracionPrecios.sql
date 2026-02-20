CREATE TABLE [dbo].[ConfiguracionPrecios] (
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [PrecioHora]         DECIMAL (10, 2) NOT NULL,
    [Margen]             DECIMAL (5, 2)  NOT NULL,
    [ConfiguradoPor]     INT             NOT NULL,
    [FechaConfiguracion] DATETIME2 (7)   DEFAULT (sysdatetime()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ConfigPrecios_Usuarios] FOREIGN KEY ([ConfiguradoPor]) REFERENCES [dbo].[Usuarios] ([Id])
);

