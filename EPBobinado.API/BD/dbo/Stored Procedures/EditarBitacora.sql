
-- Editar un registro de bitácora
CREATE   PROCEDURE EditarBitacora
    @Id INT,
    @UsuarioId INT,
    @Accion NVARCHAR(100),
    @TablaAfectada NVARCHAR(100),
    @RegistroId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Bitacora
    SET UsuarioId = @UsuarioId,
        Accion = @Accion,
        TablaAfectada = @TablaAfectada,
        RegistroId = @RegistroId
    WHERE Id = @Id;
    
    SELECT @Id AS Id;
END