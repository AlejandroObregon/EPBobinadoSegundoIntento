
-- Agregar una nueva sesión
CREATE   PROCEDURE AgregarSesion
    @UsuarioId INT,
    @Token NVARCHAR(255),
    @UltimaActividad DATETIME2,
    @Activa BIT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Sesiones (UsuarioId, Token, UltimaActividad, Activa)
    VALUES (@UsuarioId, @Token, @UltimaActividad, @Activa);
    
    SELECT SCOPE_IDENTITY() AS Id;
END