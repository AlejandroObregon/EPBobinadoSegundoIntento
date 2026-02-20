CREATE TABLE [dbo].[ModelosMotor] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Nombre]           NVARCHAR (100) NOT NULL,
    [Especificaciones] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

