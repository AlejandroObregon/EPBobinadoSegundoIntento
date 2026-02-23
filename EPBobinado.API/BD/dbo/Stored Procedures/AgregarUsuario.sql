
-- Agregar un nuevo usuario
CREATE   PROCEDURE AgregarUsuario
    @Cedula NVARCHAR(20),
    @Nombre NVARCHAR(100),
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(255),
    @RolId INT,
    @DireccionId INT,
    @Telefono NVARCHAR(20),
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Usuarios (Cedula, Nombre, Email, PasswordHash, RolId, DireccionId, Telefono, Activo)
    VALUES (@Cedula, @Nombre, @Email, @PasswordHash, @RolId, @DireccionId, @Telefono, @Activo);
    
    SELECT SCOPE_IDENTITY() AS Id;
END