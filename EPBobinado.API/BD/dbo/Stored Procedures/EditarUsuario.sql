
-- Editar un usuario existente
CREATE   PROCEDURE EditarUsuario
    @Id INT,
    @Cedula NVARCHAR(20),
    @Nombre NVARCHAR(100),
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(255),
    @RolId INT,
    @DireccionId INT,
    @Telefono NVARCHAR(20),
    @Activo BIT,
    @Username NVARCHAR(150),
    @IsSuperuser BIT,
    @IsStaff BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Usuarios
    SET Cedula = @Cedula,
        Nombre = @Nombre,
        Email = @Email,
        PasswordHash = @PasswordHash,
        RolId = @RolId,
        DireccionId = @DireccionId,
        Telefono = @Telefono,
        Activo = @Activo
    WHERE Id = @Id;
    
    SELECT @Id AS Id;
END