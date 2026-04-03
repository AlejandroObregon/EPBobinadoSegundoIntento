CREATE TABLE [dbo].[ConfiguracionImpuestos] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [Porcentaje]         DECIMAL (5, 2) NOT NULL,
    [ConfiguradoPor]     INT            NOT NULL,
    [FechaConfiguracion] DATETIME2 (7)  DEFAULT (sysdatetime()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ConfiguradoPor]) REFERENCES [dbo].[Usuarios] ([Id])
);

