CREATE TABLE [dbo].[Usuarios] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Cedula]       NVARCHAR (20)  NOT NULL,
    [Nombre]       NVARCHAR (100) NOT NULL,
    [Email]        NVARCHAR (100) NOT NULL,
    [PasswordHash] NVARCHAR (255) NOT NULL,
    [RolId]        INT            NOT NULL,
    [DireccionId]  INT            NULL,
    [Telefono]     NVARCHAR (20)  NULL,
    [Activo]       BIT            DEFAULT ((1)) NULL,
    [CreadoEn]     DATETIME2 (7)  DEFAULT (sysdatetime()) NULL,
    [Username]     NVARCHAR (150) NULL,
    [LastLogin]    DATETIME2 (7)  NULL,
    [IsSuperuser]  BIT            DEFAULT ((0)) NULL,
    [IsStaff]      BIT            DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([DireccionId]) REFERENCES [dbo].[Direcciones] ([Id]),
    FOREIGN KEY ([RolId]) REFERENCES [dbo].[Roles] ([Id]),
    UNIQUE NONCLUSTERED ([Cedula] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC)
);

