CREATE TABLE [dbo].[DiagnosticosIniciales] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [OrdenId]     INT            NOT NULL,
    [Descripcion] NVARCHAR (MAX) NOT NULL,
    [CreadoEn]    DATETIME2 (7)  DEFAULT (sysdatetime()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_DiagnosticoInicial_Orden] FOREIGN KEY ([OrdenId]) REFERENCES [dbo].[OrdenesServicio] ([Id])
);

