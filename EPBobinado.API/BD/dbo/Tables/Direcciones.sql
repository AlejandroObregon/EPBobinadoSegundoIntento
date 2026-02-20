CREATE TABLE [dbo].[Direcciones] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Provincia]    NVARCHAR (100) NOT NULL,
    [Canton]       NVARCHAR (100) NOT NULL,
    [Distrito]     NVARCHAR (100) NOT NULL,
    [CodigoPostal] NVARCHAR (20)  NULL,
    [Descripcion]  NVARCHAR (255) NULL,
    [CreadoEn]     DATETIME2 (7)  DEFAULT (sysdatetime()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

