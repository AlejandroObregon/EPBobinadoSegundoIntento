CREATE TABLE [dbo].[Diagnosticos] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [OrdenId]  INT            NOT NULL,
    [Detalle]  NVARCHAR (MAX) NOT NULL,
    [CreadoEn] DATETIME2 (7)  DEFAULT (sysdatetime()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([OrdenId]) REFERENCES [dbo].[OrdenesServicio] ([Id])
);

