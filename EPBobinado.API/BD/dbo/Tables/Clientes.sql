CREATE TABLE [dbo].[Clientes] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Nombre]    NVARCHAR (100) NOT NULL,
    [Telefono]  NVARCHAR (20)  NULL,
    [Email]     NVARCHAR (100) NULL,
    [Direccion] NVARCHAR (255) NULL,
    [Activo]    BIT            DEFAULT ((1)) NULL,
    [CreadoEn]  DATETIME2 (7)  DEFAULT (sysdatetime()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

