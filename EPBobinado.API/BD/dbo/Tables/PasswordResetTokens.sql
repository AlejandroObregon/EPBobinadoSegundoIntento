CREATE TABLE [dbo].[PasswordResetTokens] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UsuarioId]  INT            NOT NULL,
    [Token]      NVARCHAR (100) NOT NULL,
    [Expiracion] DATETIME       NOT NULL,
    [Usado]      BIT            DEFAULT ((0)) NOT NULL,
    [CreadoEn]   DATETIME       DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuarios] ([Id]),
    UNIQUE NONCLUSTERED ([Token] ASC)
);

