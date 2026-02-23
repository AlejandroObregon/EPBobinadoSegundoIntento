
-- Editar una sesión existente
CREATE   PROCEDURE EditarSesion
    @Id INT,
    @UsuarioId INT,
    @Token NVARCHAR(255),
    @UltimaActividad DATETIME2,
    @Activa BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Sesiones
    SET UsuarioId = @UsuarioId,
        Token = @Token,
        UltimaActividad = @UltimaActividad,
        Activa = @Activa
    WHERE Id = @Id;
    
    SELECT @Id AS Id;
END