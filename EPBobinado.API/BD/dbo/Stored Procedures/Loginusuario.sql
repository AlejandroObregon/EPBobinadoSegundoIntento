-- =============================================
-- SP: LoginUsuario
-- Busca un usuario activo por email + passwordHash
-- =============================================
CREATE PROCEDURE LoginUsuario
    @Email        NVARCHAR(100),
    @PasswordHash NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        Id, Cedula, Nombre, Email, PasswordHash,
        RolId, DireccionId, Telefono, Activo, CreadoEn
    FROM Usuarios
    WHERE Email        = @Email
      AND PasswordHash = @PasswordHash
      AND Activo       = 1;
END
GO